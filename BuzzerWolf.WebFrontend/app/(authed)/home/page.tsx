'use client';

import { useAuth } from '@/context/AuthContext';

export default function TeamHome() {
  const { auth, logout } = useAuth();

  return (
    <div>
      {auth && <h1>Welcome, {auth.username}!</h1>}
      <button
        onClick={logout}
        className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-700"
      >
        Log Out
      </button>
    </div>
  );
}
