# Users

<!--- GET --->

## GET all users

```javascript
var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/users", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{}
```

This endpoint retrieves all users.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/users/<ID>`

## GET a single user

```javascript
var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/users/1", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{}
```

This endpoint retrieves all users.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/users/<ID>`

### URL Parameters

| Parameter | Description                    |
| --------- | ------------------------------ |
| ID        | The ID of the user to retrieve |

<!--- POST --->

<!--- PUT --->

<!--- DELETE --->
