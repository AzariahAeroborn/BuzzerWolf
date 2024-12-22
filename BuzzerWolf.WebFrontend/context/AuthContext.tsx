'use client';

import { createContext, useContext, useState, ReactNode } from 'react';
import { usePublicApi } from '@/hooks/BuzzerWolfPublicApi';

export interface Credentials {
  username: string;
  accessKey: string;
}

interface AuthContextType {
  auth: Credentials | null;
  login: (credentials: Credentials) => Promise<void>;
  logout: () => void;
  getCredentials: () => Credentials | null; // Abstraction for accessing credentials
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [auth, setAuth] = useState<Credentials | null>(null);
  const { login: apiLogin } = usePublicApi();

  const login = async (credentials: Credentials) => {
    await apiLogin(credentials.username, credentials.accessKey); // Call API to verify credentials as our idea of 'login'
    setAuth(credentials); // Update auth state
  };

  const logout = () => setAuth(null);

  const getCredentials = () => auth; // Simple accessor for auth state

  return (
    <AuthContext.Provider value={{ auth, login, logout, getCredentials }}>
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
