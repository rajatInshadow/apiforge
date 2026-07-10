import { useEffect, useState } from 'react';
import { http } from '../../services/http';
import { DashboardSummary } from '../../types/api';

export function DashboardPage() {
  const [summary, setSummary] = useState<DashboardSummary | null>(null);

  useEffect(() => {
    http.get<DashboardSummary>('/api/analytics/summary').then((r) => setSummary(r.data));
  }, []);

  if (!summary) return <p>Loading dashboard...</p>;

  return (
    <div>
      <h1>Dashboard</h1>
      <div className="cards">
        <div className="card"><span>Total Requests</span><strong>{summary.totalRequests}</strong></div>
        <div className="card"><span>Successful</span><strong>{summary.successfulRequests}</strong></div>
        <div className="card"><span>Failed</span><strong>{summary.failedRequests}</strong></div>
        <div className="card"><span>Avg Response</span><strong>{summary.averageResponseTimeMs} ms</strong></div>
      </div>
      <div className="panel">
        <h2>Top APIs</h2>
        <table>
          <thead><tr><th>API</th><th>Requests</th><th>Avg ms</th></tr></thead>
          <tbody>
            {summary.topApis.map((x) => <tr key={x.registeredApiId}><td>{x.apiName}</td><td>{x.requestCount}</td><td>{Math.round(x.averageResponseTimeMs)}</td></tr>)}
          </tbody>
        </table>
      </div>
    </div>
  );
}
