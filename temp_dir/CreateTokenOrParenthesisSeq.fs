module OutlineAlgorithm.CreateTokenOrParenthesisSeq

open TokenOrParenthesis

/// <summary>
/// Creates a sequence of tokens and parentheses based on the input sequence and a ranking function.
/// </summary>
/// <typeparam name="T">The type of elements in the input sequence.</typeparam>
/// <param name="l">The input sequence of elements to process.</param>
/// <param name="getRank">
/// A function that determines the rank (or level) of each element in the sequence.
/// The rank is used to decide the nesting structure of parentheses.
/// </param>
/// <returns>
/// A sequence of <see cref="TokenOrParenthesis"/> values representing the input sequence
/// with appropriate parentheses added based on the rank of each element.
/// </returns>
let createTokenOrParenthesisSeq 
        (l: seq<'a>) (getRank: ('a -> int)) = 
    seq{let mutable currentLevel = 0
        for e in l do
          let nextLevel = getRank e
          if nextLevel = currentLevel then
            yield EndParenthesis
            yield StartParenthesis
            yield Token(e)
          elif nextLevel > currentLevel then
            /// <summary>
            /// Handles cases where the level increases significantly (e.g., from H2 to H4).
            /// For each missing level, a <see cref="StartParenthesis"/> and a <see cref="DummyToken"/> 
            /// are generated to maintain the correct nesting structure.
            /// </summary>
            for i = 1 to nextLevel - currentLevel - 1 do
                yield StartParenthesis
                yield DummyToken
            yield StartParenthesis
            yield Token(e)
          elif nextLevel < currentLevel then
            for i = 0 to currentLevel - nextLevel do
              yield EndParenthesis
            yield StartParenthesis
            yield Token(e)
          else failwith "hen"
          currentLevel <- nextLevel;
        for j = 1 to currentLevel do yield EndParenthesis
        }
