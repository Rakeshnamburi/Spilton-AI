import { AuthForm } from "@/components/auth-form";
import type { Metadata } from "next";
export const metadata:Metadata={title:"Sign in",description:"Sign in securely to your Spilton AI workspace.",alternates:{canonical:"/login"}};
export default async function LoginPage({ searchParams }: { searchParams: Promise<{ oauthError?: string }> }) {
  const { oauthError } = await searchParams;
  return <AuthForm mode="login" initialError={oauthError?.slice(0, 240)} />;
}
