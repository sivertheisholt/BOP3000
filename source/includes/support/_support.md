# Support

<!--- GET --->

<!--- POST --->

## POST Create new ticket

```javascript
var myHeaders = new Headers();
myHeaders.append("Content-Type", "application/json");

var raw = JSON.stringify({
  subject: "HELP",
  email: "member@test.com",
  name: "Member",
  description: "I cant log in to my user",
});

var requestOptions = {
  method: "POST",
  headers: myHeaders,
  body: raw,
  redirect: "follow",
};

fetch(
  "https://bop3000.azurewebsites.net/api/support/create-ticket",
  requestOptions
)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

> The above command returns a 200 status code

This endpoint will create a new ticket in the ticket system at freshdesk

### HTTP Request

`POST https://bop3000.azurewebsites.net/api/support/create-ticket`

### Body Schema

| Key         | Type   |
| ----------- | ------ |
| subject     | string |
| email       | string |
| name        | string |
| description | string |

<!--- PUT --->

<!--- PATCH --->

<!--- DELETE --->
