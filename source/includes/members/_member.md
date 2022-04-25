# Members

<!--- GET --->

## GET All members

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
    "email": "member@test.com",
    "username": "membertest",
    "memberProfile": {
      "birthday": "1998-07-30T00:00:00",
      "age": 0,
      "gender": "Male",
      "description": "Test2",
      "countryIso": {
        "id": 1,
        "name": "Afghanistan",
        "twoLetterCode": "AF",
        "threeLetterCode": "AFG",
        "numericCode": "004"
      },
      "memberData": {
        "upvotes": 10,
        "downvotes": 5,
        "followers": [2, 3, 4, 6, 7],
        "following": [2, 3, 4, 6, 7],
        "userFavoriteGames": [2, 3, 4, 6, 7],
        "finishedLobbies": [2, 3, 4, 6, 7]
      },
      "memberCustomization": {
        "backgroundUrl": "https://res.cloudinary.com/dzpzecnx5/image/upload/v1650547054/AccountCustomizerIcons/accountbg1-icon_lsztqe.jpg"
      },
      "memberPhoto": {
        "id": 2,
        "url": "https://res.cloudinary.com/dzpzecnx5/image/upload/v1648988522/qyf8mc0optfwaebzj0cp.png"
      }
    }
  }
]
```

This endpoint retrieves all members.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members`

### Query Parameters

| Parameter  | Default | Description              |
| ---------- | ------- | ------------------------ |
| pageNumber | 1       | Page to get results from |
| pageSize   | 50      | Size of a page           |

### Response Class (Status 200)

| Key           | Type          |
| ------------- | ------------- |
| username      | string        |
| email         | string        |
| memberProfile | MemberProfile |

#### MemberProfile

| Key                 | Type                |
| ------------------- | ------------------- |
| birthday            | string              |
| age                 | int                 |
| gender              | string              |
| description         | string              |
| countryIso          | CountryIso          |
| memberPhoto         | MemberPhoto         |
| memberData          | MemberData          |
| memberCustomization | MemberCustomization |

#### CountryIso

| Key             | Type   |
| --------------- | ------ |
| id              | int    |
| name            | string |
| twoLetterCode   | string |
| ThreeLetterCode | string |
| numericCode     | string |

#### MemberPhoto

| Key | Type   |
| --- | ------ |
| id  | int    |
| url | string |

#### MemberData

| Key               | Type  |
| ----------------- | ----- |
| upvotes           | int   |
| downvotes         | int   |
| followers         | int[] |
| following         | int[] |
| userFavoriteGames | int[] |
| finishedLobbies   | int[] |

#### MemberCustomizaiton

| Key           | Type   |
| ------------- | ------ |
| backgroundUrl | string |

## GET A specific member

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
  "email": "member@test.com",
  "username": "membertest",
  "memberProfile": {
    "birthday": "1998-07-30T00:00:00",
    "age": 0,
    "gender": "Male",
    "description": "Test2",
    "countryIso": {
      "id": 1,
      "name": "Afghanistan",
      "twoLetterCode": "AF",
      "threeLetterCode": "AFG",
      "numericCode": "004"
    },
    "memberData": {
      "upvotes": 10,
      "downvotes": 5,
      "followers": [2, 3, 4, 6, 7],
      "following": [2, 3, 4, 6, 7],
      "userFavoriteGames": [2, 3, 4, 6, 7],
      "finishedLobbies": [2, 3, 4, 6, 7]
    },
    "memberCustomization": {
      "backgroundUrl": "https://res.cloudinary.com/dzpzecnx5/image/upload/v1650547054/AccountCustomizerIcons/accountbg1-icon_lsztqe.jpg"
    },
    "memberPhoto": {
      "id": 2,
      "url": "https://res.cloudinary.com/dzpzecnx5/image/upload/v1648988522/qyf8mc0optfwaebzj0cp.png"
    }
  }
}
```

This endpoint retrieves a specific member.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/1`

### URL Parameters

| Parameter | Description                    |
| --------- | ------------------------------ |
| ID        | The ID of the user to retrieve |

### Response Class (Status 200)

| Key           | Type          |
| ------------- | ------------- |
| email         | string        |
| username      | string        |
| memberProfile | MemberProfile |

#### MemberProfile

| Key                 | Type                |
| ------------------- | ------------------- |
| birthday            | string              |
| age                 | int                 |
| gender              | string              |
| description         | string              |
| countryIso          | CountryIso          |
| memberPhoto         | MemberPhoto         |
| memberCustomization | MemberCustomization |

#### CountryIso

| Key             | Type   |
| --------------- | ------ |
| id              | int    |
| name            | string |
| twoLetterCode   | string |
| ThreeLetterCode | string |
| numericCode     | string |

