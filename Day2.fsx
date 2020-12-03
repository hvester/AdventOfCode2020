open System
open System.IO

let input = File.ReadAllLines("data/input2.txt")

let parsePolicyAndPassword (str : string) =
    match str.Split([|'-'; ' '; ':'|]) with
    | [| s1; s2; c; _; password |] ->
        {| Policy = (char c, (Int32.Parse s1, Int32.Parse s2)); Password = password |}
    | _ ->
        failwith "Invalid policy"

let countValidPasswords validator input =
    (0, input)
    ||> Array.fold (fun acc line ->
        let x = parsePolicyAndPassword line
        if validator x.Policy x.Password then acc + 1 else acc)

let validatePassword1 policy (password : string) =
    let c, (lb, ub) = policy
    let nOccurrences  =
        password.ToCharArray()
        |> Array.filter ((=) c)
        |> Array.length
    nOccurrences  >= lb && nOccurrences  <= ub

let result1 = countValidPasswords validatePassword1 input

let validatePassword2 policy (password : string) =
    let c, (i1, i2) = policy
    let chars = password.ToCharArray()
    (chars.[i1 - 1] = c) <> (chars.[i2 - 1] = c)

let result2 = countValidPasswords validatePassword2 input