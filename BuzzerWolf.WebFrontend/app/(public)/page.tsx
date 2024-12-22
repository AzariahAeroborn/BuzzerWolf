'use client';

import LoginForm from '@/components/LoginForm';
import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';

export default function LoginPage() {
  const { login } = useAuth(); // Use the `login` function from AuthContext
  const router = useRouter();

  const handleLogin = async (username: string, accessKey: string) => {
    try {
      // Call the `login` function from AuthContext
      await login({ username, accessKey });
      console.log('Login successful');
      router.push('/home'); // Redirect to the homepage on success
    } catch (err) {
      console.error('Login failed:', err);
      // Optionally handle the error (e.g., show a toast or error message)
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100">
      <div className="text-center">
        <h1 className="text-4xl font-bold mb-8">BuzzerWolf</h1>
        <LoginForm onLogin={handleLogin} />
      </div>
    </div>
  );
}
