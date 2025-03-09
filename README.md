# TechChallenge.Services.InvoiceService

For this project to work create the folder c:\invoices\sqlite. For the following command dotnet ef tools are required.\
See https://learn.microsoft.com/en-us/ef/core/cli/dotnet on how to install.

From a Powershell terminal from the TechChallenge.Common.DTO Subfolder run following command: 
```
dotnet ef database update
```

And then run with "http" or as command line. Docker will require changes to the InvoiceContextDesignTimeFactory (currently path is hard coded) and a mounted Volume

#Steps

Create an invoice with the /invoice/create endpoint.
Then upload a document with the /invoice/upload endpoint
Finally call the /invoice/evaluate endpoint to generate an evaluation. 


