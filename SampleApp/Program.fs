module Bank

open System
open System.IO
open Domain



let length inputs =
    Seq.fold
        (fun state tail ->
            let newState = state + 1
            printfn "Current %d input is %A" state tail
            newState )
        0
        inputs
  
let max inputs =
    //Seq.fold (fun state input -> if state > input then state else input) 0 input
    (0, inputs) ||> Seq.fold
                        (fun state input -> if state> input then state else input)
  
let tst =
    let seq = [-10..-1] |> List.ofSeq
    max seq
    
let test2 =
    let seq = [-10..-1] |> List.ofSeq
    Seq.max seq
    
let ceTest =
    let lines =
        seq {
            use sr = new StreamReader(@"book.txt")
            while (not sr.EndOfStream) do
                yield sr.ReadLine()
        }

    let result = (0, lines) ||> Seq.fold (fun total line -> total + line.Length)
    Console.WriteLine("Length is {0}", result)


type Rule = string -> bool * string

let rules : Rule list =
    [
        fun text -> (text.Split ' ').Length = 3, "Must be 3 words"
        fun text -> text.Length <= 30, "Max length 30"
        fun text -> text
                    |> Seq.filter Char.IsLetter
                    |> Seq.forall Char.IsUpper, "All letters must be caps"
    ]
    
let buildValidator (rules: Rule list) =
    rules
    |> List.reduce( fun firstRule secondRule ->
        fun word ->
            let passed, error = firstRule word
            if passed then
                let passed, error = secondRule word
                if passed then true, "" else false, error
            else false, error)
    
let validate = buildValidator rules
let word = "HELLO FROM F#"
    
let testValidate =
    validate word
    

    
[<EntryPoint>]
let main _ =
    let test3 = validate word
    
    0