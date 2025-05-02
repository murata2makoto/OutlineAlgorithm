module OutlineAlgorithm.Main
open TokenOrParenthesis
open CreateTokenOrParenthesisSeq
open TreeAndHedge
open CreateHedge
      
[<EntryPoint>]
let main argv =
    let test seq1 = 
        seq1 |> Seq.iter (fun x -> printf "%A " x)
        printfn ""
        let tpSeq = createTokenOrParenthesisSeq seq1 (fun x -> x)
        for e in tpSeq do
            match e with 
            | StartParenthesis -> printf "("
            | EndParenthesis -> printf ")"
            | Token(e) -> printf "%A" e
            | DummyToken -> printf "@"
        printfn ""
        let hedge, _ = sequence2Hedge tpSeq
        for tr in hedge do
            printfn "%A" tr
        printfn ""
    seq {1} |> test 
    seq {1;1} |> test 
    seq {1;1;1} |> test 
    seq {1;2} |> test 
    seq {1;2;2} |> test 
    seq {1;2;1} |> test 
    seq {1;2;3;4;1} |> test
    seq {1;2;3;4;2} |> test  
    seq {1;2;2;1} |> test 
    seq {1;2;2;1;2} |> test 
    seq {1;2;3;1;2} |> test 
    seq {1;2;3;4;5;1;2} |> test 
    seq {1;2;3;4;3;4;3;4;3;4;1;2} |> test 
    seq {1;2;3;4;2;3;4;2;3;1} |> test 
    seq {2} |> test 
    seq {4} |> test 
    seq {1;2;4} |> test
    seq {1;2;4;3} |> test
    1