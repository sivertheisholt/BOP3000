# Countries

<!--- GET --->

## GET A specific country

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/countries/1", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
{
  "name": "Afghanistan",
  "twoLetterCode": "AF",
  "threeLetterCode": "AFG",
  "numericCode": "004"
}
```

This endpoint retrieves a specific country.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/countries/<ID>`

### URL Parameters

| Parameter | Description                       |
| --------- | --------------------------------- |
| ID        | The ID of the country to retrieve |

### Response Class (Status 200)

| Key             | Type   |
| --------------- | ------ |
| name            | string |
| twoLetterCode   | string |
| threeLetterCode | string |
| numericCode     | string |

## GET All countries

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  redirect: "follow",
};

fetch("https://bop3000.azurewebsites.net/api/countries", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns JSON structured like this:

```json
[
  {
    "name": "Afghanistan",
    "twoLetterCode": "AF",
    "threeLetterCode": "AFG",
    "numericCode": "004"
  }
]
```

This endpoint retrieves all countries.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/countries`

### Response Class (Status 200)

| Key             | Type   |
| --------------- | ------ |
| name            | string |
| twoLetterCode   | string |
| threeLetterCode | string |
| numericCode     | string |

<!--- POST --->

<!--- PUT --->

<!--- DELETE --->
