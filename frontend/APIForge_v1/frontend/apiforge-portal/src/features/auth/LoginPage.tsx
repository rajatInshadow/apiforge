import { FormEvent, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { http } from '../../services/http';
import { AuthResponse } from '../../types/api';
import { useAuth } from './AuthContext';

export function LoginPage() {
  const [email, setEmail] = useState('admin@apiforge.local');
  const [password, setPassword] = useState('Admin@123');
  const [error, setError] = useState('');
  const { login } = useAuth();
  const navigate = useNavigate();

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError('');
    try {
      const response = await http.post<AuthResponse>('/api/auth/login', { email, password });
      login(response.data);
      navigate('/dashboard');
    } catch {
      setError('Invalid credentials or backend is not running.');
    }
  }

  return (
    <main className="auth-page">
      <form className="auth-card" onSubmit={handleSubmit}>
        <h1>APIForge</h1>
        <p>Sign in to the developer portal.</p>
        {error && <div className="alert error">{error}</div>}
        <label>Email</label>
        <input value={email} onChange={(e) => setEmail(e.target.value)} />
        <label>Password</label>
        <input value={password} onChange={(e) => setPassword(e.target.value)} type="password" />
        <button type="submit">Login</button>
        <Link to="/register">Create developer account</Link>
      </form>
    </main>
  );
}
