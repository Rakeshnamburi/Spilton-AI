import { PasswordRecoveryForm } from "@/components/password-recovery-form";
import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "Reset password",
  description: "Recover access to your Spilton AI workspace securely.",
  alternates: { canonical: "/forgot-password" },
};

export default async function ForgotPasswordPage({ searchParams }: { searchParams: Promise<{ email?: string }> }) {
  const { email = "" } = await searchParams;
  return <PasswordRecoveryForm initialEmail={email.slice(0, 254)} />;
}
