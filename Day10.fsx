open System
open System.IO

let input = File.ReadAllLines("data/input10.txt") |> Array.map Int32.Parse

let result1 =
    let diffCounts =
        [| 0; yield! input; Array.max input + 3 |]
        |> Array.sort
        |> Array.pairwise
        |> Array.map (fun (a, b) -> b - a)
        |> Array.countBy id
        |> dict
    diffCounts.[1] * diffCounts.[3]

let result2 =
    let nodes = Array.sort [| 0; yield! input; Array.max input + 3 |]
    let pathCounts = Array.zeroCreate<int64> nodes.Length
    pathCounts.[0] <- 1L
    for i in 0 .. nodes.Length - 1 do
        let rec loop j =
            if j < nodes.Length && nodes.[j] <= nodes.[i] + 3 then
                if nodes.[j] > nodes.[i] then
                    pathCounts.[j] <- pathCounts.[j] + pathCounts.[i]
                loop (j + 1)
        loop (i + 1)
    Array.last pathCounts
