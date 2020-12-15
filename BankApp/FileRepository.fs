module BankApp.FileRepository

open System
open System.IO
open BankApp.Domain
open Domain.Transaction

let private accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path
    
let private findAccountFolder owner =    
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner)
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name
        
let private buildPath(owner, accountId:Guid) = sprintf @"%s\%s_%O" accountsPath owner accountId

/// Logs to the file system
let writeTransaction accountId owner transaction =
    let path = buildPath(owner, accountId)    
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (transaction.Date.ToFileTimeUtc())
    let line = sprintf "%O***%s***%M***%b" transaction.Date transaction.Operation transaction.Amount transaction.Accepted
    File.WriteAllText(filePath, line)
    
    
let findTransactionsOnDisk owner =
    let nameFull = findAccountFolder owner
    if nameFull = "" then
        owner, Guid.NewGuid(), Seq.empty
    else
        let owner, accountId =
            let parts = nameFull.Split '_'
            parts.[0], Guid.Parse parts.[1]
        
        owner, accountId, buildPath(owner, accountId)
                        |> Directory.EnumerateFiles
                        |> Seq.map (File.ReadAllText >> deserialized)

