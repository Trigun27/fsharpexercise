module BankApp.Program

open BankApp.Domain
open Operations
open System
open BankApp.FileRepository
open System.IO


let withdrawWithAudit amount (CreditAccount account as creditAccount) =
    auditAs "withdraw" Auditing.composedLogger withdraw amount creditAccount account.Id account.Owner
    
let depositWithAudit amount (ratedAccount:RatedAccount) =
    let accountId = ratedAccount.GetField (fun a -> a.Id)
    let owner = ratedAccount.GetField(fun a -> a.Owner)
    auditAs "deposit" Auditing.composedLogger deposit amount ratedAccount accountId owner
let tryLoadAccountFromDisk = tryFindTransactionsOnDisk >> Option.map loadAccount


type Command =
    | AccountCommand of BankOperation
    | Exit
   
[<AutoOpen>]
module CommandParsing =
    let tryParse c : Command option =
        match c with
        | 'd' -> Some (AccountCommand Deposit)
        | 'w' -> Some (AccountCommand Withdraw) 
        | 'x' -> Some Exit
        | _ -> None
        
        
    let tryGetBankCommand cmd=
        match cmd with
        | Exit -> None
        | AccountCommand cmd -> Some cmd
    
[<AutoOpen>]
module UserInput =
    let commands = seq {
        while true do
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
            Console.WriteLine() }
    
    let getAmount (command: BankOperation) =
        let captureAmount _ =
            Console.Write "Enter Amount: "
            Console.ReadLine() |> Decimal.TryParse
        Seq.initInfinite captureAmount
        |> Seq.choose(fun amount ->
            match amount with
            | true, amount when amount <= 0M -> None
            | false, _ -> None
            | true, amount -> Some(command, amount))
        |> Seq.head

let processCommand account (command, amount) =
    printfn ""
    let account =
        match command with
        | Deposit -> account |> depositWithAudit amount 
        | Withdraw ->
            match account with
            | InCredit account -> account |> withdrawWithAudit amount
            | Overdrawn _ ->
                printfn "You cannot withdraw funds as your account is overdrawn!"
                account
    printfn "Current balance is £%M" (account.GetField(fun a -> a.Balance))
    match account with
    | InCredit _ -> ()
    | Overdrawn _ -> printfn "Your account is overdrawn!!"
    account
           
[<EntryPoint>]
let main argv =
    let openingAccount =
        Console.Write "Please enter your name: "
        let owner = Console.ReadLine()
        owner
        |> tryLoadAccountFromDisk
        |> defaultArg <|
            InCredit(CreditAccount {Id = Guid.NewGuid()
                                    Balance = 0M
                                    Owner = {Name = owner}
                                    Currency = RUB})
        
    printfn "Current balance is £%M" (openingAccount.GetField(fun a -> a.Balance))

    

            
    let closingAccount =
        commands
        |> Seq.choose tryParse
        |> Seq.takeWhile ((<>) Exit)
        |> Seq.choose tryGetBankCommand
        |> Seq.map getAmount
        |> Seq.fold processCommand openingAccount
    
    printfn ""
    printfn $"Closing Balance:\r\n %A{closingAccount}"
    Console.ReadKey() |> ignore            


    0