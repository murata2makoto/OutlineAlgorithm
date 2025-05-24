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
