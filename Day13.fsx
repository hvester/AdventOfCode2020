open System
open System.IO

let input = File.ReadAllLines("data/input13.txt")

let departureTime = Int32.Parse input.[0]
let buses =
    input.[1].Split(',')
    |> Array.map (function
        | "x" -> None
        | s -> Some (Int32.Parse s))

let result1 =
    let busId, waitingTime = 
        buses
        |> Array.choose id
        |> Array.map (fun busId ->
            let waitingTime =
                match departureTime % busId with
                | 0 -> 0
                | t -> busId - t
            (busId, waitingTime))
        |> Array.minBy snd
    busId * waitingTime