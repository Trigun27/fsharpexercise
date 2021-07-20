module BankApp.Auditing

open BankApp
open BankApp.Domain
open Domain.Transaction

/// Logs to the console
let printTransaction _ accountId (transaction: Transaction) =
    printfn "Account %O: %A of %M" accountId transaction.Operation transaction.Amount


// Logs to both console and file system
// Logs to both console and file system
let composedLogger = 
    let loggers =
        [ FileRepository.writeTransaction
          printTransaction ]
    fun accountId owner message ->
        loggers
        |> List.iter(fun logger -> logger accountId owner message)
