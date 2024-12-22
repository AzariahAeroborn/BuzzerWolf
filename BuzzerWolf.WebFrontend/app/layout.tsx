'use client';

import "./globals.css";
import { AuthProvider } from '@/context/AuthContext';

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <AuthProvider>
      <html lang="en">
        <body>
          {children}
        </body>
      </html>
    </AuthProvider>
  );
}
