import "server-only";
import { cookies } from "next/headers";
import type { User } from "./api-client";
export const AUTH_COOKIE = "spilton_session";
export const REFRESH_COOKIE = "spilton_refresh";
export const cookieOptions = { httpOnly: true, sameSite: "lax" as const, path: "/", secure: process.env.AUTH_COOKIE_SECURE !== "false" };
export function isAllowedOrigin(origin: string | null) {
  if (!origin) return false;
  return (process.env.APP_ORIGIN || "").split(",").map(value => value.trim()).filter(Boolean).includes(origin);
}
export function backendRequest(path: string, init: RequestInit = {}) {
  const base = process.env.API_BASE_URL;
  if (!base) throw new Error("API_BASE_URL is not configured.");
  return fetch(`${base.replace(/\/$/, "")}/api${path}`, { ...init, cache: "no-store", signal: init.signal ?? AbortSignal.timeout(10000), headers: { "Content-Type": "application/json", ...init.headers } });
}
export async function getSession(): Promise<{ user: User | null; unavailable: boolean }> {
  const token = (await cookies()).get(AUTH_COOKIE)?.value;
  if (!token) return { user: null, unavailable: false };
  try {
    const response = await backendRequest("/auth/me", { headers: { Authorization: `Bearer ${token}` } });
    if (response.status === 401 || response.status === 403) return { user: null, unavailable: false };
    if (!response.ok) return { user: null, unavailable: true };
    return { user: await response.json(), unavailable: false };
  } catch { return { user: null, unavailable: true }; }
}
