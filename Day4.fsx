open System
open System.IO

let input = File.ReadAllText("data/input4.txt").Split("\n\n")

let parsePassportData (input : string []) =
    input
    |> Array.map (fun line ->
        line.Split([|' '; '\n'|])
        |> Array.filter (String.IsNullOrWhiteSpace >> not)
        |> Array.map (fun parts ->
            match parts.Split(':') with
            | [| s1; s2 |] -> (s1.Trim(), s2.Trim())
            | xs -> failwithf "Invalid input: %s" line))

let countIf predicate xs = Array.sumBy (fun x -> if predicate x then 1 else 0) xs

let hasAllRequiredFields fields =
    [| "byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid" |]
    |> Array.forall (fun requiredField ->
        fields |> Array.map fst |> Array.contains requiredField)

let passports = parsePassportData input

let result1 = countIf hasAllRequiredFields passports

let numberBetween lb ub (str : string) =
    match Int32.TryParse(str) with
    | false, _ -> false
    | true, v -> lb <= v && v <= ub

let validateFieldValues fields =
    fields
    |> Array.forall (function
        | ("byr", value) ->
             // (Birth Year) - four digits; at least 1920 and at most 2002.
            numberBetween 1920 2002 value
        | ("iyr", value) ->
            // (Issue Year) - four digits; at least 2010 and at most 2020.
            numberBetween 2010 2020 value
        | ("eyr", value) ->
            // (Expiration Year) - four digits; at least 2020 and at most 2030.
            numberBetween 2020 2030 value
        | ("hgt", value) ->
            // (Height) - a number followed by either cm or in:
            //    If cm, the number must be at least 150 and at most 193.
            //    If in, the number must be at least 59 and at most 76.
            match value.[value.Length - 2 .. value.Length - 1] with
            | "cm" -> numberBetween 150 193 value.[ .. value.Length - 3]
            | "in" -> numberBetween 59 76 value.[ .. value.Length - 3]
            | _ -> false
        | ("hcl", value) ->
            // (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            value.Length = 7 &&
            value.[0] = '#' &&
            (value.[1..] |> Seq.forall (fun c -> List.contains c ([ '0' .. '9' ] @ [ 'a' .. 'f' ])))
        | ("ecl", value) ->
            // (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            [ "amb"; "blu"; "brn"; "gry"; "grn"; "hzl"; "oth" ]
            |> List.contains value
        | ("pid", value) ->
            // (Passport ID) - a nine-digit number, including leading zeroes.
            value.Length = 9 &&
            (value |> Seq.forall (fun c -> List.contains c [ '0' .. '9' ]))
        | _ ->
            //cid (Country ID) - ignored, missing or not.
            true)

let isValidPassport fields =
    hasAllRequiredFields fields && validateFieldValues fields

let result2 = countIf isValidPassport passports

