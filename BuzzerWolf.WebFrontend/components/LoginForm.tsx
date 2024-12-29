'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';

export default function LoginForm({
  onLogin,
  onSuccess,
}: {
  onLogin: (username: string, accessKey: string, secondTeam: boolean) => Promise<void>;
  onSuccess?: () => void; // Optional, with default behavior
}) {
  const router = useRouter();
  const [username, setUsername] = useState('');
  const [accessKey, setAccessKey] = useState('');
  const [secondTeam, setSecondTeam] = useState(false);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await onLogin(username, accessKey, secondTeam); // Authenticate the user
      if (onSuccess) {
        onSuccess(); // Call the custom success handler if provided
      } else {
        router.push('/home'); // Default success behavior
      }
    } catch (err) {
      setError((err as Error)?.message || 'Login failed. Please try again.');
    } finally {
      setLoading(false); // Hide spinner
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
      <div className="mb-4">
        <label className="flex items-center text-foreground text-sm">
          <input
            type="checkbox"
            checked={secondTeam}
            onChange={(e) => setSecondTeam(e.target.checked)}
            className="mr-2"
          />
          Log in as Second Team
        </label>
      </div>
      <button
        type="submit"
        disabled={loading} // Disable button while loading
        className={`bg-primary text-white px-4 py-2 rounded hover:bg-primary-dark focus:outline-none focus:ring focus:ring-primary-light ${
          loading ? 'opacity-50 cursor-not-allowed' : ''
        }`}
      >
        {loading ? (
          <svg
            className="animate-spin h-5 w-5 mx-auto text-white"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
          >
            <circle
              className="opacity-25"
              cx="12"
              cy="12"
              r="10"
              stroke="currentColor"
              strokeWidth="4"
            ></circle>
            <path
              className="opacity-75"
              fill="currentColor"
              d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
            ></path>
          </svg>
        ) : (
          'Log In'
        )}
      </button>
    </form>
  );
}
