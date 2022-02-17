# Apps

<!--- GET --->

## GET Search for app

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/apps/search?name=counter strike global offensive&limit=5",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
[
  {
    "appId": 730,
    "name": "Counter-Strike: Global Offensive"
  },
  {
    "appId": 740,
    "name": "Counter-Strike Global Offensive - Dedicated Server"
  },
  {
    "appId": 745,
    "name": "Counter-Strike: Global Offensive - SDK"
  },
  {
    "appId": 1020710,
    "name": "Counter-Strike Flair"
  },
  {
    "appId": 453590,
    "name": "Counter-Strike Nexon: Zombies - Teddy Nightmare (30 Days)"
  }
]
```

This endpoint retrieves the results from the search.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/apps/search`

### Response Class (Status 200)

| Key   | Type   |
| ----- | ------ |
| appId | int    |
| name  | string |

### URL Parameters

| Parameter | Description                    | Default |
| --------- | ------------------------------ | ------- |
| title     | The ID of the user to retrieve |         |
| limit     | The ID of the user to retrieve | 10      |

<!--- POST --->

<!--- PUT --->

<!--- DELETE --->
