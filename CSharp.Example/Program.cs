using System;
using OutlineAlgorithm.Interop;

class Program
{
    static void Main()
    {
        // Input sequence
        var input = new List<string> { "H1", "H2", "H4", "H3" };

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
        var trees = Interop.ParseToTree(tokenSeq); // ParseToTree returns an array of InteropTree<T>

        // Print each tree in the array
        foreach (var tree in trees)
        {
            PrintTree(tree, 0);
        }
    }

    static void PrintTree<T>(InteropTree<T> tree, int level)
    {
        // Check if the value is null (representing DummyToken)
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