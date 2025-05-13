module internal OutlineAlgorithm.TreeAndHedge

/// <summary>
/// Represents a tree structure where each node contains an optional value and a sequence of child trees (hedge).
/// </summary>
/// <typeparam name="T">The type of the value stored in the tree nodes.</typeparam>
type Tree<'a> = 
    /// <summary>
    /// A node in the tree containing an optional value and a sequence of child trees.
    /// </summary>
    /// <param name="value">The optional value stored in the node.</param>
    /// <param name="children">The sequence of child trees (hedge) associated with the node.</param>
    Node of 'a option * Hedge<'a>

/// <summary>
/// Represents a hedge, which is a sequence of trees.
/// </summary>
/// <typeparam name="T">The type of the value stored in the tree nodes within the hedge.</typeparam>
and Hedge<'a> = Tree<'a> seq