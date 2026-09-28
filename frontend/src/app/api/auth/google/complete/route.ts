import { NextRequest, NextResponse } from "next/server";
import { AUTH_COOKIE, REFRESH_COOKIE, backendRequest, cookieOptions } from "@/lib/backend";

export async function GET(request: NextRequest) {
  const ticket = request.nextUrl.searchParams.get("ticket");
  if (!ticket) return NextResponse.redirect(new URL("/login?oauthError=Google+sign-in+session+is+missing.", request.url));
  try {
    const upstream = await backendRequest("/auth/google/exchange", { method: "POST", body: JSON.stringify({ ticket }), signal: AbortSignal.timeout(20000) });
    if (!upstream.ok) return NextResponse.redirect(new URL("/login?oauthError=Google+sign-in+expired.+Please+try+again.", request.url));
    const data = await upstream.json();
    const response = NextResponse.redirect(new URL("/chat", request.url));
    response.cookies.set(AUTH_COOKIE, data.accessToken, { ...cookieOptions, expires: new Date(data.expiresAt) });
    response.cookies.set(REFRESH_COOKIE, data.refreshToken, { ...cookieOptions, path: "/api/auth", expires: new Date(data.refreshExpiresAt) });
    response.headers.set("Cache-Control", "no-store");
    return response;
  } catch {
    return NextResponse.redirect(new URL("/login?oauthError=Google+sign-in+service+is+temporarily+unavailable.", request.url));
  }
}
