open System
open System.IO

let input =
    File.ReadAllLines("data/input12.txt")
    |> Array.map (fun line -> (line.[0], Int32.Parse line.[1..]))

let result1 =
    let endPosition =
        ({| X = 0; Y = 0; Dx = 1; Dy = 0 |}, input)
        ||> Array.fold (fun state instruction ->
            match instruction with
            | 'N', distance -> {| state with Y = state.Y + distance |}
            | 'S', distance -> {| state with Y = state.Y - distance |}
            | 'E', distance -> {| state with X = state.X + distance |}
            | 'W', distance -> {| state with X = state.X - distance |}
            | 'L', 90
            | 'R', 270 -> {| state with Dx = -state.Dy; Dy = state.Dx |}
            | 'L', 270
            | 'R', 90 -> {| state with Dx = state.Dy; Dy = -state.Dx |}
            | 'L', 180
            | 'R', 180 -> {| state with Dx = -state.Dx; Dy = -state.Dy |}
            | 'F', distance ->
                {| state with
                    X = state.X + distance * state.Dx 
                    Y = state.Y + distance * state.Dy |}
            | _ ->
                failwithf "Invalid instruction: %A" instruction)
    abs endPosition.X + abs endPosition.Y

let result2 =
    let endPosition =
        ({| X = 0; Y = 0; Dx = 10; Dy = 1 |}, input)
        ||> Array.fold (fun state instruction ->
            match instruction with
            | 'N', distance -> {| state with Dy = state.Dy + distance |}
            | 'S', distance -> {| state with Dy = state.Dy - distance |}
            | 'E', distance -> {| state with Dx = state.Dx + distance |}
            | 'W', distance -> {| state with Dx = state.Dx - distance |}
            | 'L', 90
            | 'R', 270 -> {| state with Dx = -state.Dy; Dy = state.Dx |}
            | 'L', 270
            | 'R', 90 -> {| state with Dx = state.Dy; Dy = -state.Dx |}
            | 'L', 180
            | 'R', 180 -> {| state with Dx = -state.Dx; Dy = -state.Dy |}
            | 'F', distance ->
                {| state with
                    X = state.X + distance * state.Dx 
                    Y = state.Y + distance * state.Dy |}
            | _ ->
                failwithf "Invalid instruction: %A" instruction)
    abs endPosition.X + abs endPosition.Y
