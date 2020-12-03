open System.IO

let input = File.ReadAllLines("data/input3.txt")

let countTrees (slope : string []) dx dy =
    let nColumns = slope.[0].Length
    let rec loop acc x y =
        if y >= slope.Length then
            acc
        else
            let hitTree = slope.[y].[x % nColumns] = '#'
            loop (if hitTree then acc + 1 else acc) (x + dx) (y + dy)
    loop 0 0 0

let result1 = countTrees input 3 1

let result2 =
    let hits =
        [| (1, 1); (3, 1); (5, 1); (7, 1); (1, 2) |]
        |> Array.map (fun (dx, dy) -> countTrees input dx dy |> int64)
    Array.fold (*) 1L hits
