'use client';

import LoginForm from '@/components/LoginForm';
import { useRouter } from 'next/navigation';

export default function LoginPage() {
  const router = useRouter();

  const handleLogin = (authData: any) => {
    // Save auth data to context or local storage
    console.log('Logged in:', authData);
    router.push('/home'); // Redirect to the homepage
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100">
      <h1 className="text-4xl font-bold mb-8">BuzzerWolf</h1>
      <LoginForm onLogin={handleLogin} />
    </div>
  );
}
