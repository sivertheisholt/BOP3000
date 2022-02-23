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

### Query Parameters

| Parameter | Description         | Default |
| --------- | ------------------- | ------- |
| name      | The name of the app |         |
| limit     | Max items to return | 10      |

## GET Single app

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/apps/2", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "id": 2,
  "name": "Carnival and Girls",
  "headerImage": "https://cdn.akamai.steamstatic.com/steam/apps/1573430/header.jpg?t=1617118165",
  "background": "https://cdn.akamai.steamstatic.com/steam/apps/1573430/page_bg_generated_v6b.jpg?t=1617118165"
}
```

This endpoint retrieves a single app

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/apps/<ID>`

### Response Class (Status 200)

| Key         | Type   |
| ----------- | ------ |
| id          | int    |
| name        | string |
| headerImage | string |
| background  | string |

## GET Active apps that has lobbies

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch("https://localhost:5001/api/apps/active", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
[
  {
    "id": 51,
    "name": "Counter-Strike: Global Offensive",
    "headerImage": "https://cdn.akamai.steamstatic.com/steam/apps/730/header.jpg?t=1641233427",
    "background": "https://cdn.akamai.steamstatic.com/steam/apps/730/page_bg_generated_v6b.jpg?t=1641233427"
  },
  {
    "id": 52,
    "name": "Lost Ark",
    "headerImage": "https://cdn.akamai.steamstatic.com/steam/apps/1599340/header.jpg?t=1644892919",
    "background": "https://cdn.akamai.steamstatic.com/steam/apps/1599340/page_bg_generated_v6b.jpg?t=1644892919"
  }
]
```

This endpoint retrieves all apps that has a lobby

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/apps/active`

### Response Class (Status 200)

| Key         | Type   |
| ----------- | ------ |
| id          | int    |
| name        | string |
| headerImage | string |
| background  | string |

<!--- POST --->

<!--- PUT --->

<!--- DELETE --->
