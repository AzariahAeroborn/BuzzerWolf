# BuzzerWolf WebFrontend

A NextJS project at least somewhat-integrated with VS development and made easy to deploy to a Windows .NET server for hosting.

Uses NextjsStaticHosting.AspNetCore:
- [Repo](https://github.com/davidnx/NextjsStaticHosting-AspNetCore)
- [Blog Post](https://medium.com/@david.nissimoff/next-js-meets-asp-net-core-a-story-of-performance-and-love-at-long-tail-41cf9231b2de)

Take note of the limitations, mainly that everything has to be static content (no server components).

## First time setup

I'm pretty sure you do still need Node.js for development purposes: after all `npm` commands have to come from somewhere. I don't know the best way to do that on Windows, hopefully the next person to do it can take notes.

I expect you also have to do `npm i` for package installations, once on initial setup and then any time as usual when changing package.json dependencies:
```
$(BuzzerWolf/BuzzerWolf.WebFrontend)> npm i
```

## General Usage

For development it's probably more convenient to do normal NextJS development per the default readme below; if you're using the BuzzerWolf.Server I don't know how to get it to do hot reload, you have to manually rerun the NextJS build every time.

For using with BuzzerWolf.Server, do
```
$(BuzzerWolf/BuzzerWolf.WebFrontend)> npx next build
```

then run the Buzzerwolf Server and hopefully it will do both "frontend" and "backend" type things. for example try the localhost:8080 (Home Page)[http://localhost:8080]

note that `npm run dev` by default uses port 3000 and at time of writing BuzzerWolf.Server is configured for port 8080

# Below follows the standard default NextJS Readme

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
