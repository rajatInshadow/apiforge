import { useEffect, useState } from "react";
import Box from "@mui/material/Box";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import Paper from "@mui/material/Paper";
import Select from "@mui/material/Select";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import type { SelectChangeEvent } from "@mui/material/Select";
import { RequestLogType } from "../../types/filter";
import Button from "@mui/material/Button";
import { RegisteredApi } from "../../types/api";
import { http } from "../../services/http";
type ChildProps = {
  addFilter: (data: RequestLogType) => void;
};

const initialFilters: RequestLogType = {
  statusCode: 0,
  requestType: "",
  api: 0,
  fromDate: "",
  toDate: "",
};

export default function Filter({ addFilter }: ChildProps) {
  const [filters, setFilters] = useState<RequestLogType>(initialFilters);
  const [apis, setApis] = useState<RegisteredApi[]>();

  function updateFilter<Key extends keyof RequestLogType>(
    name: Key,
    value: RequestLogType[Key],
  ) {
    const updatedFilters = {
      ...filters,
      [name]: value,
    };

    setFilters(updatedFilters);
    addFilter(updatedFilters);
  }

  function handleStatusCodeChange(event: SelectChangeEvent<number>) {
    updateFilter("statusCode", Number(event.target.value));
  }

  function handleApiChange(event: SelectChangeEvent<number>) {
    updateFilter("api", Number(event.target.value));
  }

  function handleRequestTypeChange(event: SelectChangeEvent) {
    updateFilter("requestType", event.target.value);
  }

  function resetFilter() {
    setFilters(initialFilters);
    addFilter(initialFilters);
  }

  useEffect(() => {
    http.get<RegisteredApi[]>(`/api/apis`).then((res) => {
      setApis(res.data);
    });
  }, []);

  return (
    <Paper
      component="section"
      elevation={0}
      sx={{
        width: "100%",
        mx: "auto",
        p: { xs: 3, sm: 4 },
        border: 1,
        borderColor: "divider",
        borderRadius: 3,
        marginBottom: "40px",
      }}
    >
      <Typography component="h1" variant="h4" sx={{ mb: 1, fontWeight: 700 }}>
        Filter component
      </Typography>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
        Narrow down requests by status, API, method, or date range.
      </Typography>

      <Box
        component="form"
        aria-label="Request filters"
        onSubmit={(event) => event.preventDefault()}
        sx={{
          display: "grid",
          gridTemplateColumns: {
            xs: "1fr",
            sm: "repeat(2, minmax(0, 1fr))",
            md: "repeat(3, minmax(0, 1fr))",
          },
          gap: 2,
        }}
      >
        <FormControl fullWidth>
          <InputLabel id="status-code-label">Status Code</InputLabel>
          <Select<number>
            labelId="status-code-label"
            id="statusCode"
            name="statusCode"
            value={filters.statusCode}
            label="Status Code"
            onChange={handleStatusCodeChange}
          >
            <MenuItem value={0}>All</MenuItem>
            <MenuItem value={200}>Success</MenuItem>
            <MenuItem value={500}>Internal Server Error</MenuItem>
            <MenuItem value={404}>Not Found</MenuItem>
          </Select>
        </FormControl>

        <FormControl fullWidth>
          <InputLabel id="api-label">API</InputLabel>

          <Select<number>
            labelId="api-label"
            id="api"
            name="api"
            value={filters.api}
            label="API"
            onChange={handleApiChange}
          >
            <MenuItem value={0}>All</MenuItem>
            {apis?.map((item) => (
              <MenuItem value={item.id}>{item.name}</MenuItem>
            ))}
          </Select>
        </FormControl>

        <FormControl fullWidth>
          <InputLabel id="request-type-label">HTTP Method</InputLabel>
          <Select
            labelId="request-type-label"
            id="requestType"
            name="requestType"
            value={filters.requestType}
            label="HTTP Method"
            onChange={handleRequestTypeChange}
          >
            <MenuItem value="">All</MenuItem>
            <MenuItem value="GET">GET</MenuItem>
            <MenuItem value="POST">POST</MenuItem>
            <MenuItem value="PUT">PUT</MenuItem>
            <MenuItem value="DELETE">DELETE</MenuItem>
          </Select>
        </FormControl>

        <TextField
          fullWidth
          id="fromDate"
          name="fromDate"
          label="From Date"
          type="date"
          value={filters.fromDate}
          onChange={(event) => updateFilter("fromDate", event.target.value)}
          slotProps={{ inputLabel: { shrink: true } }}
        />

        <TextField
          fullWidth
          id="toDate"
          name="toDate"
          label="To Date"
          type="date"
          value={filters.toDate}
          onChange={(event) => updateFilter("toDate", event.target.value)}
          slotProps={{ inputLabel: { shrink: true } }}
        />
        <Button variant="outlined" onClick={() => resetFilter()}>
          CLear filter
        </Button>
      </Box>
    </Paper>
  );
}
