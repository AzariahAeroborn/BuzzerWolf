# BuzzerWolf WebFrontend

A NextJS project at least somewhat-integrated with VS development.

Uses NextjsStaticHosting.AspNetCore:
- [Repo](https://github.com/davidnx/NextjsStaticHosting-AspNetCore)
- [Blog Post](https://medium.com/@david.nissimoff/next-js-meets-asp-net-core-a-story-of-performance-and-love-at-long-tail-41cf9231b2de)

Take note of the limitations, mainly that everything has to be static content (no server components)
 
## General Usage

For development it's probably more convenient to do normal NextJS development per the default readme below

For running with BuzzerWolf, do
```
npx next build
```

then copy the generated files from /out to.... somewhere that I haven't figured out yet. Could be nice to automate that someday, maybe integrate it with VS.

run the Buzzerwolf Server and hopefully it will do both "frontend" and "backend" type things. I guess we'll find out!


# Below is the standard default NextJS Readme

This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

## Getting Started

First, run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

You can start editing the page by modifying `app/page.tsx`. The page auto-updates as you edit the file.

This project uses [`next/font`](https://nextjs.org/docs/app/building-your-application/optimizing/fonts) to automatically optimize and load [Geist](https://vercel.com/font), a new font family for Vercel.

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js) - your feedback and contributions are welcome!

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/app/building-your-application/deploying) for more details.
