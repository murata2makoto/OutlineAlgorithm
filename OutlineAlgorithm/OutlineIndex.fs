module OutlineAlgorithm.OutlineIndex

open TreeAndHedge

let rec visitHedge (hedge: Hedge<'a>) (outlineIndex: List<int>) (action: 'a -> List<int> -> unit): unit =
    hedge 
    |> Seq.iteri 
        (fun index tree ->
            visitTree tree ((index+1)::outlineIndex) action)

and visitTree (tree: Tree<'a>) (outlineIndex: List<int>) (action: 'a -> List<int> -> unit): unit = 
    match tree with 
    | Node (None, hedge) ->
        visitHedge hedge outlineIndex action
    | Node (Some(label), hedge) ->
        action label outlineIndex
        visitHedge hedge outlineIndex action

(*
let generateOutlineIndices (hedge: Hedge<'a>) =
    let dict = new System.Collections.Generic.Dictionary<'a, List<int>>()
    let addToDictionary label outlineIndex =
        dict.[label] <- outlineIndex
    visitHedge hedge [] addToDictionary
    dict
*)
