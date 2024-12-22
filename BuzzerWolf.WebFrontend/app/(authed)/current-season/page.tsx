'use client';

import { useState, useEffect } from 'react';
import { useApi } from '@/hooks/BuzzerWolfApi';

// currently doing a silly example to show bb's current season
type CurrentSeason = {
  id: number;
  start: string;
  finish: string;
};

export default function TeamHome() {
  const { currentSeason } = useApi();
  const [currentSeasonInfo, setCurrentSeasonInfo] = useState<CurrentSeason|null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Fetch current season info
  useEffect(() => {
    const fetchCurrentSeasonInfo = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await currentSeason();
        setCurrentSeasonInfo(data);
      } catch (err) {
        setError((err as Error).message);
      } finally {
        setLoading(false);
      }
    };

    fetchCurrentSeasonInfo();
  }, [currentSeason]);

  return (
    <div>
      <div className="mt-4">
        <h2 className="text-lg font-bold">Current Season</h2>
        {loading && <p>Loading...</p>}
        {error && <p className="text-red-500">{error}</p>}
        {!loading && !error && (
          <div>
            <strong>Current Season:</strong> {currentSeasonInfo?.id}
            <strong>Start:</strong> {currentSeasonInfo?.start}
            <strong>Finish:</strong> {currentSeasonInfo?.finish || 'N/A'}
          </div>
        )}
      </div>
    </div>
  );
}
