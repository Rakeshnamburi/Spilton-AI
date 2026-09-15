import { NextRequest, NextResponse } from 'next/server';
import { cookies } from 'next/headers';
import { AUTH_COOKIE, backendRequest, isAllowedOrigin } from '@/lib/backend';
export const dynamic='force-dynamic';
export async function POST(request:NextRequest){
  if(!isAllowedOrigin(request.headers.get('origin')))return NextResponse.json({title:'Request origin is not allowed.'},{status:403});
  const token=(await cookies()).get(AUTH_COOKIE)?.value;if(!token)return NextResponse.json({title:'Please sign in again.'},{status:401});
  const body=await request.text();return backendRequest('/agents/run',{method:'POST',headers:{Authorization:`Bearer ${token}`,'Content-Type':'application/json'},body});
}
