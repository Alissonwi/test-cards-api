# Backend technical test
Tecnology: C# / .Net Core

## Problem
Generate two API's, one for saving the card of a costumer and other to validate the token of this card. With a algorith to generate the token.

### API that receive customer card and save it on the db

Request information:
```
CustomerId - int
Card Number – long - max 16 characters
CVV – int - max 5 characters
```

Response information:

```
Registration date – Date now in UTC
Token – long – (Size of token may vary)
CardId - int
```

### API that validate that token based on data provided in the request

Request information:
```
CustomerId – int
CardId - int
Token – long – (Size of token may vary)
CVV – int - max 5 characters
```
Response information:
```
Validated – bool
```
### Algorithm

The algorithm used to create the token is:

```
a) Get last 4 digits of card
b) Apply algorithm described below in Problem #1 and the number of rotations would be the CVV number.
```

The algorithm to validate the token

1. If creation date of token is more than 30 min, return as not valid
2. Use the cardid to locate the card number
3. If customer is not the owner of the card, return as not valid
4. Print the card number in the console
5. Process the algorithm described above in creation process to create the token again and
compare with the token received in the request, if match return true otherwise false.

### Problem

The operation called right circular rotation on an array of integers consists of moving the last array element to the first position and shifting all remaining elements right one. Given an array of integers, perform the rotation operation a number of times.
For each array, perform a number of right circular rotations and return this array.
For example, array a = [3, 4, 5], number of rotations k = 2.
First we perform the two rotations:
```
[3, 4, 5] => [5, 3, 4] => [4, 5, 3]
The result of that would be [4, 5, 3]
```

### Explanation

Given the array [1, 2, 3] after the first rotation, the array becomes [3, 1, 2].
After the second (and final) rotation, the array becomes [2, 3, 1].

# Begin with the project
Clone the project

```
git clone https://github.com/Alissonwi/test-cards-api.git
```

It is important to check if all the librarys is installed in your application.

Set for the projects Cards.Api and Token.Api to start together.

After starting the project you can use swagger, postman or any other way to send a POST for the API's.
