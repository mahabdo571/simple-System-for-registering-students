````markdown
# Authentication Controller - API Documentation

## Overview

This API controller manages user authentication and registration processes. It exposes two key endpoints:

1. **POST /api/Auth/register**: Registers a new user (staff).
2. **POST /api/Auth/login**: Authenticates a user and returns a JWT token.

This document provides detailed information on how to interact with these endpoints, including the request body, possible responses, and additional notes about how passwords are handled.

## Endpoints

### 1. `POST /api/Auth/register`

**Description**: Registers a new user (staff) in the system.

#### Request Body (`RegisterDto`):

```json
{
  "Email": "user@example.com", // Email address of the user (required).
  "Password": "strongpassword", // Password for the account (required).
  "ConfirmPassword": "strongpassword" // Password confirmation (required).
}
```
````

- **Email**: The email address of the user to be registered.
- **Password**: The password the user will use to log in.
- **ConfirmPassword**: A confirmation of the password, it must match the "Password" field.

#### Responses:

- **200 OK**: Registration successful.

  - Example response:

  ```json
  {
    "message": "Registration successful"
  }
  ```

- **400 Bad Request**: Invalid input data (e.g., missing required fields, password mismatch).

  - Example response:

  ```json
  {
    "message": "Email and Password are required"
  }
  ```

- **409 Conflict**: The provided email is already in use by another account.
  - Example response:
  ```json
  {
    "message": "Email is already in use"
  }
  ```

#### Additional Notes:

- Passwords are hashed using **BCrypt** before being stored in the database for security.
- Ensure that the "Password" and "ConfirmPassword" fields match before submitting the registration request.

---

### 2. `POST /api/Auth/login`

**Description**: Logs in a user and returns a JSON Web Token (JWT) for authenticated access.

#### Request Body (`LoginDto`):

```json
{
  "Email": "user@example.com", // Email address of the user (required).
  "Password": "strongpassword" // Password for the account (required).
}
```

- **Email**: The email address of the user trying to log in.
- **Password**: The password associated with the user's account.

#### Responses:

- **200 OK**: Login successful, with a JWT token.

  - Example response:

  ```json
  {
    "message": "Login successful",
    "token": "your-jwt-token"
  }
  ```

  - The `token` will be used for authenticating further requests that require a logged-in user.

- **400 Bad Request**: Missing or invalid input data (e.g., missing email or password).

  - Example response:

  ```json
  {
    "message": "Email and Password are required"
  }
  ```

- **401 Unauthorized**: Incorrect email or password.
  - Example response:
  ```json
  {
    "message": "Invalid email or password"
  }
  ```

#### Additional Notes:

- The JWT token generated upon successful login is used for authentication in subsequent requests to protected endpoints.
- JWT tokens have an expiration time; once expired, the user will need to log in again to obtain a new token.

---

## Security Notes

- **Passwords**: All user passwords are hashed using **BCrypt** before storing them in the database. This ensures that even if the database is compromised, passwords cannot be easily retrieved.
- **JWT Tokens**: The JWT tokens are signed and can be used to authenticate API requests. These tokens should be kept secure and should not be shared.

---

## Example Usage

### Example 1: Register a New User

#### Request:

```bash
POST /api/Auth/register
```

#### Request Body:

```json
{
  "Email": "user@example.com",
  "Password": "strongpassword",
  "ConfirmPassword": "strongpassword"
}
```

#### Response (200 OK):

```json
{
  "message": "Registration successful"
}
```

### Example 2: Log in to Get a JWT Token

#### Request:

```bash
POST /api/Auth/login
```

#### Request Body:

```json
{
  "Email": "user@example.com",
  "Password": "strongpassword"
}
```

#### Response (200 OK):

```json
{
  "message": "Login successful",
  "token": "your-jwt-token"
}
```

---

## Conclusion

This API provides a simple way to register new users and authenticate them using a JWT token. It ensures that passwords are securely hashed and provides appropriate error handling in case of invalid data or failed login attempts.

Make sure to secure your JWT tokens and use them for authenticating further requests to protected endpoints.

```

```
