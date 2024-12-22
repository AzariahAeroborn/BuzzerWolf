'use client';

import { useAuth, Credentials } from '@/context/AuthContext';

const API_URL = 'http://localhost:8080';

export function useApi() {
  const { getCredentials } = useAuth();

  const makeApiCall = async (
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

    const { username, accessKey } = credentials;

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
      headers: {
        'Content-Type': 'application/json',
        'X-Username': username,
        Authorization: `Bearer ${accessKey}`,
      },
    };

    if (method === 'POST' && options?.body) {
      fetchOptions.body = JSON.stringify(options.body);
    }

    const response = await fetch(url, fetchOptions);

    if (!response.ok) {
      throw new Error(`API call failed: ${response.statusText}`);
    }

    return response.json();
  };

  const get = (endpoint: string, query?: Record<string, string | number | boolean>) =>
    makeApiCall(endpoint, 'GET', { query });

  const post = (
    endpoint: string,
    options?: {
      query?: Record<string, string | number | boolean>;
      body?: Record<string, any>;
    }
  ) => makeApiCall(endpoint, 'POST', options);

  const login = (credentials: Credentials) =>
    post('/login', {
      query: {
        username: credentials.username,
        accessKey: credentials.accessKey,
      },
    });

  const country = () => get('/country');

  return { makeApiCall, login, country };
}
