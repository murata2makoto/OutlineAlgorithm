module internal OutlineAlgorithm.TokenOrParenthesis

/// <summary>
/// Represents a token or a parenthesis in a sequence, used for outlining or parsing operations.
/// </summary>
/// <typeparam name="T">The type of the token value.</typeparam>
type TokenOrParenthesis<'a> =
    /// <summary>
    /// Represents a token with a specific value.
    /// </summary>
    /// <param name="value">The value of the token.</param>
    | Token of 'a * int

    /// <summary>
    /// Represents a placeholder token with no value.
    /// </summary>
    | DummyToken of int

    /// <summary>
    /// Represents the start of a parenthesis.
    /// </summary>
    | StartParenthesis of int

    /// <summary>
    /// Represents the end of a parenthesis.
    /// </summary>
    | EndParenthesis of int

    static member CreateToken(value: 'a) (layer: int) (debug: bool)=
        if debug then printfn "Token: %A" value
        Token (value, layer)
        
    static member CreateDummyToken(layer: int) (debug: bool)=
        if debug then printfn "Dummy: %d" layer
        DummyToken layer
        
    static member CreateStartParenthesis (layer: int) (debug: bool)=
        if debug then printfn "(: %d" layer
        StartParenthesis layer

    static member CreateEndParenthesis(layer: int) (debug: bool)=
        if debug then printfn "): %d" layer
        EndParenthesis layer