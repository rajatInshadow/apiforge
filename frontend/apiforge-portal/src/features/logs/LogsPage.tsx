import { useEffect, useState } from "react";
import { http } from "../../services/http";
import { PagedResponse, RequestLog } from "../../types/api";
import Filter from "../../components/filter/filter";
import { RequestLogType } from "../../types/filter";

export function LogsPage() {
  const [data, setData] = useState<PagedResponse<RequestLog> | null>(null);
  const [page, setPage] = useState(1);
  const params = new URLSearchParams();

  const [api, setApi] = useState<RequestLogType>({
    statusCode: 0,
    requestType: "",
    api: 0,
    fromDate: "",
    toDate: "",
  });

  const updateFilter = (data: RequestLogType) => {
    setPage(1);
    setApi(data);
  };

  useEffect(() => {
    params.set("page", page.toString());
    params.set("pageSize", "25");

    if (api.requestType) {
      params.set("httpMethod", api.requestType);
    }

    if (api.statusCode) {
      params.set("statusCode", api.statusCode.toString());
    }

    if (api.api) {
      params.set("apiId", api.api.toString());
    }

    if (api.fromDate) {
      params.set("fromDate", api.fromDate);
    }

    if (api.toDate) {
      params.set("endDate", api.toDate);
    }
    http
      .get<PagedResponse<RequestLog>>(`/api/request-logs?${params.toString()}`)
      .then((res) => {
        setData(res.data);
      });
  }, [page, api]);

  return (
    <div>
      <h1>Request Logs</h1>
      <Filter addFilter={updateFilter} />
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
            {data?.totalCount == 0 ? (
              <h1>No data is found</h1>
            ) : (
              data?.items.map((log) => (
                <tr key={log.id}>
                  <td>{new Date(log.createdAtUtc).toLocaleString()}</td>
                  <td>{log.httpMethod}</td>
                  <td>{log.requestPath}</td>

                  <td>{log.statusCode}</td>

                  <td>{log.responseTimeMs}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
        <div className="pager">
          <button
            disabled={page == 1}
            className="secondary"
            onClick={() => setPage((p) => p - 1)}
          >
            Previous
          </button>
          <span>Page {page}</span>
          <button
            disabled={!data || page * data.pageSize >= data.totalCount}
            className="secondary"
            onClick={() => setPage((p) => p + 1)}
          >
            Next
          </button>
        </div>
      </div>
    </div>
  );
}
