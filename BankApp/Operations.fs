module BankApp.Operations

open System
open BankApp.Domain

let classifyAccount (account: Account) =
    if account.Balance>= 0M then (InCredit(CreditAccount account))
    else Overdrawn account

let withdraw amount (CreditAccount account) =
    { account with Balance = account.Balance - amount }
    |> classifyAccount
    
let withdrawSafe amount ratedAccount =
    match ratedAccount with
    | InCredit account -> account |> withdraw amount
    | Overdrawn _ ->
        printfn "your account is overdrawn - withdraw rejected"
        ratedAccount

let deposit amount account =
    let account =
        match account with
        | InCredit (CreditAccount account) -> account
        | Overdrawn account -> account
    { account with Balance = account.Balance + amount }
    |> classifyAccount
    

let auditAs operationName audit operation amount account  accountId owner =
    let updatedAccount = operation amount account
    let transaction = { Operation = operationName; Amount = amount; Date = DateTime.UtcNow }
    audit accountId owner.Name transaction
    updatedAccount
 
 
let tryParseSerializedOperation operation =
    match operation with
    | "withdraw" -> Some Withdraw
    | "deposit" -> Some Deposit
    | _ -> None
     
let loadAccount (owner, accountId, transactions) =
    let openingAccount = classifyAccount { Id = accountId; Balance = 0M; Owner = { Name = owner }; Currency = RUB }

    printfn "%A" transactions
    
    transactions
    |> Seq.sortBy(fun txn -> txn.Date)
    |> Seq.fold(fun account txn ->
        let operation = tryParseSerializedOperation txn.Operation
        match operation, account with
        | Some Deposit, _ -> account |> deposit txn.Amount
        | Some Withdraw, InCredit account -> account |> withdraw txn.Amount
        | Some Withdraw, Overdrawn _ -> account
        | None, _ -> account) openingAccount
                        
   

    


