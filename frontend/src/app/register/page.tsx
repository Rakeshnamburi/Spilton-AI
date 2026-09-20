import { AuthForm } from "@/components/auth-form";
import type { Metadata } from "next";
export const metadata:Metadata={title:"Create account",description:"Create your personal Spilton AI workspace.",alternates:{canonical:"/register"}};
export default function RegisterPage() { return <AuthForm mode="register" />; }
