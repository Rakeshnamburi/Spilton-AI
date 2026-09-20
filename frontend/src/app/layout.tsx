import type { Metadata } from "next";
import "./globals.css";
const canonical=(process.env.CANONICAL_APP_ORIGIN||"https://spiltonai.vercel.app").replace(/\/$/,"");
export const metadata: Metadata = { metadataBase:new URL(canonical), title:{default:"Spilton AI — Learn, Research & Prepare",template:"%s | Spilton AI"}, description:"A secure AI workspace for learning, document research, coding assistance and government-exam preparation.", applicationName:"Spilton AI", alternates:{canonical:"/"}, openGraph:{type:"website",url:"/",siteName:"Spilton AI",title:"Spilton AI — Learn, Research & Prepare",description:"A secure AI workspace for learning, research and exam preparation."}, robots:{index:true,follow:true} };
export default function RootLayout({ children }: { children: React.ReactNode }) { return <html lang="en"><body>{children}</body></html>; }
