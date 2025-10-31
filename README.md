# 🔐 Login API - JWT Authentication

This API endpoint handles user authentication and returns a **JWT token** upon successful login.  
It validates the provided username and password against the database and issues a signed token for subsequent authorized requests.

---

## 📘 Endpoint Summary

**URL:** `/login`  
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
  "UserName": "john.doe",
  "Password": "MySecurePassword123"
}
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

