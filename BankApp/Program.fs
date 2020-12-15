module BankApp.Programm

open BankApp.Domain
open Operations
open System
open BankApp.FileRepository
open System.IO
        
let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit
//let loadAccountFromDisk = FileRepository.findTransactionsOnDisk >> Operations.loadAccount

let isValidCommand cmd = [ 'd'; 'w'; 'x' ] |> List.contains cmd
    
let isStopCommand = (=) 'x'
    
let commands = seq {
    while true do
        Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
        yield Console.ReadKey().KeyChar
        Console.WriteLine() }

let getAmount command =
    Console.WriteLine()
    Console.Write "Enter Amount: "
    command, Console.ReadLine() |> Decimal.Parse

let loadAccountFromDisk owner =
    let owner, accountId, transactions = findTransactionsOnDisk owner
    loadAccount owner accountId transactions
    
   
[<EntryPoint>]
let main argv =
    let openingAccount =
        Console.Write "Please enter your name: "
        Console.ReadLine() |> loadAccountFromDisk
    
    printfn "Current balance is £%M" openingAccount.Amount

    
    let processCommand account (command, amount) =
        printfn ""
        let account =
            if command = 'd' then account |> depositWithAudit amount
            else account |> withdrawWithAudit amount
        printfn "Current balance is £%M" account.Amount
        account

    let closingAccount =
        commands
        |> Seq.filter isValidCommand
        |> Seq.takeWhile (not << isStopCommand)
        |> Seq.map getAmount
        |> Seq.fold processCommand openingAccount
    
    printfn ""
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0