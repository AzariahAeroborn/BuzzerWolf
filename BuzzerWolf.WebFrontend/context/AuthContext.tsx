'use client';

import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { usePublicApi } from '@/hooks/BuzzerWolfPublicApi';

const AUTH_STORAGE_KEY = 'buzzerwolf_auth';

export interface Credentials {
  username: string;
  accessKey: string;
  secondTeam: boolean;
}

interface AuthContextType {
  auth: Credentials | null;
  isAuthLoading: boolean;
  login: (credentials: Credentials) => Promise<void>;
  logout: () => void;
  getCredentials: () => Credentials | null; // just an abstraction/alias for accessing credentials, maybe not a good idea
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [auth, setAuth] = useState<Credentials | null>(null);
  const [isAuthLoading, setAuthLoading] = useState(true);
  const { login: apiLogin } = usePublicApi();

  useEffect(() => {
    const storedAuth = localStorage.getItem(AUTH_STORAGE_KEY);
    if (storedAuth) {
      setAuth(JSON.parse(storedAuth));
    }
    setAuthLoading(false);
  }, []);

  const login = async (credentials: Credentials) => {
    await apiLogin(credentials.username, credentials.accessKey, credentials.secondTeam); // Call API to verify credentials as our idea of 'login'
    setAuth(credentials); // Update auth state
    localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(credentials)); // Persist auth state in localStorage
  };

  const logout = () => {
    setAuth(null);
    localStorage.removeItem(AUTH_STORAGE_KEY); // Clear localStorage
  };

  const getCredentials = () => auth; // Simple accessor for auth state

  // Sync auth state across tabs
  useEffect(() => {
    const handleStorage = (event: StorageEvent) => {
      if (event.key === AUTH_STORAGE_KEY) {
        setAuth(event.newValue ? JSON.parse(event.newValue) : null);
      }
    };

    window.addEventListener('storage', handleStorage);
    return () => window.removeEventListener('storage', handleStorage);
  }, []);

  return (
    <AuthContext.Provider value={{ auth, isAuthLoading, login, logout, getCredentials }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextType {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
