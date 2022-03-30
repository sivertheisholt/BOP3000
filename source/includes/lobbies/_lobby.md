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
  "gameId": 51,
  "gameName": "Counter-Strike: Global Offensive",
  "gameType": "Competetive",
  "lobbyDescription": "Hello there",
  "adminUid": 1,
  "adminUsername": "adminTest",
  "users": [],
  "log": null,
  "lobbyRequirement": null,
  "startDate": "2022-03-30T04:08:03.088913",
  "finishedDate": "0001-01-01T00:00:00",
  "finished": false
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
| gameType         | string           |
| gameName         | string           |
| lobbyDescription | string, optional |
| adminUid         | string           |
| adminUsername    | string           |
| users            | int[]            |
| log              | Log, optional    |
| lobbyRequirement | LobbyRequirement |
| startDate        | Date             |
| finishedDate     | Date, optional   |
| finished         | bool             |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

#### Log

| Key | Type |
| --- | ---- |

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
    "id": 6,
    "maxUsers": 5,
    "title": "Play smth?",
    "gameId": 54,
    "gameName": "Dead by Daylight",
    "gameType": "Casual",
    "lobbyDescription": "Hah",
    "adminUid": 2,
    "adminUsername": "membertest",
    "users": [1, 2, 3],
    "log": null,
    "lobbyRequirement": null,
    "startDate": "2022-03-30T04:08:03.0981973",
    "finishedDate": "2022-03-30T04:08:03.098157",
    "finished": false
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
| gameType         | string           |
| gameName         | string           |
| lobbyDescription | string, optional |
| adminUid         | string           |
| adminUsername    | string           |
| users            | int[]            |
| log              | Log, optional    |
| lobbyRequirement | LobbyRequirement |
| startDate        | Date             |
| finishedDate     | Date, optional   |
| finished         | bool             |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

#### Log

| Key | Type |
| --- | ---- |

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
    "gameId": 51,
    "gameName": "Counter-Strike: Global Offensive",
    "gameType": "Competetive",
    "lobbyDescription": "Hello there",
    "adminUid": 1,
    "adminUsername": "adminTest",
    "users": [],
    "log": null,
    "lobbyRequirement": null,
    "startDate": "2022-03-30T04:08:03.088913",
    "finishedDate": "0001-01-01T00:00:00",
    "finished": false
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
| gameType         | string           |
| gameName         | string           |
| lobbyDescription | string, optional |
| adminUid         | string           |
| adminUsername    | string           |
| users            | int[]            |
| log              | Log, optional    |
| lobbyRequirement | LobbyRequirement |
| startDate        | Date             |
| finishedDate     | Date, optional   |
| finished         | bool             |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

#### Log

| Key | Type |
| --- | ---- |

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
