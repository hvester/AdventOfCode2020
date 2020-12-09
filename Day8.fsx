open System
open System.IO
open System.Collections.Generic

type Instruction = { Op : string; Value : int }
type ExecutionState = { Ptr : int; Acc : int }

let instructions =
    File.ReadAllLines("data/input8.txt")
    |> Array.map (fun str ->
        let sign = if str.[4] = '+' then 1 else -1
        let absValue = Int32.Parse(str.[5..])
        { Op = str.[..2]; Value = sign * absValue })

let getNextState state instruction =
    match instruction.Op, instruction.Value with
    | "acc", value ->
        { Ptr = state.Ptr + 1; Acc = state.Acc + value }
    | "jmp", value ->
        { state with Ptr = state.Ptr + value }
    | "nop", _ ->
        { state with Ptr = state.Ptr + 1 }
    | _ ->
        failwithf "Invalid instruction: %A" instruction    

let runProgram (instructions : Instruction []) =
    let rec loop visited state =
        if state.Ptr >= 0 && state.Ptr < instructions.Length then
            let nextState = getNextState state instructions.[state.Ptr]
            if Set.contains nextState.Ptr visited then
                (false, state.Acc)
            else
                loop (Set.add state.Ptr visited) nextState
        
        elif state.Ptr = instructions.Length then
            (true, state.Acc)

        else
            (false, state.Acc)

    loop Set.empty { Ptr = 0; Acc = 0 }
    

let result1 = runProgram instructions |> snd

let result2 =
    seq {
        for i in 0 .. instructions.Length - 1 do
            let instruction = instructions.[i]
            if instruction.Op <> "acc" then
                let fixedInstruction =
                    match instruction.Op with
                    | "jmp" -> { instruction with Op = "nop" }
                    | "nop" -> { instruction with Op = "jmp" }
                    | _ -> failwith "Invalid op"
                instructions
                |> Array.mapi (fun j originalInstruction ->
                    if i = j then fixedInstruction else originalInstruction)
    }
    |> Seq.pick (fun fixedInstructions ->
        match runProgram fixedInstructions with
        | true, value -> Some value
        | false, _ -> None)
