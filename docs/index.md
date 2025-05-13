# OutlineAlgorithm

**OutlineAlgorithm** is a library for converting a flat sequence of headings (e.g., `h1`, `h2`, `h3`) - each with an explicit nesting level - into a structured hierarchical tree. It is designed for use with both **F#** and **C#**, ensuring compatibility across .NET languages.

The conversion process consists of two main stages:

1. **Token and Parenthesis Generation**  
   The input sequence is first transformed into a linear representation using tokens and parentheses that encode the nesting structure.  
   Implemented by: `CreateTokenOrParenthesisSeq` in `InteropCSharp` or `InteropFSharp`.

2. **Tree Construction**  
   This token sequence is then parsed to construct a tree that reflects the document’s logical outline.  
   Implemented by: `ParseToTree` in `InteropCSharp` or `InteropFSharp`.

**OutlineAlgorithm** is useful for tasks such as outline parsing, document structure analysis, and hierarchical data visualization.
