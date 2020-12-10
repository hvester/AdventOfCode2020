open System
open System.IO

let input = File.ReadAllLines("data/input9.txt") |> Array.map Int64.Parse

let result1 =
    input
    |> Array.windowed 26
    |> Array.pick (fun arr ->
        let n = Array.last arr
        let xs = arr.[..24]
        seq {
            for i, x in Array.indexed xs do
                for y in xs.[i + 1 ..] do
                    yield x + y
        }
        |> Seq.contains n
        |> function
            | true -> None
            | false -> Some n)

let tryFindContiguousSet startIndex =
    let rec loop acc j =
        let sum = acc + input.[j]
        if sum = result1 then
            Some j
        elif sum > result1 then
            None
        else
            loop sum (j + 1)
    loop 0L startIndex

let result2 =
    let i, j =
        seq { 0 .. input.Length - 1 }
        |> Seq.pick (fun i ->
            tryFindContiguousSet i
            |> Option.map (fun j -> (i, j)))

    Array.min input.[i .. j] + Array.max input.[i .. j]
    