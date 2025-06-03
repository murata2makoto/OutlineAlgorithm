module internal OutlineAlgorithm.CreateTokenOrParenthesisSeq

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
        (l: seq<'a>) (getRank: ('a -> int)) (getLayer: ('a -> int) ) (debug: bool) = 

    seq{let mutable currentLayer = 0
        let rank = Array.init 100 (fun _ -> 0)
        rank.[currentLayer] <- 0
        for e in l do
          let nextRank = getRank e
          let nextLayer = getLayer e
          if currentLayer >= nextLayer then
              for layer = currentLayer downto nextLayer + 1 do
                for _ = 1 to rank.[layer] do
                  yield (TokenOrParenthesis<'a>.CreateEndParenthesis layer debug) 
                rank.[layer] <- 0
              if nextRank <= rank.[nextLayer] then
                for i = nextRank to rank.[nextLayer] do
                  yield (TokenOrParenthesis<'a>.CreateEndParenthesis currentLayer debug) 
                yield (TokenOrParenthesis<'a>.CreateStartParenthesis currentLayer debug) 
                yield (TokenOrParenthesis<'a>.CreateToken e currentLayer debug)
              elif nextRank > rank.[currentLayer] then
                /// <summary>
                /// Handles cases where the level increases significantly (e.g., from H2 to H4).
                /// For each missing level, a <see cref="StartParenthesis"/> and a <see cref="DummyToken"/> 
                /// are generated to maintain the correct nesting structure.
                /// </summary>
                for i = rank.[nextLayer] + 1 to nextRank - 1 do
                    yield (TokenOrParenthesis<'a>.CreateStartParenthesis nextLayer debug);
                    yield (TokenOrParenthesis<'a>.CreateDummyToken nextLayer debug);
                yield (TokenOrParenthesis<'a>.CreateStartParenthesis nextLayer debug); 
                yield (TokenOrParenthesis<'a>.CreateToken e nextLayer debug)
              else failwith "hen"
          elif currentLayer < nextLayer then
            for layer = currentLayer + 1 to nextLayer - 1 do
                rank.[layer] <- 0
            for _ = 1 to nextRank - 1 do
              yield (TokenOrParenthesis<'a>.CreateStartParenthesis nextLayer debug);
              yield (TokenOrParenthesis<'a>.CreateDummyToken nextLayer debug);
            yield (TokenOrParenthesis<'a>.CreateStartParenthesis nextLayer debug);
            yield (TokenOrParenthesis<'a>.CreateToken e nextLayer debug);
          else failwith "hen"
          currentLayer <- nextLayer;
          rank.[nextLayer] <- nextRank

        for i = currentLayer downto 0 do
          for k = 1 to rank.[i] do 
            yield (TokenOrParenthesis<'a>.CreateEndParenthesis i debug)

        } |> Seq.toList |> Seq.ofList
