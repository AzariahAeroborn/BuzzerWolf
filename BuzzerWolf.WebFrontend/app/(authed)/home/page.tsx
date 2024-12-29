'use client';

import { useState, useEffect } from 'react';
import { useAuth } from '@/context/AuthContext';
import { useApi } from '@/hooks/BuzzerWolfApi';

// currently doing a silly example to show bb's countries
type Country = {
    id: number;
    name: string;
    divisions: number;
    firstSeason: number;
  };

export default function TeamHome() {
  const { auth } = useAuth();
  const { country } = useApi();
  const [countries, setCountries] = useState<Country[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Fetch countries
  useEffect(() => {
    const fetchCountries = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await country();
        setCountries(data);
      } catch (err) {
        setError((err as Error).message);
      } finally {
        setLoading(false);
      }
    };

    fetchCountries();
  }, [country]);

  return (
    <div>
      {auth && <h1>Welcome, {auth.username}!</h1>}
      {/* <button
        onClick={logout}
        className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-700"
      >
        Log Out
      </button> */}

      <div className="mt-4">
        <h2 className="text-lg font-bold">Countries</h2>
        {loading && <p>Loading countries...</p>}
        {error && <p className="text-red-500">{error}</p>}
        {!loading && !error && (
          <ul className="list-disc pl-6">
            {countries.map((country) => (
              <li key={country.id}>
                <strong>{country.name}</strong> - {country.divisions} divisions, first season: {country.firstSeason}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}
