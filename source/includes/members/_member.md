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
        "userFavoriteGames": [2, 3, 4, 6, 7]
      }
    }
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
      "userFavoriteGames": [2, 3, 4, 6, 7]
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

| Key         | Type       |
| ----------- | ---------- |
| birthday    | string     |
| age         | int        |
| gender      | string     |
| description | string     |
| countryIso  | CountryIso |
| memberData  | MemberData |

#### CountryIso

| Key             | Type   |
| --------------- | ------ |
| id              | int    |
| name            | string |
| twoLetterCode   | string |
| ThreeLetterCode | string |
| numericCode     | string |

#### MemberData

| Key               | Type  |
| ----------------- | ----- |
| upvotes           | int   |
| downvotes         | int   |
| followers         | array |
| following         | array |
| userFavoriteGames | array |

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
      "userFavoriteGames": [2, 3, 4, 6, 7]
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

| Key         | Type       |
| ----------- | ---------- |
| birthday    | string     |
| age         | int        |
| gender      | string     |
| description | string     |
| countryIso  | CountryIso |
| memberData  | MemberData |

#### CountryIso

| Key             | Type   |
| --------------- | ------ |
| id              | int    |
| name            | string |
| twoLetterCode   | string |
| ThreeLetterCode | string |
| numericCode     | string |

#### MemberData

| Key               | Type  |
| ----------------- | ----- |
| upvotes           | int   |
| downvotes         | int   |
| followers         | array |
| following         | array |
| userFavoriteGames | array |

<!--- POST --->

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

<!--- DELETE --->
