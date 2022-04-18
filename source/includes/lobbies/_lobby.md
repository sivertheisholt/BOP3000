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
  "users": [1, 2],
  "log": {
    "messages": [
      {
        "uid": 1,
        "dateSent": "2022-04-14T14:33:55.1220942",
        "username": "Playfu1",
        "chatMessage": "yo"
      },
      {
        "uid": 1,
        "dateSent": "2022-04-14T14:33:55.9965347",
        "username": "Playfu1",
        "chatMessage": "sup"
      },
      {
        "uid": 2,
        "dateSent": "2022-04-14T14:33:58.4013644",
        "username": "Playfu2",
        "chatMessage": "not much"
      },
      {
        "uid": 2,
        "dateSent": "2022-04-14T14:33:59.6158034",
        "username": "Playfu2",
        "chatMessage": "how are u"
      },
      {
        "uid": 2,
        "dateSent": "2022-04-14T14:34:00.9457239",
        "username": "Playfu2",
        "chatMessage": "test"
      }
    ]
  },
  "lobbyRequirement": null,
  "startDate": "2022-03-30T04:08:03.088913",
  "finishedDate": "0001-01-01T00:00:00",
  "finished": true,
  "votes": [
    {
      "voterUid": 2,
      "votedUid": 1,
      "upvote": true
    }
  ]
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
| votes            | Vote[]           |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

#### Vote

| Key      | Type |
| -------- | ---- |
| voterUid | int  |
| votedUid | int  |
| upvote   | int  |

#### Log

| Key      | Type      |
| -------- | --------- |
| messages | Message[] |

#### Message

| Key         | Type   |
| ----------- | ------ |
| uid         | int    |
| dateSent    | string |
| username    | string |
| chatMessage | string |

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
    "finished": true,
    "votes": [
      {
        "voterUid": 2,
        "votedUid": 1,
        "upvote": true
      }
    ]
  }
]
```

This endpoint retrieves all lobbies.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/lobbies`

### Query Parameters

| Parameter  | Default | Description              |
| ---------- | ------- | ------------------------ |
| pageNumber | 1       | Page to get results from |
| pageSize   | 50      | Size of a page           |

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
| votes            | Vote[]           |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

#### Vote

| Key      | Type |
| -------- | ---- |
| voterUid | int  |
| votedUid | int  |
| upvote   | int  |

#### Log

| Key | Type |
| --- | ---- |

## GET Active lobbies with specific game ID

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
    "finished": true,
    "votes": [
      {
        "voterUid": 2,
        "votedUid": 1,
        "upvote": true
      }
    ]
  }
]
```

This endpoint retrieves all lobbies with a specific game ID.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/lobbies/game/<ID>`

### Query Parameters

| Parameter  | Default | Description              |
| ---------- | ------- | ------------------------ |
| pageNumber | 1       | Page to get results from |
| pageSize   | 50      | Size of a page           |

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
| votes            | Vote[]           |

#### LobbyRequirement

| Key    | Type             |
| ------ | ---------------- |
| gender | string, optional |

#### Vote

| Key      | Type |
| -------- | ---- |
| voterUid | int  |
| votedUid | int  |
| upvote   | int  |

#### Log

| Key | Type |
| --- | ---- |

## GET Check if lobby is finished

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/lobbies/7/finished",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns true/false

This endpoint checks if lobby is finished.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/lobbies/<ID>/finished`

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

<!--- PATCH --->

## PATCH Upvote

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var requestOptions = {
  method: "PATCH",
  headers: myHeaders,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/lobbies/7/upvote/1",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will upvote another member

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/lobbies/<ID>/upvote/<UID>`

## PATCH Downvote

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var requestOptions = {
  method: "PATCH",
  headers: myHeaders,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/lobbies/7/downvote/1",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will downvote another member

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/lobbies/<ID>/downvote/<UID>`

<!--- DELETE --->
