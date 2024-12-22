'use client';

import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';
import { ReactNode, useEffect } from 'react';
import Sidebar from '@/components/Sidebar';

export default function AuthenticatedLayout({ children }: { children: ReactNode }) {
  const { auth, isAuthLoading } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (!isAuthLoading && !auth) {
      router.push('/'); // Redirect to login if unauthenticated
    }
  }, [isAuthLoading, auth, router]);

  // apparently since auth loads asynchronously it's possible to
  // try to render the layout before auth is loaded, so we need to
  // be careful not to show any sensitive data or cause screen flickers
  // while things initialize
  if (!auth) {
    return null; // Optionally render a loading spinner here
  }

  if (isAuthLoading) {
    return <div>Loading...</div>; // Render a loading indicator while auth is being initialized
  }

  return (
    <div className="flex h-screen">
      <Sidebar />
      <main className="flex-grow p-4">{children}</main>
    </div>
  );
}
