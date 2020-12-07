open System
open System.IO
open System.Collections.Generic

let input = File.ReadAllLines("data/input7.txt")

let parseBagCount (str : string) =
    let parts = str.Split(' ')
    let count = Int32.Parse(parts.[0])
    let bag = String.Join(' ', parts.[1 .. parts.Length - 2])
    (count, bag)

let parseRule (line : string) =
    match line.Split(" bags contain ") with
    | [| bag; str |] ->
        if str = "no other bags." then
            (bag, Array.empty)
        else
            (bag, str.Split(", ") |> Array.map parseBagCount)
    | _ ->
        failwithf "Invalid line: %s" line

let rules = input |> Array.map parseRule

let containmentDict =
    rules
    |> Array.collect (fun (bag, containedBags) ->
        containedBags
        |> Array.map (fun (_, otherBag) ->
            (otherBag, bag)))
    |> Array.groupBy fst
    |> Array.map (fun (bag, group) -> (bag, group |> Array.map snd))
    |> dict

let result1 =
    let containingBags = HashSet<string>()
    let rec loop bags =
        let newBags =
            bags
            |> Array.collect (fun bag ->
                match containmentDict.TryGetValue(bag) with
                | false, _ -> [||]
                | true, containingBags -> containingBags)
            |> Array.distinct
            |> Array.filter (containingBags.Contains >> not)
        if Array.isEmpty newBags then
            containingBags.Count
        else
            for bag in newBags do containingBags.Add(bag) |> ignore
            loop newBags
    loop [| "shiny gold" |]
   
let result2 =
    let ruleDict = dict rules
    let rec countInside bag =
        ruleDict.[bag]
        |> Array.sumBy (fun (count, bagInside) ->
            count * (1 + countInside bagInside))
    countInside "shiny gold"
    