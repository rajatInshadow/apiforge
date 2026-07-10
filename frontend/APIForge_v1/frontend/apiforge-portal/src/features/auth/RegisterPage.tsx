import { FormEvent, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { http } from '../../services/http';
import { AuthResponse } from '../../types/api';
import { useAuth } from './AuthContext';

export function RegisterPage() {
  const [fullName, setFullName] = useState('New Developer');
  const [email, setEmail] = useState('new.dev@apiforge.local');
  const [password, setPassword] = useState('Dev@123');
  const [error, setError] = useState('');
  const { login } = useAuth();
  const navigate = useNavigate();

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError('');
    try {
      const response = await http.post<AuthResponse>('/api/auth/register', { fullName, email, password, role: 'Developer' });
      login(response.data);
      navigate('/dashboard');
    } catch {
      setError('Registration failed. Email may already exist.');
    }
  }

  return (
    <main className="auth-page">
      <form className="auth-card" onSubmit={handleSubmit}>
        <h1>Create Account</h1>
        {error && <div className="alert error">{error}</div>}
        <label>Full name</label>
        <input value={fullName} onChange={(e) => setFullName(e.target.value)} />
        <label>Email</label>
        <input value={email} onChange={(e) => setEmail(e.target.value)} />
        <label>Password</label>
        <input value={password} onChange={(e) => setPassword(e.target.value)} type="password" />
        <button type="submit">Register</button>
        <Link to="/login">Back to login</Link>
      </form>
    </main>
  );
}
