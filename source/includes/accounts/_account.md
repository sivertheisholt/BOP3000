# Accounts

<!--- GET --->

<!--- POST --->

## POST Login

```javascript
var myHeaders = new Headers();
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  username: "dave",
  password: "DetVar1Gang!",
});

var requestOptions = {
  method: "POST",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/accounts/login", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "username": "dave",
  "token": "<Token>"
}
```

This endpoint will authorize an account.

### HTTP Request

`POST https://bop3000.azurewebsites.net/api/accounts/login`

### Body Schema

| Key      | Type   |
| -------- | ------ |
| username | string |
| password | string |

### Response Class (Status 200)

| Key      | Type   |
| -------- | ------ |
| username | string |
| token    | string |

## POST Register

```javascript
var myHeaders = new Headers();
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  username: "dave",
  email: "daveTest@test.com"
  password: "DetVar1Gang!",
});

var requestOptions = {
  method: "POST",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/accounts/register", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "username": "dave",
  "token": "<Token>"
}
```

This endpoint will create a new account.

### HTTP Request

`POST https://bop3000.azurewebsites.net/api/accounts/register`

### Body Schema

| Key      | Type   |
| -------- | ------ |
| username | string |
| email    | string |
| password | string |

### Response Class (Status 200)

| Key      | Type   |
| -------- | ------ |
| username | string |
| token    | string |

## POST Forgotten password

```javascript
var myHeaders = new Headers();
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  email: "member2@test.com",
});

var requestOptions = {
  method: "POST",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/accounts/forgotten_password",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 202 Accepted:

This endpoint will create a new token for resetting password.

### HTTP Request

`POST https://bop3000.azurewebsites.net/api/accounts/forgotten_password`

### Body Schema

| Key   | Type   |
| ----- | ------ |
| email | string |

<!--- PUT --->

## PUT Change forgotten password

```javascript
var myHeaders = new Headers();
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  email: "member2@test.com",
  newPassword: "Detvar1gang.",
});

var requestOptions = {
  method: "PUT",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/accounts/change_forgotten_password?token=<Token>",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 No Content:

This endpoint will update the password to the new password.

### HTTP Request

`PUT https://bop3000.azurewebsites.net/api/accounts/change_forgotten_password?token=<Token>`

### Query Parameters

| Parameter | Default | Description                                |
| --------- | ------- | ------------------------------------------ |
| token     |         | The generated token for forgotten password |

### Body Schema

| Key         | Type   |
| ----------- | ------ |
| email       | string |
| newPassword | string |

## PUT Change password

```javascript
var myHeaders = new Headers();
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  currentPassword: "Playfu123!!",
  newPassword: "Playfu123!",
});

var requestOptions = {
  method: "PUT",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch("https://localhost:5001/api/accounts/change_password", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 No Content:

This endpoint will update the password to the new password.

### HTTP Request

`PUT https://bop3000.azurewebsites.net/api/accounts/change_password`

### Body Schema

| Key             | Type   |
| --------------- | ------ |
| currentPassword | string |
| newPassword     | string |

<!--- DELETE --->

## DELETE Account

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "DELETE",
  headers: myHeaders,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/accounts/delete", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a string:

```json
"User deleted successfully!"
```

This endpoint will delete a user account

### HTTP Request

`DELETE https://bop3000.azurewebsites.net/api/accounts/delete`
