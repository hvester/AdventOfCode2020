open System
open System.IO

let input = File.ReadAllLines("data/input6.txt")

let groups =
    [|
        let currentGroup = ResizeArray<_>()
        for line in input do
            if String.IsNullOrWhiteSpace line then
                yield Seq.toArray currentGroup
                currentGroup.Clear()
            else
                currentGroup.Add(line)
        yield Seq.toArray currentGroup
    |]

let result1 =
    groups
    |> Array.sumBy (fun group ->
        group
        |> Array.collect (fun answers ->
            answers.ToCharArray())
        |> Array.distinct
        |> Array.length)

let result2 =
    groups
    |> Array.sumBy (fun group ->
        let answers =
            group
            |> Array.map (fun s ->
                Set.ofArray (s.ToCharArray()))
        Set.intersectMany answers
        |> Set.count)
