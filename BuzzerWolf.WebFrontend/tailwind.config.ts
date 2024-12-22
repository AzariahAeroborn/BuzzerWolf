import type { Config } from "tailwindcss";

export default {
  content: [
    "./pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./components/**/*.{js,ts,jsx,tsx,mdx}",
    "./app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        background: "var(--background)",
        foreground: "var(--foreground)",
        primary: "var(--primary)",
        "primary-light": "var(--primary-light)",
        "primary-dark": "var(--primary-dark)",
        secondary: "var(--secondary)",
        "secondary-light": "var(--secondary-light)",
        "secondary-dark": "var(--secondary-dark)",
        neutral: "var(--neutral)",
        "neutral-light": "var(--neutral-light)",
        "neutral-dark": "var(--neutral-dark)",
        sidebarBg: 'var(--color-sidebar-bg)',
        textPrimary: 'var(--color-text-primary)',
        linkHover: 'var(--color-link-hover)',
        'sidebar-bg': 'var(--color-sidebar-bg)',
        'sidebar-text': 'var(--color-sidebar-text)',
        'sidebar-hover': 'var(--color-sidebar-hover)',
      },
    },
  },
  plugins: [],
} satisfies Config;
