# AuthController - Authentication and User Registration

This controller manages user authentication and registration. It provides the following endpoints for user management:

## Endpoints

### `POST /api/Auth/register`

**Description**: Registers a new user (staff).

#### Request Body (RegisterDto):

```json
{
  "Email": "user@example.com", // Email address of the user.
  "Password": "strongpassword", // Password for the account.
  "ConfirmPassword": "strongpassword" // Password confirmation.
}
```
