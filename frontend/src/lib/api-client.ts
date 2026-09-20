export type User = { id: string; name: string; email: string; roles: string[]; emailVerifiedAt?: string | null };
export class ApiError extends Error {
  constructor(message: string, public status: number) { super(message); }
}
export async function apiRequest<T>(path: string, body?: unknown): Promise<T> {
  let response: Response;
  try {
    response = await fetch(`/api${path}`, {
      method: body === undefined ? "GET" : "POST", headers: { "Content-Type": "application/json" },
      body: body === undefined ? undefined : JSON.stringify(body),
      credentials: "same-origin", cache: "no-store", signal: AbortSignal.timeout(15000),
    });
  } catch { throw new ApiError("Unable to connect. Please check your connection and try again.", 0); }
  const data = await response.json().catch(() => ({}));
  if (!response.ok) {
    const errors = data.errors as Record<string, string[]> | undefined;
    throw new ApiError((errors ? Object.values(errors).flat().join(" ") : data.title) || "The request could not be completed. Please try again.", response.status);
  }
  return data as T;
}
