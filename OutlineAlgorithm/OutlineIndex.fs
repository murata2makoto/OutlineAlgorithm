module internal OutlineAlgorithm.OutlineIndex

open TreeAndHedge

/// <summary>
/// Recursively traverses a hedge structure and executes the specified action for each node.
/// </summary>
/// <param name="hedge">The hedge structure to traverse</param>
/// <param name="outlineIndex">Current outline index (as a reversed list)</param>
/// <param name="action">Action to execute for each node, receiving the value and outline index</param>
/// <remarks>
/// This function traverses a hedge (sequence of sibling nodes) in a tree structure,
/// calling visitTree for each node. The outline index uses 1-based numbering.
/// The index list is maintained in reverse order for efficient prepending.
/// </remarks>
let rec visitHedge (hedge: Hedge<'a>) (outlineIndex: int list) (action: 'a -> int list -> unit): unit =
    hedge 
    |> Seq.iteri 
        (fun index tree ->
            visitTree tree ((index+1)::outlineIndex) action)

/// <summary>
/// Recursively traverses a tree and executes the specified action for each node.
/// </summary>
/// <param name="tree">The tree node to traverse</param>
/// <param name="outlineIndex">Current outline index (as a reversed list)</param>
/// <param name="action">Action to execute for labels, receiving the label value and outline index</param>
/// <remarks>
/// Processes a tree node, executing the action if the node contains a label (Some case),
/// then continues processing any child hedge with visitHedge.
/// The outline index represents hierarchical numbering like chapter numbers.
/// </remarks>
and visitTree (tree: Tree<'a>) (outlineIndex: int list) (action: 'a -> int list -> unit): unit = 
    match tree with 
    | Node (None, hedge) ->
        visitHedge hedge outlineIndex action
    | Node (Some(label), hedge) ->
        action label outlineIndex
        visitHedge hedge outlineIndex action
