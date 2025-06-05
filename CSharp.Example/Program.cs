using OutlineAlgorithm.Interop;
using System.Text.Json;
using System.Xml.Linq;


class Program
{
    static void Main()
    {
        // Input outline headings
        var input1 = new List<string> { "H1", "H2", "H4", "H3", "H5", "H1", "H4", "H3" };
        var input2 = new List<string> { "H1", "H2", "P", "H4", "H3", "P", "H5", "P", "H1", "H4", "P", "H3" };
        var input3 = new List<string> {"H1", "P", "H2", "P", "H2", "P", "H3"};
        var input4 = new List<string> {"H1","P", "H3","P","H3","H2","P", "P2","H3"};
        var input5 = new List<string> {"H1", "P", "H2", "P", "H2"};
        var input6 = new List<string> {"H1", "P", "H2", "P", "H2", "UL", "LI", "LI2", "LI2", "LI", "LI2", "LI3", "P" };
        var input7 = new List<string> {"P", "P", "P" };
        var input8 = new List<string> { "P", "H1", "P", "P" };
        var input = input8;

        // Rank function mapping heading to numeric depth
        int GetRank(string h) => h switch
        {
            "H1" => 1,
            "H2" => 2,
            "H3" => 3,
            "H4" => 4,
            "H5" => 5,
            "H6" => 6,
            "P"  => 1,
            "P2" => 2,
            "UL" => 1,
            "LI" => 2,
            "LI2"=> 3,
            "LI3"=> 4,
            _ => throw new ArgumentException("undefined", h)
        };

        // Layer function
        int GetLayer(string h) => h switch
        {
            "H1" => 0,
            "H2" => 0,
            "H3" => 0,
            "H4" => 0,
            "H5" => 0,
            "H6" => 0,
            "P"  => 1,
            "P2" => 1,
            "UL" => 1,
            "LI" => 1,
            "LI2"=> 1,
            "LI3"=> 1,
            _ => throw new ArgumentException("undefined", h)
        };

        Console.WriteLine(JsonSerializer.Serialize(input));

        // Step 1: Convert input to token stream
        var r = new Func<string, int>(GetRank);
        var l = new Func<string, int>(GetLayer);
        var tree = InteropCSharp.CreateTreeWithRanksAndLayers(input, r, l, false);

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
