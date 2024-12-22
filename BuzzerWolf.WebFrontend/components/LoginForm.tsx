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
    <form
      onSubmit={handleSubmit}
      className="p-6 bg-background shadow-md rounded"
    >
      <h2 className="text-2xl text-foreground font-bold mb-4">Log In</h2>
      {error && <p className="text-red-500 mb-4">{error}</p>}
      <div className="mb-4">
        <label
          htmlFor="username"
          className="block text-foreground text-sm font-bold mb-2"
        >
          Username
        </label>
        <input
          id="username"
          type="text"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className="w-full px-3 py-2 border border-neutral focus:border-primary focus:ring focus:ring-primary-light bg-background text-foreground rounded"
          required
        />
      </div>
      <div className="mb-4">
        <label
          htmlFor="accessKey"
          className="block text-foreground text-sm font-bold mb-2"
        >
          Access Key
        </label>
        <input
          id="accessKey"
          type="password"
          value={accessKey}
          onChange={(e) => setAccessKey(e.target.value)}
          className="w-full px-3 py-2 border border-neutral focus:border-primary focus:ring focus:ring-primary-light bg-background text-foreground rounded"
          required
        />
      </div>
      <button
        type="submit"
        className="bg-primary text-white px-4 py-2 rounded hover:bg-primary-dark focus:outline-none focus:ring focus:ring-primary-light"
      >
        Log In
      </button>
    </form>
  );
}
