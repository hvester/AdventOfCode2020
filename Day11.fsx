open System
open System.IO

let input = File.ReadAllLines("data/input11.txt")

let getNextSeats countOccupiedAdjacentSeats tolerableLimit (seats : string []) =
    seats
    |> Array.mapi (fun row seatRow ->
        seatRow
        |> Seq.mapi (fun col c ->
            let occupiedAdjacentSeats = countOccupiedAdjacentSeats row col seats
            match c with
            | 'L' when occupiedAdjacentSeats = 0 -> "#"
            | '#' when occupiedAdjacentSeats >= tolerableLimit -> "L"
            | _ -> string c)
        |> String.concat "")

let countSeats seats =
    seats
    |> Array.sumBy (fun seatRow ->
        seatRow |> Seq.filter ((=) '#') |> Seq.length)

let findEquilibrium countOccupiedAdjacentSeats tolerableLimit initialSeats =
    initialSeats
    |> Seq.unfold (fun seats ->
        let nextSeats = getNextSeats countOccupiedAdjacentSeats tolerableLimit seats
        Some (seats, nextSeats))
    |> Seq.pairwise
    |> Seq.pick (fun (prevSeats, seats) ->
        if prevSeats = seats then Some (countSeats seats) else None)

let directions =
    [|
        for dx in -1 .. 1 do
            for dy in -1 .. 1 do
                if dx <> 0 || dy <> 0 then
                    yield (dx, dy)
    |]

let countNeighbourSeats row col (seats : string []) =
    directions
    |> Seq.map (fun (dx, dy) -> (col + dx, row + dy))
    |> Seq.filter (fun (x, y) ->
        y >= 0 && y < seats.Length &&
        x >= 0 && x < seats.[y].Length &&
        seats.[y].[x] = '#')
    |> Seq.length

let result1 = findEquilibrium countNeighbourSeats 4 input

let countVisibleSeats row col (seats : string []) =
    directions
    |> Seq.map (fun (dx, dy) ->
        Seq.initInfinite (fun n ->
            (col + (n + 1) * dx, row + (n + 1) * dy))
        |> Seq.takeWhile (fun (x, y) ->
            y >= 0 && y < seats.Length &&
            x >= 0 && x < seats.[y].Length)
        |> Seq.tryPick (fun (x, y) ->
            let c = seats.[y].[x]
            if c = '#' || c = 'L' then Some c else None))
    |> Seq.filter ((=) (Some '#'))
    |> Seq.length

let result2 = findEquilibrium countVisibleSeats 5 input
