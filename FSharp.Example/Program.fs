open System
open OutlineAlgorithm.Interop

[<EntryPoint>]
let main argv =

    /// Input outline headings
    let input1 = ["H1"; "H2"; "H4"; "H3"; "H5"; "H1"; "H4"; "H3"]
    let input2 = ["H1"; "H2"; "P"; "H4"; "H3"; "P"; "H5"; "P"; "H1"; "H4"; "P"; "H3"]
    let input3 = ["H1"; "P"; "H2"; "P"; "H2"; "P"; "H3"]
    let input4 = ["H1";"P";"H3";"P";"H3";"H2";"P"; "P2";"H3"]
    let input5 = ["H1"; "P"; "H2"; "P"; "H2"]
    let input6 = ["H1"; "P"; "H2"; "P"; "H2"; "UL"; "LI"; "LI2"; "LI2"; "LI"; "LI2";"LI3"; "P"]
    let input7 = ["P"; "P";  "P"]
    let input = input7

    /// Rank function that assigns numeric depth based on the heading level
    let getRank (element: string) =
        match element with
        | "H1" -> 1
        | "H2" -> 2
        | "H3" -> 3
        | "H4" -> 4
        | "H5" -> 5
        | "H6" -> 6
        | "P"  -> 1
        | "P2"  -> 2
        | "UL" -> 1
        | "LI" -> 2
        | "LI2" -> 3
        | "LI3" -> 4
        | _ -> failwith "undefined"

    /// Layer function
    let getLayer(element: string) =
        match element with
        | "H1" | "H2" | "H3" | "H4" | "H5" | "H6" -> 0
        | "P" | "P2"  | "UL" | "LI" | "LI2"| "LI3" -> 1
        | _ -> failwith "undefined"

    printfn "%A" input

    /// Step 1: Convert input sequence to tokens with parentheses
    let tree = InteropFSharp.CreateTreeWithRanksAndLayers input getRank getLayer true

    // Step 2: Traverse the tree using depth-first strategy
    printfn "\n=== Depth-First Traversal ==="
    InteropFSharp.depthFirstTraversal tree
        (fun x -> printfn "Entering: %s" x)
        (fun x -> printfn "Leaving : %s" x)

    // Step 3: Traverse the tree and print outline indices (e.g., [1; 2; 1])
    printfn "\n=== Traverse with Outline Index ==="
    InteropFSharp.traverseWithOutlineIndex tree
        (fun x idx -> printfn "Label: %s, Index: %A" x idx)

    0   