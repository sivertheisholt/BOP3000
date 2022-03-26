# Lobbies

<!--- GET --->

## GET Specific lobby

```javascript
var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/lobbies/1", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "id": 1,
  "maxUsers": 5,
  "title": "Whats up gamers",
  "steamId": null,
  "gameId": 1,
  "gameType": "Competetive",
  "lobbyDescription": "Hello there",
  "adminUid": 1,
  "adminUsername": "adminTest",
  "users": [1],
  "lobbyRequirement": null
}
```

This endpoint retrieves a specific lobby.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/lobbies/<ID>`

### Response Class (Status 200)

| Key              | Type             |
| ---------------- | ---------------- |
| id               | int              |
| maxUsers         | int              |
| title            | string           |
| gameId           | int              |
| steamId          | int              |
| gameType         | string           |
| lobbyDescription | string, optional |
| adminUid         | string           |
| adminUsername    | string           |
| users            | int[]            |
| lobbyRequirement | LobbyRequirement |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

## GET All lobbies

```javascript
var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/lobbies", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
[
  {
    "id": 1,
    "maxUsers": 5,
    "title": "Whats up gamers",
    "steamId": null,
    "gameId": 51,
    "gameType": "Competetive",
    "lobbyDescription": "Hello there",
    "adminUid": 1,
    "adminUsername": "adminTest",
    "users": [1],
    "lobbyRequirement": null
  }
]
```

This endpoint retrieves all lobbies.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/lobbies`

### Response Class (Status 200)

| Key              | Type             |
| ---------------- | ---------------- |
| id               | int              |
| maxUsers         | int              |
| title            | string           |
| gameId           | int              |
| steamId          | int              |
| gameType         | string           |
| lobbyDescription | string, optional |
| adminUid         | string           |
| adminUsername    | string           |
| users            | int[]            |
| lobbyRequirement | LobbyRequirement |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

## GET Lobbies with specific game ID

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/lobbies/game/51", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
[
  {
    "id": 1,
    "maxUsers": 5,
    "title": "Whats up gamers",
    "steamId": null,
    "gameId": 51,
    "gameType": "Competetive",
    "lobbyDescription": "Hello there",
    "adminUid": 1,
    "adminUsername": "AdminTest",
    "users": [1],
    "lobbyRequirement": {
      "gender": "Male"
    }
  }
]
```

This endpoint retrieves all lobbies with a specific game ID.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/lobbies/game/<ID>`

### Response Class (Status 200)

| Key              | Type             |
| ---------------- | ---------------- |
| id               | int              |
| maxUsers         | int              |
| title            | string           |
| gameId           | int              |
| steamId          | int              |
| gameType         | string           |
| lobbyDescription | string, optional |
| adminUid         | int              |
| adminUsername    | string           |
| users            | int[]            |
| lobbyRequirement | LobbyRequirement |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

<!--- POST --->

## POST A new lobby

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
