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
    | Token of 'a

    /// <summary>
    /// Represents a placeholder token with no value.
    /// </summary>
    | DummyToken

    /// <summary>
    /// Represents the start of a parenthesis.
    /// </summary>
    | StartParenthesis

    /// <summary>
    /// Represents the end of a parenthesis.
    /// </summary>
    | EndParenthesis

