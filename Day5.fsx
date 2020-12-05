open System.IO

let input = File.ReadAllLines("data/input5.txt")

let parseNumber oneChar (str : string) =
    Seq.fold (fun acc c -> if c = oneChar then 2 * acc + 1 else 2 * acc) 0 str

let parseSeat (str : string) =
    (parseNumber 'B' str.[..6], parseNumber 'R' str.[7..])

let getSeatId (row, column) = 8 * row + column

let seatIds = input |> Array.map (parseSeat >> getSeatId)

let result1 = Array.max seatIds

let result2 =
    seatIds
    |> Array.sort
    |> Array.pairwise
    |> Array.pick (fun (id1, id2) ->
        if id2 - id1 = 2 then Some (id1 + 1) else None)