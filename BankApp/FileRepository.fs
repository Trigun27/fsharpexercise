module BankApp.FileRepository

open System
open System.IO
open BankApp.Domain
open Domain.Transaction

let private accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path
    
let tryFindAccountFolder owner =    
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner)
    let l = folders |> Seq.toList
    match l with
    | [] -> None
    | head::tail -> Some (DirectoryInfo(head).Name)
        
let private buildPath(owner, accountId:Guid) = sprintf @"%s\%s_%O" accountsPath owner accountId

/// Logs to the file system
let writeTransaction accountId owner transaction =
    let path = buildPath(owner, accountId)    
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (transaction.Date.ToFileTimeUtc())
    let line = sprintf "%O***%A***%M" transaction.Date transaction.Operation transaction.Amount
    File.WriteAllText(filePath, line)
    
let readTransaction (name:string) =
    let owner, accountId =
            let parts = name.Split '_'
            parts.[0], Guid.Parse parts.[1]
        
    owner, accountId, buildPath(owner, accountId)
                    |> Directory.EnumerateFiles
                    |> Seq.map (File.ReadAllText >> deserialized)
    
let tryFindTransactionsOnDisk owner =
    let nameFull = tryFindAccountFolder owner
    match nameFull with
    | Some name -> Some (readTransaction name)
    | None -> None
    //owner, Guid.NewGuid(), Seq.empty
        

