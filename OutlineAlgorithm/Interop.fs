namespace OutlineAlgorithm.Interop

open System.Collections.Generic
open OutlineAlgorithm.TokenOrParenthesis
open OutlineAlgorithm.TreeAndHedge
open OutlineAlgorithm.CreateHedge
open OutlineAlgorithm.CreateTokenOrParenthesisSeq

type InteropTree<'a>(value: 'a option, children: seq<InteropTree<'a>>) =
    /// <summary>
    /// Gets the value of the node if it exists, or the default value of the type <typeparamref name="'a"/> if the value is None.
    /// </summary>
    /// <value>
    /// The value of the node if it exists; otherwise, the default value of type <typeparamref name="'a"/>.
    /// </value>
    member _.Value = value

    /// <summary>
    /// Gets the child nodes of the current node.
    /// </summary>
    /// <value>
    /// A sequence of child nodes.
    /// </value>
    member _.Children = children

    /// <summary>
    /// Gets the value of the node if it exists, or the default value of the type <typeparamref name="'a"/> if the value is None.
    /// </summary>
    /// <value>
    /// The value of the node if it exists; otherwise, the default value of type <typeparamref name="'a"/>.
    /// </value>
    member _.ValueOrDefault =
        match value with
        | Some v -> v
        | None -> Unchecked.defaultof<'a>

type Interop =
    /// <summary>
    /// Converts a sequence of elements into a sequence of tokens and parentheses.
    /// </summary>
    /// <param name="elements">The input sequence of elements.</param>
    /// <param name="getRank">A function to determine the rank (or level) of each element.</param>
    /// <returns>A sequence of tokens and parentheses.</returns>
    static member CreateTokenOrParenthesisSeq(elements: IEnumerable<'a>, getRank: System.Func<'a, int>) =
        createTokenOrParenthesisSeq elements (fun e -> getRank.Invoke(e)) // Pass seq<'a> and ('a -> int)
        |> Seq.toArray

    /// <summary>
    /// Parses a sequence of tokens and parentheses into a tree structure.
    /// </summary>
    /// <param name="tokens">The input sequence of tokens and parentheses.</param>
    /// <returns>A tree structure representing the parsed data.</returns>
    static member ParseToTree(tokens: IEnumerable<TokenOrParenthesis<'a>>) =
        let hedge, _ = sequence2Hedge (Seq.ofArray (Seq.toArray tokens)) // Use sequence2Hedge first
        let rec convertTree (tree: Tree<'a>) =
            match tree with
            | Node(value, children) ->
                InteropTree(value, children |> Seq.map convertTree)
        hedge |> Seq.map convertTree |> Seq.toArray // Convert the hedge to an array of InteropTree

