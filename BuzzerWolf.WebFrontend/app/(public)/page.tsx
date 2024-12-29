'use client';

import { useEffect } from 'react';
import LoginForm from '@/components/LoginForm';
import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';

export default function LoginPage() {
  const { auth, login } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (auth) {
      console.log('already logged in, redirecting to /home');
      router.push('/home'); // Redirect if already logged in
    }
  }, [auth, router]);

  if (auth) {
    return null; // Prevent rendering if redirecting
  }


  const handleLogin = async (username: string, accessKey: string, secondTeam: boolean) => {
    await login({ username, accessKey, secondTeam }); // default behavior on success is to redirect to /home
  };

  return (
    <div className="flex min-h-screen items-center justify-center">
      <div className="text-center">
        <h1 className="text-4xl text-secondary font-bold mb-8">BuzzerWolf</h1>
        <LoginForm onLogin={handleLogin} />
      </div>
    </div>
  );
}
