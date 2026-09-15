import {redirect} from 'next/navigation';
import {getSession} from '@/lib/backend';
import {MockWorkspace} from '@/components/mock-workspace';
export const dynamic='force-dynamic';
export default async function Page({searchParams}:{searchParams:Promise<{space?:string;attempt?:string;paper?:string}>}){const {user,unavailable}=await getSession();const params=await searchParams;if(unavailable)return <main>Spilton is temporarily unavailable. Your saved attempt remains in the database. <a href="/mocks">Retry</a></main>;if(!user)redirect('/login?returnTo='+encodeURIComponent('/mocks?'+new URLSearchParams(params as Record<string,string>)));return <MockWorkspace key={(params.space||'general')+(params.attempt||params.paper||'')} space={params.space||''} attemptId={params.attempt} paperId={params.paper}/>;}
