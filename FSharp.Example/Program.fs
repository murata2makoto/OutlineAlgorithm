open System
open OutlineAlgorithm.Interop

[<EntryPoint>]
let main argv =

    /// Input outline headings
    let input = ["H1"; "H2"; "H4"; "H3"; "H5"; "H3"; "H5"]

    /// Rank function that assigns numeric depth based on the heading level
    let getRank (element: string) =
        match element with
        | "H1" -> 1
        | "H2" -> 2
        | "H3" -> 3
        | "H4" -> 4
        | "H5" -> 5
        | "H6" -> 6
        | _ -> 0

    /// Step 1: Convert input sequence to tokens with parentheses
    let tokens = InteropFSharp.createTokenOrParenthesisSeq input getRank
    printfn "=== Tokens ==="
    tokens |> Seq.iter (printfn "%A")

    /// Step 2: Parse tokens into tree structure
    let tree = InteropFSharp.parseToTree tokens

    /// Step 3: Traverse the tree using depth-first strategy
    printfn "\n=== Depth-First Traversal ==="
    InteropFSharp.depthFirstTraversal tree
        (fun x -> printfn "Entering: %s" x)
        (fun x -> printfn "Leaving : %s" x)

    /// Step 4: Traverse the tree and print outline indices (e.g., [1; 2; 1])
    printfn "\n=== Traverse with Outline Index ==="
    InteropFSharp.traverseWithOutlineIndex tree
        (fun x idx -> printfn "Label: %s, Index: %A" x idx)

    0   