module internal OutlineAlgorithm.CreateHedge

open TokenOrParenthesis
open TreeAndHedge
open System.Collections.Generic

/// <summary>
/// Recursively processes a sequence of tokens and parentheses to construct a tree structure.
/// This function is conceptually similar to the `read` function in Lisp, which parses S-expressions
/// into a tree-like structure.
/// </summary>
/// <typeparam name="T">The type of the value stored in the tree nodes.</typeparam>
/// <param name="l">The input sequence of <see cref="TokenOrParenthesis"/> values.</param>
/// <returns>
/// A tuple containing:
/// <list type="bullet">
/// <item>A <see cref="Tree"/> representing the parsed structure.</item>
/// <item>The remaining sequence of <see cref="TokenOrParenthesis"/> values after processing.</item>
/// </list>
/// </returns>
let rec nest (en: IEnumerator<TokenOrParenthesis<'a>>) : Tree<'a> =
    if not (en.MoveNext()) then
        failwith "Unexpected end of input in nest"

    match en.Current with
    | Token(e) ->
        let children = sequence2Hedge en
        Node(Some e, children)
    | DummyToken ->
        let children = sequence2Hedge en
        Node(None, children)
    | _ ->
        failwithf "Unexpected token in nest: %A" en.Current


/// <summary>
/// Processes a sequence of tokens and parentheses to construct a hedge (a sequence of trees).
/// This function is conceptually similar to the `read` function in Lisp, which parses S-expressions
/// into a tree-like structure.
/// </summary>
/// <typeparam name="T">The type of the value stored in the tree nodes.</typeparam>
/// <param name="l">The input sequence of <see cref="TokenOrParenthesis"/> values.</param>
/// <returns>
/// A tuple containing:
/// <list type="bullet">
/// <item>A <see cref="Hedge"/> representing the parsed sequence of trees.</item>
/// <item>The remaining sequence of <see cref="TokenOrParenthesis"/> values after processing.</item>
/// </list>
/// </returns>
and sequence2Hedge (en: IEnumerator<TokenOrParenthesis<'a>>) : Hedge<'a> =
    [
        let mutable done_ = false
        while not done_ && en.MoveNext() do
            match en.Current with
            | StartParenthesis ->
                yield nest en
            | EndParenthesis ->
                done_ <- true
            | _ ->
                failwithf "Unexpected token in sequence2Hedge: %A" en.Current
    ]

