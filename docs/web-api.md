# MusicPortal Admin Web API

The REST API is hosted separately by `MusicPortal.WebApi` at `https://localhost:7001`. The administrator client is a separate static application (`MusicPortal.AdminSPA`) at `https://localhost:7202`. Browser calls are made with Fetch API and cross-origin access is configured by the `AdminSpa` CORS policy.

The API uses an HttpOnly cookie issued by `POST /api/auth/login`. The login endpoint accepts an administrator account only. Fetch sends the cookie using `credentials: 'include'`. All management endpoints require the `Admin` role. Requests without a valid cookie receive HTTP 401; authenticated non-admin users receive HTTP 403.

## Authentication

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/login` | Authenticate an administrator and issue the API cookie |
| POST | `/api/auth/logout` | Revoke the current API cookie |

Login body:

```json
{
  "username": "admin",
  "password": "StrongPassword123"
}
```

## Users — `/api/users`

| Method | Endpoint | Description | Success |
|---|---|---|---|
| GET | `/api/users` | Get all users (password hashes and salts are never returned) | 200 |
| GET | `/api/users/{id}` | Get one user | 200 / 404 |
| POST | `/api/users` | Create a user; the account is created unapproved and not as an administrator | 201 |
| PUT | `/api/users/{id}` | Update username, email, admin role and confirmation state | 204 |
| PATCH | `/api/users/{id}/approval` | Approve or reject/revoke approval | 204 |
| DELETE | `/api/users/{id}` | Delete a user (the current administrator cannot delete their own account) | 204 |

## Artists — `/api/artists`

| Method | Endpoint | Description | Success |
|---|---|---|---|
| GET | `/api/artists` | Get all artists | 200 |
| GET | `/api/artists/{id}` | Get one artist | 200 / 404 |
| POST | `/api/artists` | Create an artist | 201 |
| PUT | `/api/artists/{id}` | Update an artist | 204 |
| DELETE | `/api/artists/{id}` | Delete an artist if no records are linked | 204 / 409 |

## Genres — `/api/genres`

| Method | Endpoint | Description | Success |
|---|---|---|---|
| GET | `/api/genres` | Get all genres | 200 |
| GET | `/api/genres/{id}` | Get one genre | 200 / 404 |
| POST | `/api/genres` | Create a genre | 201 |
| PUT | `/api/genres/{id}` | Update a genre | 204 |
| DELETE | `/api/genres/{id}` | Delete a genre if no records are linked | 204 / 409 |

## CORS

The API reads its allowed SPA origin from `SpaOrigin` in `MusicPortal.WebApi/appsettings.json`. For local development it is `https://localhost:7202`. Do not replace this with `*` when credentials are enabled. `UseCors("AdminSpa")` runs after routing and before authentication/authorization.

The SPA's `wwwroot/js/api-config.js` points to `https://localhost:7001`. Change both values together for another deployment.

## Common HTTP status codes

- `200 OK` — a resource or collection was returned.
- `201 Created` — a resource was created; the response includes its representation and `Location` header.
- `204 No Content` — an update, approval change or deletion succeeded.
- `400 Bad Request` — invalid request or mismatched route/body IDs.
- `401 Unauthorized` — no valid API login cookie.
- `403 Forbidden` — the signed-in user is not an administrator.
- `404 Not Found` — the requested resource does not exist.
- `409 Conflict` — a user already exists or an artist/genre cannot be deleted because records refer to it.

All DTO responses omit password hashes and salts.