#### MemberPhoto

| Key | Type   |
| --- | ------ |
| id  | int    |
| url | string |

#### MemberData

| Key               | Type  |
| ----------------- | ----- |
| upvotes           | int   |
| downvotes         | int   |
| followers         | int[] |
| following         | int[] |
| userFavoriteGames | int[] |
| finishedLobbies   | int[] |

#### MemberCustomizaiton

| Key           | Type   |
| ------------- | ------ |
| backgroundUrl | string |

## GET Current member info

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/members/current", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "email": "member@test.com",
  "username": "membertest",
  "memberProfile": {
    "birthday": "1998-07-30T00:00:00",
    "age": 0,
    "gender": "Male",
    "description": "Test2",
    "countryIso": {
      "id": 1,
      "name": "Afghanistan",
      "twoLetterCode": "AF",
      "threeLetterCode": "AFG",
      "numericCode": "004"
    },
    "memberData": {
      "upvotes": 10,
      "downvotes": 5,
      "followers": [2, 3, 4, 6, 7],
      "following": [2, 3, 4, 6, 7],
      "userFavoriteGames": [2, 3, 4, 6, 7],
      "finishedLobbies": [2, 3, 4, 6, 7]
    },
    "memberCustomization": {
      "backgroundUrl": "https://res.cloudinary.com/dzpzecnx5/image/upload/v1650547054/AccountCustomizerIcons/accountbg1-icon_lsztqe.jpg"
    },
    "memberPhoto": {
      "id": 2,
      "url": "https://res.cloudinary.com/dzpzecnx5/image/upload/v1648988522/qyf8mc0optfwaebzj0cp.png"
    }
  }
}
```

This endpoint retrieves the current member.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/current`

### Response Class (Status 200)

| Key           | Type          |
| ------------- | ------------- |
| email         | string        |
| username      | string        |
| memberProfile | MemberProfile |

#### MemberProfile

| Key                 | Type                |
| ------------------- | ------------------- |
| birthday            | string              |
| age                 | int                 |
| gender              | string              |
| description         | string              |
| countryIso          | CountryIso          |
| memberPhoto         | MemberPhoto         |
| memberData          | MemberData          |
| memberCustomization | Membercustomization |

#### CountryIso

| Key             | Type   |
| --------------- | ------ |
| id              | int    |
| name            | string |
| twoLetterCode   | string |
| ThreeLetterCode | string |
| numericCode     | string |

#### MemberPhoto

| Key | Type   |
| --- | ------ |
| id  | int    |
| url | string |

#### MemberData

| Key               | Type  |
| ----------------- | ----- |
| upvotes           | int   |
| downvotes         | int   |
| followers         | int[] |
| following         | int[] |
| userFavoriteGames | int[] |
| finishedLobbies   | int[] |

#### MemberCustomization

| Key           | Type   |
| ------------- | ------ |
| backgroundUrl | string |

## GET Current member lobby status

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/lobby-status",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "inQueue": true,
  "lobbyId": 1
}
```

This endpoint retrieves the current member lobby status.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/lobby-status`

### Response Class (Status 200)

| Key     | Type |
| ------- | ---- |
| inQueue | bool |
| lobbyId | int  |

## GET Check if following member

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/check-follow?memberId=1",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns true/False

This endpoint checks if member is followed

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/check-follow?memberId=1`

### Response Class (Status 200)

True/False

## GET Check if mail exists

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/check-mail-exists?mail=test",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns true/False

This endpoint checks if mail is already taken

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/check-mail-exists?mail=test`

### Response Class (Status 200)

True/False

## GET Check if blocking user

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/check-blocking?memberId=1",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns true/False

This endpoint checks if blocking user

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/check-blocking?memberId=1`

### Response Class (Status 200)

True/False

## GET Check if blocked

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/check-blocked?memberId=1",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns true/False

This endpoint checks if blocked by a user

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/check-blocked?memberId=1`

### Response Class (Status 200)

True/False

## GET Search for member

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/search?name=admin",
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
    "id": 1,
    "userName": "adminTest"
  }
]
```

This endpoint searches for members.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/search?name=admin`

### Response Class (Status 200)

| Key      | Type   |
| -------- | ------ |
| id       | int    |
| userName | string |

## GET Current member activity log

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/current/activity",
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
    "date": "2022-03-31T00:00:00",
    "username": "membertest",
    "appUserId": 2,
    "identifier": "lobby-created",
    "lobbyId": 1,
    "gameName": "Counter-Strike: Global Offensive",
    "gameId": 1,
    "headerImage": "https://cdn.akamai.steamstatic.com/steam/apps/730/header.jpg?t=1641233427",
    "profilePicture": "https://res.cloudinary.com/dzpzecnx5/image/upload/v1650922825/TestImages/pf2_myeoew.png"
  }
]
```

This endpoint gets the current logged in member's activity list

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/current/activity`

