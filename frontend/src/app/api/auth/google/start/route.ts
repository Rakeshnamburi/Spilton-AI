import { NextRequest, NextResponse } from "next/server";

export function GET(request: NextRequest) {
  const base = process.env.API_BASE_URL;
  if (!base) return NextResponse.redirect(new URL("/login?oauthError=Google+sign-in+is+not+configured.", request.url));
  return NextResponse.redirect(`${base.replace(/\/$/, "")}/api/auth/google/start`);
}
