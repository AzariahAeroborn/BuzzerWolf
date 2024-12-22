'use client';

import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';
import { ReactNode, useEffect } from 'react';

export default function AuthenticatedLayout({ children }: { children: ReactNode }) {
  const { auth } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (!auth) {
      router.push('/'); // Redirect to home (login) if not authenticated
    }
  }, [auth, router]);

  // AP: pretty sure this isn't needed though chatgpt made an overcomplicated explanation
  //  we should keep an eye out for flickers of content beating the redirect
//   if (!auth) {
//     return null; // Optionally render a loading spinner here
//   }

  return <>{children}</>;
}
