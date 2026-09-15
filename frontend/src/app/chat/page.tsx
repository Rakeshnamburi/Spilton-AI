import { redirect } from "next/navigation";
import { Workspace } from "@/components/workspace";
import { getSession } from "@/lib/backend";
export const dynamic = "force-dynamic";
export default async function ChatPage() {
  const { user, unavailable } = await getSession();
  if (unavailable) return <main className="auth-page"><section className="auth-card"><h1>We couldn’t connect</h1><p role="alert">Spilton is temporarily unavailable. Please try again shortly.</p><a className="primary-button" href="/chat">Try again</a></section></main>;
  if (!user) redirect("/login");
  return <Workspace user={user} />;
}
