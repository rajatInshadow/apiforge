import { useEffect, useState } from "react";
import { http } from "../../services/http";
import { PagedResponse, RequestLog } from "../../types/api";
import Filter from "../../components/filter/filter";
import { RequestLogType } from "../../types/filter";
import { getApiNameFromPath } from "../../utils/stringUtils";

export function LogsPage() {
  const [data, setData] = useState<PagedResponse<RequestLog> | null>(null);
  const [page, setPage] = useState(1);

  const [api, setApi] = useState<RequestLogType>({
    statusCode: 0,
    requestType: "",
    api: 0,
    fromDate: "",
    toDate: "",
  });

  const updateFilter = (data: RequestLogType) => {
    setApi(data);
  };

  useEffect(() => {
    http
      .get<
        PagedResponse<RequestLog>
      >(`/api/request-logs?page=${page}&pageSize=25&apiId=${api.api}&httpMethod=${api.requestType}&fromDate=${api.fromDate}&endDate=${api.toDate}`)
      .then((res) => {
        setData(res.data);
        console.log("api response ", res);
      });
  }, [page,api]);

  useEffect(() => {
    console.log("api", api);
    console.log("data ", data);
  }, [api]);

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
            {data?.items
              .filter((x) => {
                const apiStatusCode =
                  api.statusCode === 0 || x.statusCode === api.statusCode;
                const apiRequestType =
                  api.requestType === "" || x.httpMethod === api.requestType;
                const apiApi =
                  api.api === 0 ||
                x.registeredApiId === api.api;
                console.log("api api api ",api, " ",api.statusCode === 0 || x.statusCode === api.statusCode)

                const logDate = new Date(x.createdAtUtc);

                const fromDateMatch =
                  api.fromDate === "" || logDate >= new Date(api.fromDate);

                const toDateEnd = api.toDate ? new Date(api.toDate) : null;

                if (toDateEnd) {
                  toDateEnd.setHours(23, 59, 59, 999);
                }

                const toDateMatch = !toDateEnd || logDate <= toDateEnd;

                return (
                  apiStatusCode &&
                  apiRequestType &&
                  apiApi &&
                  fromDateMatch &&
                  toDateMatch
                );
              })
              .map((log) => (
                
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
