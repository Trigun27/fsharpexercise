module BankApp.Operations

open System
open BankApp.Domain


let withdraw (amount: decimal) (account: Account) =
    if (amount > account.Amount) then account
    else { account with Amount = account.Amount - amount}

let deposit amount (account: Account) =
    {account with Amount = account.Amount + amount}
    

let auditAs operationName audit operation amount account =
    let updatedAccount = operation amount account   
    let accountIsUnchanged = (updatedAccount = account)
    let transaction =
        let transaction = { Operation = operationName; Amount = amount; Date = DateTime.UtcNow; Accepted = true }
        if accountIsUnchanged then { transaction with Accepted = false }
        else transaction

    audit account.Id account.Owner.Name transaction
    updatedAccount
    
 
let applyTransaction account transaction =
    if transaction.Operation = "deposit" then deposit transaction.Amount account
    elif transaction.Operation = "withdraw" then withdraw transaction.Amount account
    else account
    
let loadAccount owner accountId (transactions: Transaction seq) =
    let accountZero = {Owner = {Name = owner}; Id = accountId; Amount = 0M; Currency = Currency.RUB}
    
    transactions
    |> Seq.sortBy (fun x -> x.Date)
    |> Seq.fold applyTransaction accountZero
                        
   

    


