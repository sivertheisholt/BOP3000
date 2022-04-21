# Images

<!--- GET --->

## GET Backgrounds for customizer

```javascript
var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer <Token>");

var requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/images/customizer_images",
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
    "backgroundUrl": "http://res.cloudinary.com/dzpzecnx5/image/upload/v1650544575/AccountBackgrounds/pexels-suzukii-xingfu-698319_brqyfl.jpg",
    "iconUrl": "http://res.cloudinary.com/dzpzecnx5/image/upload/c_scale,h_140,w_206/v1650544575/AccountBackgrounds/pexels-suzukii-xingfu-698319_brqyfl.jpg"
  },
  {
    "backgroundUrl": "http://res.cloudinary.com/dzpzecnx5/image/upload/v1650544575/AccountBackgrounds/pexels-donald-tong-55787_g8fhfo.jpg",
    "iconUrl": "http://res.cloudinary.com/dzpzecnx5/image/upload/c_scale,h_140,w_206/v1650544575/AccountBackgrounds/pexels-donald-tong-55787_g8fhfo.jpg"
  },
  {
    "backgroundUrl": "http://res.cloudinary.com/dzpzecnx5/image/upload/v1650544575/AccountBackgrounds/pexels-sebastiaan-stam-1480693_txjoxk.jpg",
    "iconUrl": "http://res.cloudinary.com/dzpzecnx5/image/upload/c_scale,h_140,w_206/v1650544575/AccountBackgrounds/pexels-sebastiaan-stam-1480693_txjoxk.jpg"
  }
]
```

This endpoint retrieves all backgrounds for the customizer.

### HTTP Request

`GET https://bop3000.azurewebsites.net/api/images/customizer_images`

### Response Class (Status 200)

| Key           | Type   |
| ------------- | ------ |
| backgroundUrl | string |
| iconUrl       | string |

<!--- POST --->

<!--- PUT --->

<!--- PATCH --->

<!--- DELETE --->
