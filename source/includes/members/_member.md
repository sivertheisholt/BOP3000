# Members

<!--- GET --->

## GET all members

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/members", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
[
  {
    "username": "test"
  }
]
```

This endpoint retrieves all members.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members`

### Response Class (Status 200)

| Key      | Type   |
| -------- | ------ |
| username | string |

## GET a single member

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/members/1", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "username": "test"
}
```

This endpoint retrieves all users.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/1`

### URL Parameters

| Parameter | Description                    |
| --------- | ------------------------------ |
| ID        | The ID of the user to retrieve |

### Response Class (Status 200)

| Key      | Type   |
| -------- | ------ |
| username | string |

<!--- POST --->

<!--- PUT --->

<!--- DELETE --->
