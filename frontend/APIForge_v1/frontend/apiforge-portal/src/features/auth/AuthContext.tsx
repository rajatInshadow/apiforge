import { createContext, ReactNode, useContext, useMemo, useState } from 'react';
import { AuthResponse } from '../../types/api';

type AuthContextValue = {
  user: AuthResponse | null;
  token: string | null;
  login: (auth: AuthResponse) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthResponse | null>(() => {
    const raw = localStorage.getItem('apiforge_user');
    return raw ? JSON.parse(raw) : null;
  });

  const token = localStorage.getItem('apiforge_token');

  const value = useMemo<AuthContextValue>(() => ({
    user,
    token,
    login: (auth) => {
      localStorage.setItem('apiforge_token', auth.token);
      localStorage.setItem('apiforge_user', JSON.stringify(auth));
      setUser(auth);
    },
    logout: () => {
      localStorage.removeItem('apiforge_token');
      localStorage.removeItem('apiforge_user');
      setUser(null);
    }
  }), [user, token]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth must be used inside AuthProvider');
  return context;
}
