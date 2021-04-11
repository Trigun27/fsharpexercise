module MyAsync

open System.ComponentModel.DataAnnotations
open System.Threading
open BenchmarkDotNet.Attributes


let random = System.Random()
let pickANumberAsync = async { return random.Next(10) }
let createFiftyNumbers =
    let workflows = [ for i in 1 .. 50 -> pickANumberAsync ]
    async {
    let! numbers = workflows |> Async.Parallel
    printfn "Total is %d" (numbers |> Array.sum) }

createFiftyNumbers |> Async.Start

let urls =
    [|
        "http://www.fsharp.org"
        "http://microsoft.com"
        "http://fsharpforfunandprofit.com"
    |]

let downloadData url = async {
    use wc = new System.Net.WebClient()
    printfn "Downloading data on thread %d" Thread.CurrentThread.ManagedThreadId
    let! data = wc.AsyncDownloadData(System.Uri url)
    return data.Length
}

let downloadDataTask url = async {
    use wc = new System.Net.WebClient()
    printfn "Downloading data on thread %d" Thread.CurrentThread.ManagedThreadId
    let! data = wc.DownloadDataTaskAsync(System.Uri url) |> Async.AwaitTask
    return data.Length
}

let downloadedBytes =
    urls
    |> Array.map downloadData
    |> Async.Parallel
    |> Async.RunSynchronously
    
printfn "You downloaded %d characters" (Array.sum downloadedBytes)

let downloadedBytesTask =
    urls
    |> Array.map downloadData
    |> Async.Parallel
    |> Async.StartAsTask
   
printfn "You downloaded %d characters" (Array.sum downloadedBytesTask.Result)
