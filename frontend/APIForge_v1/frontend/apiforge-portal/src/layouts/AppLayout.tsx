import { NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../features/auth/AuthContext';

export function AppLayout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate('/login');
  }

  return (
    <div className="shell">
      <aside className="sidebar">
        <h2>APIForge</h2>
        <NavLink to="/dashboard">Dashboard</NavLink>
        <NavLink to="/apis">API Catalog</NavLink>
        <NavLink to="/api-keys">API Keys</NavLink>
        <NavLink to="/logs">Request Logs</NavLink>
        <NavLink to="/analytics">Analytics</NavLink>
      </aside>
      <section className="main">
        <header className="topbar">
          <div>
            <strong>{user?.fullName}</strong>
            <span>{user?.role}</span>
          </div>
          <button className="secondary" onClick={handleLogout}>Logout</button>
        </header>
        <div className="content">
          <Outlet />
        </div>
      </section>
    </div>
  );
}
