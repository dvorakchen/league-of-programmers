## used status code
* 200
* 201 when create success
* 202 when create fault
* 400
* 404 resource not exist, for example, if a user not exist and client want to request it.
* 429

## Antiforgery
middleware Antiforgery will send the antiforgery token when client-side request index.html