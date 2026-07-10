export type AuthResponse = {
  userId: number;
  email: string;
  fullName: string;
  role: 'Admin' | 'Developer';
  token: string;
};

export type RegisteredApi = {
  id: number;
  name: string;
  description: string;
  routePrefix: string;
  downstreamBaseUrl: string;
  status: string;
  createdAtUtc: string;
  updatedAtUtc?: string | null;
};

export type ApiKey = {
  id: number;
  name: string;
  keyPrefix: string;
  registeredApiId?: number | null;
  isActive: boolean;
  createdAtUtc: string;
  expiresAtUtc?: string | null;
};

export type ApiKeyCreateResponse = ApiKey & {
  plainTextKey: string;
};

export type RequestLog = {
  id: number;
  registeredApiId?: number | null;
  apiName?: string | null;
  apiKeyId?: number | null;
  httpMethod: string;
  requestPath: string;
  statusCode: number;
  responseTimeMs: number;
  clientIp?: string | null;
  errorMessage?: string | null;
  createdAtUtc: string;
};

export type PagedResponse<T> = {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
};

export type DashboardSummary = {
  totalRequests: number;
  successfulRequests: number;
  failedRequests: number;
  averageResponseTimeMs: number;
  topApis: Array<{ registeredApiId: number; apiName: string; requestCount: number; averageResponseTimeMs: number }>;
  recentFailures: Array<{ logId: number; requestPath: string; statusCode: number; errorMessage?: string | null; createdAtUtc: string }>;
};
