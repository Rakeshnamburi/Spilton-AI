import { PasswordRecoveryForm } from "@/components/password-recovery-form";
import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "Reset password",
  description: "Recover access to your Spilton AI workspace securely.",
  alternates: { canonical: "/forgot-password" },
};

export default function ForgotPasswordPage(){return <PasswordRecoveryForm/>;}
