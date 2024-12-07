# Simple System for Registering Students

This project is a simple system designed for registering students, managing staff, and assigning roles. It is built using **ASP.NET Core** and **Entity Framework** with a clean architecture structure that separates concerns into repositories, services, and controllers.
Add Migration and Update Database
After configuring the database settings in the appsettings.json file, you can generate and update the database using Entity Framework Core by running the following commands:

Add a New Migration: To generate a migration based on the changes made to your models or database, run the following command:

--Add-Migration <Migration_Name>

Replace <Migration_Name> with a descriptive name for the migration.

Update the Database: After creating the migration, apply it to the database to update its structure by running:

--Update-Database

Once these commands are executed, the database will be created or updated according to the migration changes.

## Project Structure

### 1. **Repositories**

- **IStudentRepository**: Interface for student-related database operations.
- **IStaffRepository**: Interface for staff-related database operations.
- **StudentRepository**: Implementation of `IStudentRepository` handling database operations for students.
- **StaffRepository**: Implementation of `IStaffRepository` handling database operations for staff.

### 2. **Services**

- **IStudentService**: Interface that defines business logic related to student management.
- **IStaffService**: Interface that defines business logic related to staff management.
- **StudentService**: Implementation of `IStudentService` that includes logic for adding, updating, and deleting students.
- **StaffService**: Implementation of `IStaffService` that includes logic for authentication, staff registration, role management, and student assignment.

### 3. **DTOs**

- **StudentDto**: Data Transfer Object used for adding new students with only essential fields.

### 4. **Models**

- **Student**: Represents a student with properties like `FirstName`, `LastName`, `Email`, etc.
- **Staff**: Represents a staff member with properties like `Email`, `PasswordHash`, and `Role`.

### 5. **Utilities**

- **BCrypt**: Used for hashing passwords to ensure secure staff authentication.

## Key Features

- **Staff Authentication**: Staff can log in using email and password, where the password is securely hashed using BCrypt.
- **Permissions and Roles**: The system implements role-based access control, where only Admins or Managers can access specific functionalities like adding or deleting students.
- **Student Management**: Admins or Managers can add, update, and delete students, and view all students assigned to a specific staff member.
- **Staff Role Management**: Admins can manage staff roles, including updating the role of a staff member.

## How It Works

### 1. **Authentication**

- Staff authentication is handled via the `StaffService` using the email and password, with password verification done using `BCrypt`.

### 2. **Student Management**

- **Add Student**: Admins or Managers can add new students via `StudentService`. The system allows adding a student with basic details like name, email, phone number, and staff assignment.
- **Get All Students**: Admins or Managers can view all students.
- **Update and Delete Student**: Admins or Managers can update student details or delete a student from the system.

### 3. **Role Management**

- Staff roles can be updated by the Admin using the `UpdateRoleAsync` method.
- The first staff member added will automatically be assigned an Admin role.

### 4. **Permissions Check**

- Role-based permissions ensure that only authorized users (Admin/Manager) can perform sensitive actions like adding or deleting staff, updating roles, and viewing students.

## Technologies Used

- **ASP.NET Core**: Framework for building the web application.
- **Entity Framework Core**: ORM used for database operations.
- **BCrypt.Net**: For password hashing and authentication.
- **SQL Server**: Database for storing the staff and student records.

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/simple-system-for-registering-students.git
   ```

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
