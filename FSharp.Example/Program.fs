open System
open OutlineAlgorithm.Interop

[<EntryPoint>]
let main argv =
    // Input sequence
    let input = ["H1"; "H2"; "H4"; "H3"]

    // Rank function
    let getRank (element: string) =
        match element with
        | "H1" -> 1
        | "H2" -> 2
        | "H3" -> 3
        | "H4" -> 4
        | _ -> 0

    // Convert F# function to System.Func
    let rankFunc = Func<string, int>(getRank)

    // Generate token and parenthesis sequence using Interop
    let tokenSeq = Interop.CreateTokenOrParenthesisSeq(input, rankFunc)

    // Parse to tree using Interop
    let trees = Interop.ParseToTree(tokenSeq)

    // Print each tree in the sequence
    let rec printTree (tree: InteropTree<string>) (level: int) =
        match tree.Value with
        | None ->
            printfn "%s@" (String.replicate (level * 2) " ")
        | Some(v)  ->
            printfn "%s%s" (String.replicate (level * 2) " ") v
        for child in tree.Children do
            printTree child (level + 1)

    for tree in trees do
        printTree tree 0

    0 // Return an integer exit code