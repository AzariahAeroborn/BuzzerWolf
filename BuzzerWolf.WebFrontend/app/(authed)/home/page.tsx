'use client';

import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';

export default function Dashboard() {
  const { auth, logout } = useAuth();
  const router = useRouter();

  if (!auth) {
    router.push('/login'); // Redirect to login if not authenticated
    return null;
  }

  return (
    <div>
      <h1>Welcome, {auth.username}!</h1>
      <button
        onClick={logout}
        className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-700"
      >
        Log Out
      </button>
    </div>
  );
}
