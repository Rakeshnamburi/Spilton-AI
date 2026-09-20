import { AuthForm } from "@/components/auth-form";
import type { Metadata } from "next";
export const metadata:Metadata={title:"Sign in",description:"Sign in securely to your Spilton AI workspace.",alternates:{canonical:"/login"}};
export default function LoginPage() { return <AuthForm mode="login" />; }
