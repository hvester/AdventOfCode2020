open System
open System.IO

let input =
    File.ReadAllLines("data/input1.txt")
    |> Array.map Int32.Parse

let result1 =
    seq {
        for i in 0 .. input.Length - 1 do
            for j in i + 1 .. input.Length - 1 do
                (input.[i], input.[j])
    }
    |> Seq.pick (fun (x, y) ->
        if x + y = 2020 then
            Some (x * y)
        else
            None)

let result2 =
    seq {
        for i in 0 .. input.Length - 1 do
            for j in i + 1 .. input.Length - 1 do
                for k in i + 1 .. input.Length - 1 do
                    (int64 input.[i], int64 input.[j], int64 input.[k])
    }
    |> Seq.pick (fun (x, y, z) ->
        if x + y + z = 2020L then
            Some (x * y * z)
        else
            None)
