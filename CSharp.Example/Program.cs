using OutlineAlgorithm.Interop;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Input outline headings
        var input = new List<string> { "H1", "H2", "H4", "H3", "H5", "H3", "H5" };

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

        // Step 1: Convert input to token stream
        var tokens = InteropCSharp.CreateTokenOrParenthesisSeq(input, new Func<string, int>(GetRank));
        Console.WriteLine("=== Tokens ===");
        foreach (var token in tokens)
            Console.WriteLine($"{token}");

        // Step 2: Parse tokens into tree structure
        var tree = InteropCSharp.ParseToTree(tokens);

        // Step 3: Depth-first traversal
        Console.WriteLine("\n=== Depth-First Traversal ===");
        InteropCSharp.DepthFirstTraversal(tree,
            val => Console.WriteLine($"Entering: {val}"),
            val => Console.WriteLine($"Leaving : {val}")
        );

        // Step 4: Traverse with outline index (e.g., [1,2,1])
        Console.WriteLine("\n=== Traverse With Outline Index ===");
        InteropCSharp.TraverseWithOutlineIndex(tree,
            (label, index) =>
                Console.WriteLine($"Label: {label}, Index: [{string.Join(", ", index)}]"));
    }
}
