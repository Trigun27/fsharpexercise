namespace BankApp.Domain

open System

type Currency =
    | RUB
    | DOL
    | EURO

type Owner = { Name: string }

type Account = {
    Id: Guid
    Currency: Currency
    Amount: decimal
    Owner: Owner
}

type Transaction = {
    Operation: string
    Amount: decimal
    Date: DateTime
    Accepted: bool
}

module Transaction =
    let serialized transaction =
        sprintf "%O***%s***%M***%b"
            transaction.Date
            transaction.Operation
            transaction.Amount
            transaction.Accepted
            
    let deserialized (str: string) =
        let data = str.Split "***"
        {
            Date = data.[0] |> DateTime.Parse
            Operation = data.[1]
            Amount = data.[2] |> Decimal.Parse
            Accepted = data.[3] |> Boolean.Parse  
        }
        


