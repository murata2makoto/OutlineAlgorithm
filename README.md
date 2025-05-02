# OutlineAlgorithm

OutlineAlgorithm is an F# library designed to process a sequence of headings (e.g., h1, h2, h3) with explicit nesting levels into a hierarchical tree structure. The library operates in two stages:

1. **Token and Parenthesis Generation**: The input sequence of headings is first converted into a sequence of tokens and parentheses. This intermediate representation captures the nesting structure of the headings.
2. **Tree Construction**: The sequence of tokens and parentheses is then parsed to construct a hierarchical tree structure.

This two-step process allows the library to handle cases where the nesting levels change abruptly, such as skipping levels (e.g., from h2 to h4) or returning to a shallower level.

The library is particularly useful for processing documents with deeply nested structures, such as HTML or OOXML documents.

## Features

- **Token and Parenthesis Parsing**: Converts a sequence of tokens and parentheses into a structured tree.
- **Handles Deep Nesting**: Automatically inserts dummy tokens (`DummyToken`) to handle cases where the hierarchy deepens significantly (e.g., jumping from level H2 to H4).
- **Inspired by Lisp's `read` Function**: Implements functionality similar to Lisp's `read` function, which parses S-expressions into tree-like structures.

## Key Functions

### 1. `createTokenOrParenthesisSeq`
Generates a sequence of tokens and parentheses based on an input sequence and a ranking function. This function ensures that the nesting structure is maintained, even when the hierarchy deepens significantly.

### 2. `nest` and `sequence2Hedge`
These functions recursively process a sequence of tokens and parentheses to construct a tree (`Tree`) or a hedge (`Hedge`), which is a sequence of trees. These functions are conceptually similar to Lisp's `read` function for parsing S-expressions.

## Example Usage

### C# Example

The following example demonstrates how to use the library in C# to process a sequence of tokens and construct a tree structure:


~~~
using System;
using System.Collections.Generic;
using OutlineAlgorithm.Interop;
class Program { static void Main() {
    // Input sequence var input = new List<string> { "H1", "H2", "H4", "H3" };
    // Rank function
    Func<string, int> getRank = element => element switch
    {
        "H1" => 1,
        "H2" => 2,
        "H3" => 3,
        "H4" => 4,
        _ => 0
    };

    // Generate token and parenthesis sequence
    var tokenSeq = Interop.CreateTokenOrParenthesisSeq(input, getRank);

    // Parse to tree
    var trees = Interop.ParseToTree(tokenSeq);

    // Print each tree in the sequence
    foreach (var tree in trees)
    {
        PrintTree(tree, 0);
    }
}

static void PrintTree<T>(InteropTree<T> tree, int level)
{
    if (tree.Value == null)
    {
        Console.WriteLine(new string(' ', level * 2) + "@");
    }
    else
    {
        Console.WriteLine(new string(' ', level * 2) + tree.Value);
    }

    foreach (var child in tree.Children)
    {
        PrintTree(child, level + 1);
    }
}
}
~~~
### F# Example

The following example demonstrates how to use the library in F# to process a sequence of tokens and construct a tree structure:
~~~
open OutlineAlgorithm.CreateTokenOrParenthesisSeq
open OutlineAlgorithm.CreateHedge
let input = [ "H1"; "H2"; "H4"; "H3" ]
let getRank element =
  match element with | "H1" -> 1 | "H2" -> 2 | "H3" -> 2 | "H4" -> 4 | _ -> 0
let tokenSeq = createTokenOrParenthesisSeq input getRank
let tree, _ = nest tokenSeq
printfn "%A" tree
~~~


## Installation

This project is written in F#. To set it up, follow these steps:

1. Clone the repository:

2. Restore dependencies (requires .NET SDK):


## Applications

OutlineAlgorithm is particularly useful for:
- Parsing and processing hierarchical document formats like HTML and OOXML.
- Generating tree structures for syntax analysis or document outlining.
- Handling cases where the hierarchy deepens significantly, ensuring structural consistency.

## Contributing

Contributions are welcome! If you encounter any issues or have feature requests, please open an [issue](https://github.com/your-username/OutlineAlgorithm/issues). Pull requests are also appreciated.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
