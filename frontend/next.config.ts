import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  async redirects(){
    const canonical=(process.env.CANONICAL_APP_ORIGIN||"https://spiltonai.vercel.app").replace(/\/$/,"");
    return [{source:"/:path*",has:[{type:"host",value:"spilton-ai.vercel.app"}],destination:`${canonical}/:path*`,permanent:true}];
  }
};

export default nextConfig;
