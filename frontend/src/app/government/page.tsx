import {redirect} from 'next/navigation';
import {getSession} from '@/lib/backend';
import {GovernmentPanel} from '@/components/government-panel';
export const dynamic='force-dynamic';
export default async function Page({searchParams}:{searchParams:Promise<{space?:string}>}){const {user,unavailable}=await getSession();if(unavailable)return <main>Spilton is temporarily unavailable. <a href="/government">Retry</a></main>;if(!user)redirect('/login');return <GovernmentPanel key={(await searchParams).space||"general"} space={(await searchParams).space||''}/>;}
