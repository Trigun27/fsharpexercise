module QuickSort

open System
open System.Diagnostics
open System.Threading.Tasks

let genRandomNumbers count =
    let rnd = System.Random()
    List.init count (fun _ -> rnd.Next ())

let rec quickSort aList =
    match aList with
    | [] -> []
    | firstElement :: restOfList ->
        let smaller, larger =
            List.partition (fun number -> number < firstElement) restOfList
        quickSort smaller @ (firstElement :: quickSort larger)

let rec quickSortDepth depth aList =
    match aList with
    | [] -> []
    | firstElement :: restOfList ->
        let smaller, larger =
            List.partition (fun number -> number < firstElement) restOfList
        if depth < 0 then
            let left = quickSortDepth depth smaller
            let right = quickSortDepth depth larger
            left @ (firstElement :: right)
        else
            let left = Task.Run( fun() -> quickSortDepth (depth - 1) smaller)
            let right = Task.Run (fun() -> quickSortDepth (depth - 1) larger)
            left.Result @ (firstElement :: right.Result)
            
           
let test =
    let aList = genRandomNumbers  10000000    
    quickSort aList

let test2 =
    let aList = genRandomNumbers  10000000        
    quickSortDepth 2 aList

