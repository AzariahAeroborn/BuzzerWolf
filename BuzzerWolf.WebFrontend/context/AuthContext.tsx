import { createContext, useContext, useState, ReactNode } from 'react';

interface Credentials {
  username: string;
  accessKey: string;
}

interface AuthContextType {
  auth: Credentials | null;
  login: (credentials: Credentials) => void;
  getCredentials: () => Credentials | null;
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [auth, setAuth] = useState<Credentials | null>(null);

  const login = (credentials: Credentials) => setAuth(credentials);

  const getCredentials = () => auth;

  return (
    <AuthContext.Provider value={{ auth, login, getCredentials }}>
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
