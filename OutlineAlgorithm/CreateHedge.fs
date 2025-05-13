module internal OutlineAlgorithm.CreateHedge

open TokenOrParenthesis
open TreeAndHedge

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
let rec nest (l:seq<TokenOrParenthesis<'a>>): 
        Tree<'a> * seq<TokenOrParenthesis<'a>> =

    let head, tail = Seq.head l, Seq.tail l
    match head with 
    | Token(e) ->
        let content, remainder =  sequence2Hedge tail
        Node (Some(e),content), remainder
    | DummyToken ->
        let content, remainder =  sequence2Hedge tail
        Node (None,content), remainder
    | _ -> failwith "shouldn't happen"

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
and sequence2Hedge (l:seq<TokenOrParenthesis<'a>>):
        Hedge<'a> * seq<TokenOrParenthesis<'a>> =

    if Seq.isEmpty l then
        Seq.empty, l
    else
        let head, tail = Seq.head l, Seq.tail l
        match head with
        | Token(_) | DummyToken ->
            failwith "Should not happen"
        | EndParenthesis -> 
            Seq.empty, tail
        | StartParenthesis -> 
            let subtree, remainder = nest tail
            let rest, remainder = sequence2Hedge remainder
            Seq.append (seq{subtree}) rest,
            remainder


