import { NextRequest, NextResponse } from "next/server";
import { AUTH_COOKIE, REFRESH_COOKIE, backendRequest, cookieOptions, getSession, isAllowedOrigin } from "@/lib/backend";
export async function POST(request: NextRequest, { params }: { params: Promise<{ action: string }> }) {
  const { action } = await params;
  if (!["login", "register", "logout", "logout-all", "refresh"].includes(action)) return NextResponse.json({ title: "Not found." }, { status: 404 });
  // Origin validation protects cookie mutations, including login CSRF.
  if (!isAllowedOrigin(request.headers.get("origin")))
    return NextResponse.json({ title: "Request origin is not allowed." }, { status: 403 });
  if (action === "logout" || action === "logout-all") {
    const token = request.cookies.get(AUTH_COOKIE)?.value;
    const refreshToken = request.cookies.get(REFRESH_COOKIE)?.value;
    if (refreshToken && action === "logout") {
      try {
        const result = await backendRequest("/auth/revoke-refresh", { method: "POST", body: JSON.stringify({ refreshToken }) });
        if (!result.ok) return NextResponse.json({ title: "Unable to revoke the session. Please retry." }, { status: 503 });
      } catch { return NextResponse.json({ title: "Unable to revoke the session. Please retry." }, { status: 503 }); }
    }
    if (!token && action === "logout-all") return NextResponse.json({ title: "Sign in again to revoke all sessions." }, { status: 401 });
    if (token) {
      try {
        const upstream = await backendRequest(`/auth/${action}`, { method: "POST", headers: { Authorization: `Bearer ${token}` } });
        if (!upstream.ok && (upstream.status !== 401 || action === "logout-all")) return NextResponse.json({ title: "Unable to revoke the session. Please retry." }, { status: upstream.status === 401 ? 401 : 503 });
      } catch { return NextResponse.json({ title: "Unable to revoke the session. Please retry." }, { status: 503 }); }
    }
    const response = NextResponse.json({ success: true });
    response.cookies.set(AUTH_COOKIE, "", { ...cookieOptions, maxAge: 0 });
    response.cookies.set(REFRESH_COOKIE, "", { ...cookieOptions, path: "/api/auth", maxAge: 0 });
    response.headers.set("Cache-Control", "no-store");
    return response;
  }
  if (action === "refresh") {
    const refreshToken = request.cookies.get(REFRESH_COOKIE)?.value;
    if (!refreshToken) return NextResponse.json({ title: "Please sign in." }, { status: 401 });
    try {
      const upstream = await backendRequest("/auth/refresh", { method: "POST", body: JSON.stringify({ refreshToken }) });
      if (!upstream.ok) return NextResponse.json({ title: "Session could not be renewed. Please sign in." }, { status: upstream.status });
      const data = await upstream.json();
      const response = NextResponse.json({ user: data.user }, { headers: { "Cache-Control": "no-store" } });
      response.cookies.set(AUTH_COOKIE, data.accessToken, { ...cookieOptions, expires: new Date(data.expiresAt) });
      response.cookies.set(REFRESH_COOKIE, data.refreshToken, { ...cookieOptions, path: "/api/auth", expires: new Date(data.refreshExpiresAt) });
      return response;
    } catch { return NextResponse.json({ title: "Session service unavailable. Please retry." }, { status: 503 }); }
  }
  if (!request.headers.get("content-type")?.includes("application/json")) return NextResponse.json({ title: "JSON is required." }, { status: 415 });
  let body;
  try {
    const raw = await request.text();
    if (raw.length > 8192) return NextResponse.json({ title: "Request is too large." }, { status: 413 });
    body = JSON.parse(raw);
  } catch { return NextResponse.json({ title: "Invalid JSON." }, { status: 400 }); }
  try {
    const upstream = await backendRequest(`/auth/${action}`, { method: "POST", body: JSON.stringify(body) });
    const data = await upstream.json().catch(() => ({}));
    if (!upstream.ok) return NextResponse.json({
      title: upstream.status >= 500 ? "Spilton is temporarily unavailable. Please try again shortly."
        : upstream.status === 429 ? "Too many attempts. Please wait a minute and try again." : data.title,
      errors: upstream.status < 500 ? data.errors : undefined,
    }, { status: upstream.status, headers: { "Cache-Control": "no-store" } });
    const response = NextResponse.json({ user: data.user }, { status: upstream.status });
    response.cookies.set(AUTH_COOKIE, data.accessToken, { ...cookieOptions, expires: new Date(data.expiresAt) });
    if (data.refreshToken) response.cookies.set(REFRESH_COOKIE, data.refreshToken, { ...cookieOptions, path: "/api/auth", expires: new Date(data.refreshExpiresAt) });
    response.headers.set("Cache-Control", "no-store");
    return response;
  } catch { return NextResponse.json({ title: "Spilton is temporarily unavailable. Please try again shortly." }, { status: 503 }); }
}
export async function GET(_request: NextRequest, { params }: { params: Promise<{ action: string }> }) {
  if ((await params).action !== "session") return NextResponse.json({ title: "Not found." }, { status: 404 });
  const { user, unavailable } = await getSession();
  return NextResponse.json(user ? { user } : { title: unavailable ? "Spilton is temporarily unavailable." : "Please sign in." },
    { status: user ? 200 : unavailable ? 503 : 401, headers: { "Cache-Control": "no-store" } });
}
