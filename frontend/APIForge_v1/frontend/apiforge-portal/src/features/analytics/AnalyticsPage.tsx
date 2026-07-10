import { useEffect, useState } from 'react';
import { http } from '../../services/http';
import { DashboardSummary } from '../../types/api';

export function AnalyticsPage() {
  const [summary, setSummary] = useState<DashboardSummary | null>(null);

  useEffect(() => {
    http.get<DashboardSummary>('/api/analytics/summary').then((r) => setSummary(r.data));
  }, []);

  return (
    <div>
      <h1>Analytics</h1>
      <div className="panel">
        <h2>Baseline Analytics</h2>
        <p>This v1 screen intentionally uses simple tables. Your assigned frontend ticket will add charts and filters.</p>
        {summary && (
          <table>
            <tbody>
              <tr><td>Total requests</td><td>{summary.totalRequests}</td></tr>
              <tr><td>Failed requests</td><td>{summary.failedRequests}</td></tr>
              <tr><td>Average response time</td><td>{summary.averageResponseTimeMs} ms</td></tr>
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
