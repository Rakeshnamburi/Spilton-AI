import { redirect } from 'next/navigation';
import { getSession } from '@/lib/backend';
import { PreparationPanel } from '@/components/preparation-panel';
export const dynamic='force-dynamic';
export default async function Page({searchParams}:{searchParams:Promise<{space?:string}>}){const {user,unavailable}=await getSession();if(unavailable)return <main>Spilton is temporarily unavailable. <a href="/prepare">Retry</a></main>;if(!user)redirect('/login');return <PreparationPanel key={(await searchParams).space||"general"} space={(await searchParams).space||""}/>;}
