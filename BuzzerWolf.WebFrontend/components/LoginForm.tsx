'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';

export default function LoginForm({
  onLogin,
  onSuccess,
}: {
  onLogin: (username: string, accessKey: string) => Promise<void>;
  onSuccess?: () => void; // Optional, with default behavior
}) {
  const router = useRouter();
  const [username, setUsername] = useState('');
  const [accessKey, setAccessKey] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      await onLogin(username, accessKey); // Authenticate the user
      if (onSuccess) {
        onSuccess(); // Call the custom success handler if provided
      } else {
        router.push('/home'); // Default success behavior
      }
    } catch (err) {
      setError((err as Error)?.message || 'Login failed. Please try again.');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="p-4 shadow-md rounded">
      <h2 className="text-2xl text-secondary font-bold mb-4">Log In</h2>
      {error && <p className="text-red-500 mb-4">{error}</p>}
      <div className="mb-4">
        <label className="block text-sm text-neutral-dark font-bold mb-2">Username</label>
        <input
          type="text"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className="w-full px-3 py-2 border rounded focus:outline-none"
          required
        />
      </div>
      <div className="mb-4">
        <label className="block text-sm text-foreground font-bold mb-2">Access Key</label>
        <input
          type="password"
          value={accessKey}
          onChange={(e) => setAccessKey(e.target.value)}
          className="w-full px-3 py-2 border rounded focus:outline-none"
          required
        />
      </div>
      <button
        type="submit"
        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        Log In
      </button>
    </form>
  );
}
