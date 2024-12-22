'use client';

import LoginForm from '@/components/LoginForm';
import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';

export default function LoginPage() {
  const { getCredentials, login } = useAuth();
  const router = useRouter();

  const credentials = getCredentials();
  if (credentials) {
    console.log("already logged in, redirecting to /home");
    router.push('/home');
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
