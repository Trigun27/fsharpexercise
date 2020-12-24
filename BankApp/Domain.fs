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
    Balance: decimal
    Owner: Owner
}

type CreditAccount = CreditAccount of Account
type RatedAccount =
    | InCredit of CreditAccount
    | Overdrawn of Account
    member this.GetField getter =
        match this with
        | InCredit (CreditAccount account) -> getter account
        | Overdrawn account -> getter account

type BankOperation = | Withdraw | Deposit
    
type Transaction = {
    Operation: string
    Amount: decimal
    Date: DateTime
}

module Transaction =
    let serialized transaction =
        sprintf "%O***%A***%M"
            transaction.Date
            transaction.Operation
            transaction.Amount
                   
    let deserialized (str: string) =
        let data = str.Split "***"
        {
            Date = data.[0] |> DateTime.Parse
            Operation = data.[1] 
            Amount = data.[2] |> Decimal.Parse
        }
        


