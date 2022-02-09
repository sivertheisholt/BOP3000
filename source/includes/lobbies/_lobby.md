# Lobbies

<!--- GET --->

## GET all lobbies

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
[
  {
    "id": 1,
    "maxUsers": 10,
    "title": "TestHello",
    "steamId": null,
    "users": [],
    "lobbyRequirement": null,
    "gameId": 0
  }
]
```

This endpoint retrieves all users.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/lobbies`

### Query Parameters

| Parameter | Default | Description                  |
| --------- | ------- | ---------------------------- |
| page      | 1       | Each page returns 25 entries |

### Response Class (Status 200)

| Key              | Type             |
| ---------------- | ---------------- |
| id               | int              |
| maxUsers         | int              |
| title            | string           |
| lobbyDescription | string, optional |
| gameId           | int              |
| steamId          | int              |
| gameType         | string           |
| users            | int[]            |
| lobbyRequirement | LobbyRequirement |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

<!--- POST --->

## POST a new lobby

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  MaxUsers: 10,
  Title: "TestHello",
  GameId: 2,
  GameType: "Casual",
  LobbyRequirement: {},
});

var requestOptions = {
  method: "POST",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/lobbies", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "maxUsers": 10,
  "title": "TestHello",
  "lobbyDescription": null,
  "gameId": 2,
  "gameType": "Casual",
  "lobbyRequirement": {
    "gender": null
  }
}
```

This endpoint will create a new lobby.

### HTTP Request

`POST https://bop3000.azurewebsites.net/api/lobbies`

### Body Schema

| Key              | Type             |
| ---------------- | ---------------- |
| maxUsers         | int              |
| title            | string           |
| lobbyDescription | string, optional |
| gameId           | int              |
| gameType         | string           |
| lobbyRequirement | LobbyRequirement |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

### Response Class (Status 200)

| Key              | Type             |
| ---------------- | ---------------- |
| maxUsers         | int              |
| title            | string           |
| lobbyDescription | string           |
| gameId           | int              |
| gameType         | string           |
| lobbyRequirement | LobbyRequirement |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

<!--- PUT --->

<!--- DELETE --->
