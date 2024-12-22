'use client';

const API_URL = 'http://localhost:8080';

export function usePublicApi() {

  const makePublicApiCall = async (
    endpoint: string,
    method: 'GET' | 'POST',
    options?: {
      query?: Record<string, string | number | boolean>;
      body?: Record<string, any>;
    }
  ) => {
    // Construct query parameters
    let url = `${API_URL}${endpoint}`;
    console.log("url:", url);
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
      },
    };

    if (method === 'POST' && options?.body) {
      fetchOptions.body = JSON.stringify(options.body);
    }

    const response = await fetch(url, fetchOptions);
    console.log("response:", response);

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
  };

  const get = (endpoint: string, query?: Record<string, string | number | boolean>) =>
    makePublicApiCall(endpoint, 'GET', { query });

  const post = (
    endpoint: string,
    options?: {
      query?: Record<string, string | number | boolean>;
      body?: Record<string, any>;
    }
  ) => makePublicApiCall(endpoint, 'POST', options);

  const login = (username: string, accessKey: string, secondTeam: boolean) =>
    post('/login', {
      query: {
        username: username,
        accessKey: accessKey,
        secondTeam: secondTeam,
      },
    });

  return { makePublicApiCall, login };
}
