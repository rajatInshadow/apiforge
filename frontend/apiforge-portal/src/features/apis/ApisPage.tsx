import { FormEvent, useEffect, useState } from 'react';
import { useAuth } from '../auth/AuthContext';
import { http } from '../../services/http';
import { RegisteredApi } from '../../types/api';

export function ApisPage() {
  const { user } = useAuth();
  const [apis, setApis] = useState<RegisteredApi[]>([]);
  const [name, setName] = useState('Inventory API');
  const [routePrefix, setRoutePrefix] = useState('inventory');
  const [downstreamBaseUrl, setDownstreamBaseUrl] = useState('https://localhost:7501');
  const [description, setDescription] = useState('New downstream API');

  async function load() {
    const response = await http.get<RegisteredApi[]>('/api/apis');
    setApis(response.data);
  }

  useEffect(() => { load(); }, []);

  async function createApi(e: FormEvent) {
    e.preventDefault();
    await http.post('/api/apis', { name, routePrefix, downstreamBaseUrl, description, status: 'Active' });
    await load();
  }

  return (
    <div>
      <h1>API Catalog</h1>
      {user?.role === 'Admin' && (
        <form className="panel form-grid" onSubmit={createApi}>
          <h2>Register New API</h2>
          <input value={name} onChange={(e) => setName(e.target.value)} placeholder="Name" />
          <input value={routePrefix} onChange={(e) => setRoutePrefix(e.target.value)} placeholder="Route prefix" />
          <input value={downstreamBaseUrl} onChange={(e) => setDownstreamBaseUrl(e.target.value)} placeholder="Downstream URL" />
          <textarea value={description} onChange={(e) => setDescription(e.target.value)} placeholder="Description" />
          <button type="submit">Create API</button>
        </form>
      )}
      <div className="panel">
        <table>
          <thead><tr><th>Name</th><th>Prefix</th><th>Downstream</th><th>Status</th></tr></thead>
          <tbody>
            {apis.map((api) => <tr key={api.id}><td>{api.name}</td><td>/gateway/{api.routePrefix}</td><td>{api.downstreamBaseUrl}</td><td>{api.status}</td></tr>)}
          </tbody>
        </table>
      </div>
    </div>
  );
}
