module Bank

open System
open System.IO
open System.Threading
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Jobs
open Domain
open MyAsync
//
//let parseInt str =
//    match str with
//    | "-1" -> Some -1
//    | "0" -> Some 0
//    | "1" -> Some 1
//    | "2" -> Some 2
//    // etc
//    | _ -> None
//
//type OrderQty = OrderQty of int
//
//let toOrderQty qty =
//    if qty >= 1 then
//        Some (OrderQty qty)
//    else
//        // only positive numbers allowed
//        None
//        
//let parseOrderQty str =
//    let s = parseInt str
//    Option.bind toOrderQty s
//
//let length inputs =
//    Seq.fold
//        (fun state tail ->
//            let newState = state + 1
//            printfn "Current %d input is %A" state tail
//            newState )
//        0
//        inputs
//  
//let max inputs =
//    //Seq.fold (fun state input -> if state > input then state else input) 0 input
//    (0, inputs) ||> Seq.fold
//                        (fun state input -> if state> input then state else input)
//  
//let tst =
//    let seq = [-10..-1] |> List.ofSeq
//    max seq
//    
//let test2 =
//    let seq = [-10..-1] |> List.ofSeq
//    Seq.max seq
//    
//let ceTest =
//    let lines =
//        seq {
//            use sr = new StreamReader(@"book.txt")
//            while (not sr.EndOfStream) do
//                yield sr.ReadLine()
//        }
//
//    let result = (0, lines) ||> Seq.fold (fun total line -> total + line.Length)
//    Console.WriteLine("Length is {0}", result)
//
//
//type Rule = string -> bool * string
//
//let rules : Rule list =
//    [
//        fun text -> (text.Split ' ').Length = 3, "Must be 3 words"
//        fun text -> text.Length <= 30, "Max length 30"
//        fun text -> text
//                    |> Seq.filter Char.IsLetter
//                    |> Seq.forall Char.IsUpper, "All letters must be caps"
//    ]
//    
//let buildValidator (rules: Rule list) =
//    rules
//    |> List.reduce( fun firstRule secondRule ->
//        fun word ->
//            let passed, error = firstRule word
//            if passed then
//                let passed, error = secondRule word
//                if passed then true, "" else false, error
//            else false, error)
//    
//let validate = buildValidator rules
//let word = "HELLO FROM F#"
//    
//let testValidate =
//    validate word
//    
//
//[|32; 124; 62; 32; 40; 102; 117; 110; 32; 120; 45; 62; 32; 112; 114; 105; 110;
//  116; 102; 110; 32; 34; 37; 65; 37; 115; 34; 32; 120; 32; 60; 124; 32; 83; 121;
//  115; 116; 101; 109; 46; 83; 116; 114; 105; 110; 103; 46; 74; 111; 105; 110; 40;
//  34; 34; 44; 32; 65; 114; 114; 97; 121; 46; 109; 97; 112; 32; 99; 104; 97; 114;
//  32; 120; 41; 41|] |> (fun x-> printfn "%A%s" x <| System.String.Join("", Array.map char x))
//
//let workThenWait() = 
//  Thread.Sleep(1000)
//  printfn "work done"
//  async { do! Async.Sleep(1000) }
//
//let demo() = 
//  let work = workThenWait() |> Async.StartAsTask
//  printfn "started"
//  work.Wait()
//  printfn "completed"

open QuickSort

[<EntryPoint>]
let main _ =
    
    test
    
    0