import { useEffect, useState } from "react";
import { http } from "../../services/http";
import { PagedResponse, RequestLog } from "../../types/api";
import Filter from "../../components/filter/filter";

export function LogsPage() {
  const [data, setData] = useState<PagedResponse<RequestLog> | null>(null);
  const [page, setPage] = useState(1);

  useEffect(() => {
    http
      .get<
        PagedResponse<RequestLog>
      >(`/api/request-logs?page=${page}&pageSize=25`)
      .then((res) =>{

        setData(res.data);
        console.log(res.data)
      }
    );
  }, [page]);

  return (
    <div>
      <h1>Request Logs</h1>
      <Filter/>
      <div className="panel">
        <table>
          <thead>
            <tr>
              <th>Time</th>
              <th>Method</th>
              <th>Path</th>
              <th>Status</th>
              <th>Time ms</th>
            </tr>
          </thead>
          <tbody>
            {data?.items.map((log) => (
              <tr key={log.id}>
                <td>{new Date(log.createdAtUtc).toLocaleString()}</td>
                <td>{log.httpMethod}</td>
                <td>{log.requestPath}</td>
                <td>{log.statusCode}</td>
                <td>{log.responseTimeMs}</td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="pager">
          <button
            className="secondary"
            onClick={() => setPage((p) => Math.max(1, p - 1))}
          >
            Previous
          </button>
          <span>Page {page}</span>
          <button className="secondary" onClick={() => setPage((p) => p + 1)}>
            Next
          </button>
        </div>
      </div>
    </div>
  );
}
