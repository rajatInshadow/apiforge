import { useState } from "react";
import { RequestLogType } from "../../types/filter";

type ChildProps = {
  addFilter: (data: RequestLogType) => void;
};

const Filter = ({ addFilter }: ChildProps) => {
  const [filters, setFilters] = useState<RequestLogType>({
    statusCode: 0,
    requestType: "",
    api: "",
    fromDate: "",
    toDate: ""
  });

  function updateFilter<K extends keyof RequestLogType>(
    name: K,
    value: RequestLogType[K],
  ) {
    const updatedFilters = {
      ...filters,
      [name]: value,
    };

    console.log("upppp ",updatedFilters)

    setFilters(updatedFilters);
    addFilter(updatedFilters);
  }

  return (
    <>
      <h1>Filter component</h1>

      <label htmlFor="statusCode">Status Code</label>
      <select
        name="statusCode"
        id="statusCode"
        value={filters.statusCode}
        onChange={(e) => updateFilter("statusCode", Number(e.target.value))}
      >
        <option value="0">All</option>
        <option value="200">Success</option>
        <option value="500">Internal Server Error</option>
        <option value="404">Not Found</option>
      </select>

      <label htmlFor="Api">API</label>
      <select
        name="Api"
        id="Api"
        value={filters.api}
        onChange={(e) => updateFilter("api", e.target.value)}
      >
        <option value="">All</option>
        <option value="products">Product</option>
        <option value="orders">Order</option>
      </select>

      <label htmlFor="RequestType">Http Method</label>
      <select
        name="RequestType"
        id="RequestType"
        value={filters.requestType}
        onChange={(e) => updateFilter("requestType", e.target.value)}
      >
        <option value="">All</option>
        <option value="GET">GET</option>
        <option value="POST">POST</option>
        <option value="PUT">PUT</option>
        <option value="DELETE">DELETE</option>
      </select>

      <label htmlFor="fromDate">From Date</label>
      <input
        type="date"
        id="fromDate"
        value={filters.fromDate}
        onChange={(e) => updateFilter("fromDate", e.target.value)}
      />

      <label htmlFor="toDate">To Date</label>
      <input
        type="date"
        id="toDate"
        value={filters.toDate}
        onChange={(e) => updateFilter("toDate", e.target.value)}
      />
    </>
  );
};

export default Filter;
