import { FormEvent, useEffect, useState } from 'react';
import { http } from '../../services/http';
import { ApiKey, ApiKeyCreateResponse, RegisteredApi } from '../../types/api';

export function ApiKeysPage() {
  const [keys, setKeys] = useState<ApiKey[]>([]);
  const [apis, setApis] = useState<RegisteredApi[]>([]);
  const [name, setName] = useState('Local Test Key');
  const [registeredApiId, setRegisteredApiId] = useState('');
  const [newKey, setNewKey] = useState('');

  async function load() {
    const [keysResponse, apisResponse] = await Promise.all([
      http.get<ApiKey[]>('/api/api-keys'),
      http.get<RegisteredApi[]>('/api/apis')
    ]);
    setKeys(keysResponse.data);
    setApis(apisResponse.data);
  }

  useEffect(() => { load(); }, []);

  async function createKey(e: FormEvent) {
    e.preventDefault();
    const response = await http.post<ApiKeyCreateResponse>('/api/api-keys', {
      name,
      registeredApiId: registeredApiId ? Number(registeredApiId) : null,
      expiresAtUtc: null
    });
    setNewKey(response.data.plainTextKey);
    await load();
  }

  return (
    <div>
      <h1>API Keys</h1>
      <form className="panel form-grid" onSubmit={createKey}>
        <h2>Generate Key</h2>
        <input value={name} onChange={(e) => setName(e.target.value)} />
        <select value={registeredApiId} onChange={(e) => setRegisteredApiId(e.target.value)}>
          <option value="">All APIs</option>
          {apis.map((api) => <option key={api.id} value={api.id}>{api.name}</option>)}
        </select>
        <button type="submit">Generate</button>
        {newKey && <div className="alert success"><strong>Copy this now:</strong><br />{newKey}</div>}
      </form>
      <div className="panel">
        <table>
          <thead><tr><th>Name</th><th>Prefix</th><th>API</th><th>Status</th></tr></thead>
          <tbody>
            {keys.map((key) => <tr key={key.id}><td>{key.name}</td><td>{key.keyPrefix}</td><td>{key.registeredApiId ?? 'All'}</td><td>{key.isActive ? 'Active' : 'Inactive'}</td></tr>)}
          </tbody>
        </table>
      </div>
    </div>
  );
}
