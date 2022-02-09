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

`{ "username": "dave", "token": "<Token>" }`

## POST Register

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

<!--- PUT --->

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
