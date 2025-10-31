# Course Project Fall 2025- ITIS6200- Implementing a Concept or Security Protocol
# Topic: Implementation of JWT token with REST API
## 🛠️ Technologies
- REST API with C#
- SQL Server
- SQL Server Management Studio 21
- MS Visual Studio 2022
- Database Refer DB script table.sql into codebase

## 🧰 Installation
Install dependencies:
- dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
- dotnet add package System.IdentityModel.Tokens.Jwt
- dotnet add package Microsoft.Sql.SqlClient
- dotnet add package dapper

## 🚀 REST API Details
# 🔐 Login API - JWT Authentication

This API endpoint handles user authentication and returns a **JWT token** upon successful login.  
It validates the provided username and password against the database and issues a signed token for subsequent authorized requests.

---

## 📘 Endpoint Summary

**URL:** `/api/auth/login`  
**Method:** `POST`  
**Authorization:** ❌ Not required (this is the authentication endpoint)  
**Response Type:** `application/json`

---

## 🧾 Request

### **Body Parameters**

| Field       | Type     | Required | Description                   |
|--------------|----------|-----------|--------------------------------|
| `UserName`   | `string` | ✅        | The username of the user.      |
| `Password`   | `string` | ✅        | The user's password.           |

### Example Request
```json
{
  "UserName": "david",
  "Password": "passwordxyz"
}
```
✅ Successful Response

When valid credentials are provided, the API returns a JWT token that can be used to authenticate future requests.

Status Code: 200 OK

Example Response
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}

❌ Error Response

If the username or password is invalid, the API returns an error message with an appropriate code.

Status Code: 200 OK

Note: The response includes an ErrorCode field instead of using HTTP 401 directly.

Example Response
{
  "ErrorCode": 401,
  "Error": "Invalid username or password."
}





# 🎓 Student Management API - JWT & Role-Based Authorization

This API provides secure endpoints to **retrieve**, **add**, and **delete** students from the system.  
Access is protected using **JWT authentication** and **role-based authorization**, ensuring only users with specific roles can perform certain actions.

---

## 🔐 Authentication & Authorization

All endpoints require a valid **JWT token** in the `Authorization` header.  
Each route also enforces **role-based access** using the `[Authorize(Roles = "...")]` attribute.

### Possible Authorization Responses

| Status | Condition | Response Example |
|---------|------------|------------------|
| `401 Unauthorized` | Missing or invalid JWT token | `{ "Error": "Please log in to access this resource" }` |
| `403 Forbidden` | Authenticated but insufficient role permissions | `{ "Error": "Not authorized to perform this action" }` |

---

## 🧾 Endpoints Summary

| Endpoint | Method | Roles Allowed | Description |
|-----------|---------|----------------|--------------|
| `/StudentList` | `GET` | `Admin, Student, User` | Retrieve a list of all students. |
| `/AddStudent` | `POST` | `Admin, User` | Add a new student record and return the updated list. |
| `/DeleteStudent` | `POST` | `Admin, User` | Delete an existing student record and return the updated list. |

---

## 📘 1. Get All Students

### **Request**

**Method:** `GET`  
**URL:** `/Student/StudentList`  
**Headers:**  
Authorization: Bearer <your-jwt-token>

### **Successful Response**

**Status:** `200 OK`

```json
[
  {
    "StudentId": 1,
    "FirstName": "David",
    "MiddleName": "J",
    "LastName": "Wheeler",
    "EmailAddress": "d@test.com",
    "PhoneNumber": "24524524566"
  },
  {
    "StudentId": 2,
    "FirstName": "Peter",
    "MiddleName": "J",
    "LastName": "Fransis",
    "EmailAddress": "pFransis@test.com",
    "PhoneNumber": "8989898966"
  }
]
```
## 📘 2. Add New Student
### **Request**

**Method:** `POST`  
**URL:** `/Student/AddStudent`  
**Headers:**  
Authorization: Bearer <your-jwt-token>

Content-Type: application/json

Body Example:
```json
{
    "StudentId": 3,
    "FirstName": "Niami",
    "MiddleName": "J",
    "LastName": "Campbell",
    "EmailAddress": "Campbell@test.com",
    "PhoneNumber": "7878787878"
}
````
### **Successful Response**
Returns the updated list of students after adding the new record.

**Status:** `200 OK`
```json
[
 {
    "StudentId": 1,
    "FirstName": "David",
    "MiddleName": "J",
    "LastName": "Wheeler",
    "EmailAddress": "d@test.com",
    "PhoneNumber": "24524524566"
  },
  {
    "StudentId": 2,
    "FirstName": "Peter",
    "MiddleName": "J",
    "LastName": "Fransis",
    "EmailAddress": "pFransis@test.com",
    "PhoneNumber": "8989898966"
  }
  {
    "StudentId": 3,
    "FirstName": "Niami",
    "MiddleName": "J",
    "LastName": "Campbell",
    "EmailAddress": "Campbell@test.com",
    "PhoneNumber": "7878787878"
  }
]
```
## 📘 3. Delete Student
### **Request**

**Method:** `POST`  
**URL:** `/Student/DeleteStudent`  
**Headers:**  
Authorization: Bearer <your-jwt-token>

Content-Type: application/json

Body Example:
```json
{
  "studentId": 3
}
````
### **Successful Response**
Returns the updated list of students after deletion.

**Status:** `200 OK`
```json
[
  {
    "StudentId": 1,
    "FirstName": "David",
    "MiddleName": "J",
    "LastName": "Wheeler",
    "EmailAddress": "d@test.com",
    "PhoneNumber": "24524524566"
  },
  {
    "StudentId": 2,
    "FirstName": "Peter",
    "MiddleName": "J",
    "LastName": "Fransis",
    "EmailAddress": "pFransis@test.com",
    "PhoneNumber": "8989898966"
  }
]
```
