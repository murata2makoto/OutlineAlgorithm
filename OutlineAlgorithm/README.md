# OutlineAlgorithm

OutlineAlgorithm is a library designed to process a sequence of headings (e.g., h1, h2, h3) with explicit nesting levels into a hierarchical tree structure. This library is compatible with both F# and C#, making it accessible to developers from both ecosystems.

The library operates in two stages:

1. **Token and Parenthesis Generation**: The input sequence of headings is first converted into a sequence of tokens and parentheses. This intermediate representation captures the nesting structure of the headings.
2. **Tree Construction**: The sequence of tokens and parentheses is then parsed to construct a hierarchical tree structure.

This two-step process allows the library to handle cases where the nesting levels change abruptly, such as skipping levels (e.g., from h2 to h5) or returning to a shallower level (e.g., from h5 to h2).

Whether you're working in F# or C#, OutlineAlgorithm provides a straightforward API to process documents with deeply nested structures, such as HTML or OOXML documents, efficiently.

---

## How It Works

### Stage 1. Token and Parenthesis Generation

The input sequence of headings is first converted into a sequence of **tokens and parentheses**. This intermediate representation captures the nesting structure of the headings in a way that simplifies subsequent tree construction.

Crucially, the process always begins by implicitly inserting a **`StartParenthesis` followed by a `DummyToken`**. This initial `DummyToken` acts as the **conceptual root** for the entire document, ensuring that even if the input contains multiple `H1`s, there's a single, consistent top-level node for the tree.

Furthermore, when the nesting level of headings jumps unexpectedly (e.g., from `H1` to `H3` skipping `H2`), additional **`DummyToken`s are implicitly inserted** to represent the skipped intermediate levels. These `DummyToken`s, whether for the root or for skipped levels, are treated identically in the subsequent tree construction phase.

For instance, given the heading sequence `H1, H3, H1, H2`, the generated token and parenthesis sequence would be:

`StartParenthesis, Token(DummyToken), StartParenthesis, Token(H1), StartParenthesis, DummyToken, StartParenthesis, Token(H3), EndParenthesis, EndParenthesis, EndParenthesis, StartParenthesis, Token(H1), StartParenthesis, Token(H2), EndParenthesis, EndParenthesis, EndParenthesis, EndParenthesis`

### Stageg 2. Tree Construction

The sequence of tokens and parentheses is then parsed to construct the final hierarchical tree structure. This parsing step is **inspired by the robust parsing mechanism of LISP's `read` function**, allowing the library to efficiently build the nested tree from the parenthesis-delimited token stream.

During this phase, all `DummyToken`s (whether they represent the initial root or skipped intermediate levels) are processed in the same manner. They act as placeholder nodes that facilitate the correct nesting and hierarchical positioning of actual heading nodes.

This two-step process allows the library to handle cases where the nesting levels change abruptly, such as skipping levels (e.g., from `H1` to `H3`) or returning to a shallower level (e.g., from `H3` to `H1`), all while maintaining a consistent single-rooted hierarchy.

---

## Example of Tree Construction

To illustrate how OutlineAlgorithm handles complex and irregular heading sequences, let's consider the input: `H1, H3, H1, H2`.

Here's how the library internally constructs the tree, including the `DummyToken`s:

* **Initial `DummyToken` (Root):** This acts as the single, consistent root for the entire tree.
* **First H1:** Becomes a direct child of the initial `DummyToken`.
* **H3:** As `H3` is deeper than `H1`, an intermediate `DummyToken` (representing the skipped `H2` level) is inserted as a child of `H1`, and `H3` then becomes a child of that intermediate `DummyToken`.
* **Second H1:** This `H1` is at the same logical level as the *first* `H1`. The parsing logic "pops" up the hierarchy from `H3`'s branch and attaches this `H1` as another child of the initial `DummyToken` (the conceptual root).
* **H2:** This `H2` is deeper than the second `H1`, so it becomes a child of the second `H1`.

The resulting internal hierarchical tree structure would logically represent:

```
- DummyToken (Conceptual Root)
    - H1
        - DummyToken (Placeholder for H2 level)
            - H3
    - H1
        - H2
```

This explicit representation of `DummyToken`s highlights how they are crucial for maintaining the correct hierarchical relationships and a single, unified tree structure, even with complex and irregular input heading sequences.

---

## Example Usage

### C# Example

