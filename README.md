# dotnet-webapi-bank
This is a project made for a test related to WebAPIs.

### get all users

    [GET] endpoint: users/

##### example response
      
    [
        {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "type": "string",
            "name": "string",
            "balance": 0.0
        }, ....
    ]

### get a user by id

    [GET] endpoint: users/{id}
    
##### example response
    
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "type": "string",
      "name": "string",
      "balance": 0.0
    }
    
### create a user

     [POST] endpoint: users/
  
##### post model

    {
      "type": "string", // only "common" or "sales"
      "name": "string",
      "cpf": "string", // format xxx.xxx.xxx-xx
      "email": "string", // format xxx@xxx.xxx
      "password": "string"
    }

### see all transactions
      
      [GET] endpoint: transactions/

##### example response
      [
        {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "sender": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "receiver": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "createdAt": "2021-10-23T01:54:07.493Z",
          "amount": 0.0
        }, ...
      ]
      
### get transaction by id

      [GET] endpoint: transactions/{id}
      
##### example response
     [
        {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "sender": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "receiver": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "createdAt": "2021-10-23T01:54:07.493Z",
          "amount": 0.0
        }
    ]
    
### create transaction
     
     [POST] endpoint: transactions/create

##### post model

    {
      "sender": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "receiver": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "amount": 0.0
    }
    
### undo transaction

    [POST] endpoint: transactions/undotransaction/{id}
    
##### no response