### Response Class (Status 200)

| Key            | Type             |
| -------------- | ---------------- |
| date           | string           |
| username       | string           |
| appUserId      | int              |
| identifier     | string           |
| lobbyId        | int, optional    |
| gameName       | string, optional |
| gameId         | int, optional    |
| headerImage    | string, optional |
| profilePicture | string, optional |

## GET Discord connected status

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/members/1/discord", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "discordId": 960533457853874287,
  "connected": true,
  "username": "playfu",
  "discriminator": "2869",
  "hidden": false
}
```

This endpoint gets the discord connected status for a user

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/<ID>/discord`

### Response Class (Status 200)

| Key           | Type             |
| ------------- | ---------------- |
| discordId     | int              |
| connected     | bool             |
| username      | string, optional |
| discriminator | int, optional    |
| hidden        | bool             |

## GET Steam connected status

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/members/1/steam", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "connected": false,
  "steamId": 0,
  "hidden": false
}
```

This endpoint gets the steam connected status for a user

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/members/<ID>/steam`

### Response Class (Status 200)

| Key       | Type          |
| --------- | ------------- |
| connected | bool          |
| steamid   | int, optional |
| hidden    | bool          |

<!--- POST --->

## POST Set profile picture

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var formdata = new FormData();
formdata.append("File", fileInput.files[0], "Background.png");

var requestOptions = {
  method: "POST",
  headers: myHeaders,
  body: formdata,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/members/set-photo", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a JSON structured like this:

```json
{
  "id": 2,
  "url": "https://res.cloudinary.com/dzpzecnx5/image/upload/v1648988522/qyf8mc0optfwaebzj0cp.png"
}
```

This endpoint sets the profile picture for the current user

### HTTP Request

`POST https://bop3000.azurewebsites.net/api/members/set-photo`

## POST Set background

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  url: "https://res.cloudinary.com/dzpzecnx5/image/upload/v1650547054/AccountCustomizerIcons/accountbg1-icon_lsztqe.jpg",
});

var requestOptions = {
  method: "POST",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/set-background",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 No Content status code

This endpoint sets the background picture for the current user

### HTTP Request

`POST https://bop3000.azurewebsites.net/api/members/set-background`

<!--- PUT --->

<!--- PATCH --->

## PATCH Update a single member

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  userName: "TestName",
  email: "member@test.com",
  countryid: 1,
  gender: "Male",
  birthday: "2012-04-23T00:00:00.000Z",
  description: "Test",
});

var requestOptions = {
  method: "PATCH",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/members", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint updates a single member

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/members`

### Body Schema

| Key         | Type   |
| ----------- | ------ |
| username    | string |
| email       | string |
| countryId   | id     |
| gender      | string |
| birthday    | string |
| description | string |

## PATCH Follow a member

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var requestOptions = {
  method: "PATCH",
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/follow?memberId=1",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will follow another member

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/members/follow?memberId=1`

### Query Parameters

| Key      | Type |
| -------- | ---- |
| memberId | int  |

## PATCH Unfollow a member

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var requestOptions = {
  method: "PATCH",
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/members/unfollow?memberId=1",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will unfollow another member

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/members/unfollow?memberId=1`

### Query Parameters

| Key      | Type |
| -------- | ---- |
| memberId | int  |

## PATCH Block member

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var requestOptions = {
  method: "PATCH",
  headers: myHeaders,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/members/block/1", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will block another member

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/members/block/<ID>`

## PATCH Unblock member

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var requestOptions = {
  method: "PATCH",
  headers: myHeaders,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/members/unblock/1", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will block another member

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/members/unblock/<ID>`

## PATCH Unlink Discord

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
  "https://bop3000.azurewebsites.net/api/members/discord/unlink",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will unlink discord from account

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/members/discord/unlink`

## PATCH Unlink Steam

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
  "https://bop3000.azurewebsites.net/api/members/steam/unlink",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will unlink steam from account

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/members/steam/unlink`

## PATCH Hide Steam

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  hide: true,
});

var requestOptions = {
  method: "PATCH",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/members/steam/hide", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will hide steam from account

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/members/steam/hide`

### Body Schema

| Key  | Type |
| ---- | ---- |
| hide | bool |

## PATCH Hide Discord

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  hide: true,
});

var requestOptions = {
  method: "PATCH",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/members/discord/hide", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 204 (No content) status code

This endpoint will hide steam from account

### HTTP Request

`PATCH https://bop3000.azurewebsites.net/api/members/discord/hide`

### Body Schema

| Key  | Type |
| ---- | ---- |
| hide | bool |

<!--- DELETE --->