The following example demonstrates how to use the library in C# to process a sequence of tokens and construct a tree structure:

Note that neither `DepthFirstTraversal` nor `TraverseWithOutlineIndex` visit dummy nodes.
```
using OutlineAlgorithm.Interop;
using System.Text.Json;

class Program
{
    static void Main()
    {
        // Input outline headings
        var input = new List<string> { "H1", "H2", "H4", "H3", "H5", "H1", "H4", "H3" };

        // Rank function mapping heading to numeric depth
        int GetRank(string h) => h switch
        {
            "H1" => 1,
            "H2" => 2,
            "H3" => 3,
            "H4" => 4,
            "H5" => 5,
            "H6" => 6,
            _ => 0
        };

        Console.WriteLine(JsonSerializer.Serialize(input));

        // Step 1: Convert input to token stream
        var tree = InteropCSharp.CreateTree(input, new Func<string, int>(GetRank));

        // Step 2: Depth-first traversal
        Console.WriteLine("\n=== Depth-First Traversal ===");
        InteropCSharp.DepthFirstTraversal(tree,
            val => Console.WriteLine($"Entering: {val}"),
            val => Console.WriteLine($"Leaving : {val}")
        );

        // Step 3: Traverse with outline index (e.g., [1,2,1])
        Console.WriteLine("\n=== Traverse With Outline Index ===");
        InteropCSharp.TraverseWithOutlineIndex(tree,
            (label, index) =>
                Console.WriteLine($"Label: {label}, Index: [{string.Join(", ", index)}]"));
    }
}

```

### F# Example

The following example demonstrates how to use the library in F# to process a sequence of tokens and construct a tree structure:

Note that neither `depthFirstTraversal` nor `traverseWithOutlineIndex` visit dummy nodes.
```
open OutlineAlgorithm.Interop

[<EntryPoint>]
let main argv =

    /// Input outline headings
    let input = ["H1"; "H2"; "H4"; "H3"; "H5"; "H1"; "H4"; "H3"]

    /// Rank function that assigns numeric depth based on the heading level
    let getRank (element: string) =
        match element with
        | "H1" -> 1
        | "H2" -> 2
        | "H3" -> 3
        | "H4" -> 4
        | "H5" -> 5
        | "H6" -> 6
        | _ -> 0

    printfn "%A" input

    /// Step 1: Convert input sequence to tokens with parentheses
    let tree = InteropFSharp.CreateTree input getRank

    /// Step 2: Traverse the tree using depth-first strategy
    printfn "\n=== Depth-First Traversal ==="
    InteropFSharp.depthFirstTraversal tree
        (fun x -> printfn "Entering: %s" x)
        (fun x -> printfn "Leaving : %s" x)

    /// Step 3: Traverse the tree and print outline indices (e.g., [1; 2; 1])
    printfn "\n=== Traverse with Outline Index ==="
    InteropFSharp.traverseWithOutlineIndex tree
        (fun x idx -> printfn "Label: %s, Index: %A" x idx)

    0   
```

### Output

Both versions provide the same result:

```
["H1"; "H2"; "H4"; "H3"; "H5"; "H1"; "H4"; "H3"]

=== Depth-First Traversal ===
Entering: H1
Entering: H2
Entering: H4
Leaving : H4
Entering: H3
Entering: H5
Leaving : H5
Leaving : H3
Leaving : H2
Leaving : H1
Entering: H1
Entering: H4
Leaving : H4
Entering: H3
Leaving : H3
Leaving : H1

=== Traverse with Outline Index ===
Label: H1, Index: [1]
Label: H2, Index: [1; 1]
Label: H4, Index: [1; 1; 1; 1]
Label: H3, Index: [1; 1; 2]
Label: H5, Index: [1; 1; 2; 1; 1]
Label: H1, Index: [2]
Label: H4, Index: [2; 1; 1; 1]
Label: H3, Index: [2; 1; 2]
```

## Installation

This project is written in F#. To set it up, follow these steps:

1. Clone the repository:
2. Restore dependencies (requires .NET SDK):

## API documentation

See [OutlineAlgorithm](https://murata2makoto.github.io/OutlineAlgorithm/).

## Contributing

Contributions are welcome! If you encounter any issues or have feature requests, please open an [issue](https://github.com/murata2makoto/OutlineAlgorithm/issues). Pull requests are also appreciated.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.
