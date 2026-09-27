import { redirect } from "next/navigation";
import { getSession } from "@/lib/backend";
import { ImageGenerator } from "@/components/image-generator";

export const dynamic = "force-dynamic";
export default async function ImagesPage() {
  const { user, unavailable } = await getSession();
  if (unavailable) return <main>Spilton is temporarily unavailable. <a href="/images">Retry</a></main>;
  if (!user) redirect("/login?returnTo=%2Fimages");
  return <ImageGenerator/>;
}
