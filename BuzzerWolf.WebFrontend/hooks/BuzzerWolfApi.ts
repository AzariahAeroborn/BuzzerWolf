'use client';

import { useCallback, useMemo } from 'react';
import { useAuth, Credentials } from '@/context/AuthContext';

const API_URL = 'api';

export function useApi() {
  const { getCredentials, logout } = useAuth();

  const makeApiCall = useCallback(
    async (
      endpoint: string,
      method: 'GET' | 'POST',
      options?: {
        query?: Record<string, string | number | boolean>;
        body?: Record<string, any>;
      }
    ) => {
      const credentials = getCredentials();
      if (!credentials) {
        throw new Error('User is not logged in.');
      }

      // creds are via cookie not headers. so... magic?
      //const { username, accessKey } = credentials;

      // Construct query parameters
      let url = `${API_URL}${endpoint}`;
      if (options?.query) {
        const params = new URLSearchParams(
          Object.entries(options.query).map(([key, value]) => [key, String(value)])
        );
        url += `?${params.toString()}`;
      }

      // Prepare fetch options
      const fetchOptions: RequestInit = {
        method,
        credentials: 'include', // needed for cookie-style auth
        headers: {
          'Content-Type': 'application/json',
          // creds are via cookie not headers
          //'X-Username': username,
          //Authorization: `Bearer ${accessKey}`,
        },
      };

      if (method === 'POST' && options?.body) {
        fetchOptions.body = JSON.stringify(options.body);
      }

      const response = await fetch(url, fetchOptions);

      if (response.status === 401 || response.status === 403) {
        // Unauthorized or forbidden: likely due to expired or invalid session
        console.log(`response status ${response.status}, logging out`);
        logout(); // Log the user out
        throw new Error('Session expired or unauthorized.');
      }

      if (!response.ok) {
        throw new Error(`API call failed: ${response.statusText}`);
      }

      // Check if the response body is empty before parsing as JSON
      const contentLength = response.headers.get('Content-Length');
      if (contentLength === '0' || response.status === 204) {
        // Empty success response (e.g., HTTP 204 No Content)
        return null;
      }

      return response.json();
    },
    [getCredentials] // Dependencies
  );

  const get = useCallback(
    (endpoint: string, query?: Record<string, string | number | boolean>) =>
      makeApiCall(endpoint, 'GET', { query }),
    [makeApiCall]
  );

  const post = useCallback(
    (
      endpoint: string,
      options?: {
        query?: Record<string, string | number | boolean>;
        body?: Record<string, any>;
      }
    ) => makeApiCall(endpoint, 'POST', options),
    [makeApiCall]
  );

  const country = useCallback(() => get('/country'), [get]);

  const currentSeason = useCallback(() => get('/season/current'), [get]);


  // Memoize the returned API methods to ensure stability
  return useMemo(() =>
    ({ makeApiCall, country, currentSeason }),
      [makeApiCall, country, currentSeason]
  );
}
