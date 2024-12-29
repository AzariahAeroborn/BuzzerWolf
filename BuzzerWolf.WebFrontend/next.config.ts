import type { NextConfig } from "next";

const nextConfig: NextConfig = {
    /* config options here */
    output: 'export', // required for NextJS static .net hosting
    distDir: '../BuzzerWolf.Server/nodeapp'
};

export default nextConfig;
