AuthController
This controller manages authentication and user registration. It provides the following endpoints:

Endpoints
POST /api/Auth/register

Description: Registers a new user (staff).
Request Body: containing:RegisterDto
Email: Email address of the user.
Password: Password for the account.
ConfirmPassword: Password confirmation.
Responses:
200 OK: Registration successful.
400 Bad Request: Invalid input data.
409 Conflict: Email is already in use.
POST /api/Auth/login

Description: Logs in a user and returns a JWT token.
Request Body: containing:LoginDto
Email: Email address of the user.
Password: Password for the account.
Responses:
200 OK: Login successful, with a JWT token.
400 Bad Request: Missing or invalid input data.
401 Unauthorized: Incorrect email or password.
Additional Notes
Passwords are hashed using .BCrypt
