namespace OutlineAlgorithm.Interop

open System
open System.Collections.Generic
open OutlineAlgorithm.TokenOrParenthesis
open OutlineAlgorithm.TreeAndHedge
open OutlineAlgorithm.CreateHedge
open OutlineAlgorithm.CreateTokenOrParenthesisSeq

/// <summary>
/// Simple tree structure used across F# and C# APIs.
/// </summary>
type InteropTree<'a>(value: 'a option, children: seq<InteropTree<'a>>) =
    /// <summary>Gets the optional value of this node.</summary>
    member _.Value = value

    /// <summary>Gets the child nodes of this tree node.</summary>
    member _.Children = children

    /// <summary>Gets the contained value if present, otherwise returns the default value of type 'a.</summary>
    member _.ValueOrDefault =
        match value with
        | Some v -> v
        | None -> Unchecked.defaultof<'a>

    /// <summary>Indicates whether this node has a value (i.e., is not None).</summary>
    member _.HasValue = value.IsSome

/// <summary>
/// Shared implementation of tree traversal and conversion logic used by both F# and C# interfaces.
/// </summary>
module internal InteropTraversalImpl =

    /// <summary>
    /// Performs a depth-first traversal of the tree.
    /// </summary>
    /// <param name="tree">The root of the InteropTree to traverse.</param>
    /// <param name="onEnter">Function to execute when entering a node with a value.</param>
    /// <param name="onExit">Function to execute when exiting a node with a value.</param>
    let rec depthFirst (tree: InteropTree<'a>) (onEnter: 'a -> unit) (onExit: 'a -> unit) =
        match tree.Value with
        | Some v -> onEnter v
        | None -> ()
        tree.Children |> Seq.iter (fun child -> depthFirst child onEnter onExit)
        match tree.Value with
        | Some v -> onExit v
        | None -> ()

    /// <summary>
    /// Performs a breadth-first traversal of the tree.
    /// </summary>
    /// <param name="tree">The root of the InteropTree to traverse.</param>
    /// <param name="onEnter">Function to execute when entering a node with a value.</param>
    /// <param name="onExit">Function to execute when exiting a node with a value.</param>
    let breadthFirst (tree: InteropTree<'a>) (onEnter: 'a -> unit) (onExit: 'a -> unit) =
        let queue = Queue<InteropTree<'a> * bool>()
        queue.Enqueue((tree, false))
        while queue.Count > 0 do
            let (current, isExit) = queue.Dequeue()
            match current.Value with
            | Some v when isExit -> onExit v
            | Some v ->
                onEnter v
                queue.Enqueue((current, true))
                current.Children |> Seq.iter (fun child -> queue.Enqueue((child, false)))
            | None ->
                if not isExit then
                    current.Children |> Seq.iter (fun child -> queue.Enqueue((child, false)))

    /// <summary>
    /// Traverses the tree and provides an outline index (e.g., [1; 2; 3]) to each node with a value.
    /// </summary>
    /// <param name="tree">The InteropTree to traverse.</param>
    /// <param name="action">Function to invoke with each node value and its outline index.</param>
    /// <param name="index">The accumulated index during recursion (used internally).</param>
    let rec traverseWithOutlineIndex (tree: InteropTree<'a>) (action: 'a -> int list -> unit) (index: int list) =
        match tree.Value with
        | Some v -> action v (List.rev index)
        | None -> ()
        tree.Children |> Seq.iteri (fun i child -> traverseWithOutlineIndex child action ((i + 1) :: index))

    /// <summary>
    /// Converts a sequence of elements into a flat array of tokens and parentheses.
    /// </summary>
    /// <param name="elements">The sequence of elements to convert.</param>
    /// <param name="getRank">Function that returns the rank (nesting level) of an element.</param>
    /// <returns>An array of TokenOrParenthesis representing the structure.</returns>
    let createTokenOrParenthesisSeqFromSeq (elements: seq<'a>) (getRank: 'a -> int) : seq<TokenOrParenthesis<'a>> =
        createTokenOrParenthesisSeq elements getRank

    /// <summary>
    /// Parses a flat token and parenthesis sequence into a structured InteropTree.
    /// </summary>
    /// <param name="tokens">The token sequence to parse.</param>
    /// <returns>An InteropTree representing the hierarchical structure.</returns>
    let parseToTree (tokens: seq<TokenOrParenthesis<'a>>) =
        use en = tokens.GetEnumerator()
        let hedge = sequence2Hedge en
        let rec convertTree (tree: Tree<'a>) =
            match tree with
            | Node(value, children) ->
                InteropTree(value, children |> Seq.map convertTree)
        InteropTree(None, hedge |> Seq.map convertTree)

/// <summary>
/// F#-friendly API using curried functions and idiomatic constructs.
/// </summary>
module InteropFSharp =

    /// <summary>
    /// Performs depth-first traversal of an InteropTree. Only nodes with values are visited.  Dummy nodes are not visited.
    /// </summary>
    /// <param name="tree">The InteropTree to traverse.</param>
    /// <param name="onEnter">Function to call when entering a node with a value.</param>
    /// <param name="onExit">Function to call when exiting a node with a value.</param>
    let depthFirstTraversal tree onEnter onExit =
        InteropTraversalImpl.depthFirst tree onEnter onExit

    /// <summary>
    /// Performs breadth-first traversal of an InteropTree. Only nodes with values are visited.  Dummy nodes are not visited.
    /// </summary>
    /// <param name="tree">The InteropTree to traverse.</param>
    /// <param name="onEnter">Function to call when entering a node with a value.</param>
    /// <param name="onExit">Function to call when exiting a node with a value.</param>
    let breadthFirstTraversal tree onEnter onExit =
        InteropTraversalImpl.breadthFirst tree onEnter onExit

    /// <summary>
    /// Traverses the tree and applies the function to each node with a value, passing its outline index.  Dummy nodes are not visited.
    /// </summary>
    /// <param name="tree">The InteropTree to traverse.</param>
    /// <param name="action">Function to call with each node's value and outline index (e.g., [1; 2; 3]).</param>
    let traverseWithOutlineIndex tree action =
        InteropTraversalImpl.traverseWithOutlineIndex tree action []

    /// <summary>
    /// Converts a sequence of elements and rank function into an InteropTree structure.
    /// </summary>
    /// <param name="elements">The sequence of elements to convert.</param>
    /// <param name="getRank">A function that returns the rank (nesting level) of each element.</param>
    /// <returns>An InteropTree representing the reconstructed hierarchy.</returns>
    let CreateTree elements getRank =
        InteropTraversalImpl.createTokenOrParenthesisSeqFromSeq elements getRank
        |> InteropTraversalImpl.parseToTree 

/// <summary>
/// C#-friendly API using .NET delegate types and standard collections.
/// </summary>
type InteropCSharp =

    /// <summary>
    /// Performs depth-first traversal using Action delegates.
    /// Only nodes with values are visited.  Dummy nodes are not visited.
    /// </summary>
    /// <param name="tree">The InteropTree to traverse.</param>
    /// <param name="onEnter">Action to invoke when entering a node with a value.</param>
    /// <param name="onExit">Action to invoke when exiting a node with a value.</param>
    static member DepthFirstTraversal(tree: InteropTree<'a>, onEnter: Action<'a>, onExit: Action<'a>) =
        InteropTraversalImpl.depthFirst tree (fun x -> onEnter.Invoke(x)) (fun x -> onExit.Invoke(x))

    /// <summary>
    /// Performs breadth-first traversal using Action delegates.
    /// Only nodes with values are visited.  Dummy nodes are not visited.
    /// </summary>
    /// <param name="tree">The InteropTree to traverse.</param>
    /// <param name="onEnter">Action to invoke when entering a node with a value.</param>
    /// <param name="onExit">Action to invoke when exiting a node with a value.</param>
    static member BreadthFirstTraversal(tree: InteropTree<'a>, onEnter: Action<'a>, onExit: Action<'a>) =
        InteropTraversalImpl.breadthFirst tree (fun x -> onEnter.Invoke(x)) (fun x -> onExit.Invoke(x))

    /// <summary>
    /// Traverses the tree and invokes the Action with outline index for each node with a value.  Dummy nodes are not visited.
    /// </summary>
    /// <param name="tree">The InteropTree to traverse.</param>
    /// <param name="action">Action to invoke with the node value and its outline index (e.g., [1, 2, 3]).</param>
    static member TraverseWithOutlineIndex(tree: InteropTree<'a>, action: Action<'a, IReadOnlyList<int>>) =
        let adaptedAction = fun v idx -> action.Invoke(v, idx :> IReadOnlyList<int>)
        InteropTraversalImpl.traverseWithOutlineIndex tree adaptedAction []

    /// <summary>
    /// Converts a sequence of elements and rank function into an InteropTree structure.
    /// </summary>
    /// <param name="elements">The sequence of elements to convert.</param>
    /// <param name="getRank">A Func delegate returning the nesting level (rank) of each element.</param>
    /// <returns>An InteropTree representing the reconstructed hierarchy.</returns>
    static member CreateTree(elements: IEnumerable<'a>, getRank: Func<'a, int>) =
        InteropTraversalImpl.createTokenOrParenthesisSeqFromSeq elements (fun e -> getRank.Invoke(e))
        |> InteropTraversalImpl.parseToTree 
