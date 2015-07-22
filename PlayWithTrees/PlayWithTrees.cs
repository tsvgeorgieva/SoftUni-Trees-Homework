using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace PlayWithTrees
{
    public class PlayWithTrees
    {
        static Dictionary<int, Tree<int>> nodeByValue = new Dictionary<int, Tree<int>>();

        static void Main(string[] args)
        {
            int nodesCount = int.Parse(ReadLine());
            for (int i = 1; i < nodesCount; i++)
            {
                string[] edge = ReadLine().Split(' ');

                int parentValue = int.Parse(edge[0]);
                Tree<int> parentNode = GetTreeNodeByValue(parentValue);

                int childValue = int.Parse(edge[1]);
                Tree<int> childNode = GetTreeNodeByValue(childValue);

                parentNode.Children.Add(childNode);
                childNode.Parent = parentNode;
            }

            int pathSum = int.Parse(ReadLine());
            int subtreeSum = int.Parse(ReadLine());

            var rootNode = FindRootNode();
            WriteLine($"Root node: {rootNode?.Value}");

            var leafNodeValues = FindLeafNodes()
                .Select(leaf => leaf.Value)
                .OrderBy(value => value);
            WriteLine($"Leaf nodes: {string.Join(", ", leafNodeValues)}");

            var middleNodeValues = FindMiddleNodes()
                .Select(middle => middle.Value)
                .OrderBy(value => value);
            WriteLine($"Middle nodes: {string.Join(", ", middleNodeValues)}");

            var longestPath = FindLongestPath(rootNode).ToArray();
            Array.Reverse(longestPath);
            WriteLine($"Longest path: {string.Join("->", longestPath.Select(n => n.Value))} (length = {longestPath.Count()})");

            var subtrees = FindSubtreesWithSum(rootNode, subtreeSum);
            WriteLine($"Paths of sum {subtreeSum}:");
            foreach (var subtree in subtrees)
            {
                var list = new List<int>();
                subtree.Each(list.Add);
                WriteLine(string.Join(" + ", list));
            }
        }

        static Tree<int> GetTreeNodeByValue(int value)
        {
            if (! nodeByValue.ContainsKey(value))
            {
                nodeByValue[value] = new Tree<int>(value);
            }

            return nodeByValue[value];
        }

        static Tree<int> FindRootNode()
        {
            var rootNode = nodeByValue.Values
                .FirstOrDefault(node => node.Parent == null);
            return rootNode;
        }

        static IEnumerable<Tree<int>> FindLeafNodes()
        {
            var leafNodes = nodeByValue.Values
                .Where(node => node.Children.Count == 0)
                .ToList();
            return leafNodes;
        }

        static IEnumerable<Tree<int>> FindMiddleNodes()
        {
            var middleNodes = nodeByValue.Values
                .Where(node => node.Children.Count > 0
                && node.Parent != null)
                .ToList();
            return middleNodes;
        }

        static IList<Tree<int>> FindLongestPath(Tree<int> treeNode)
        {
            IList<Tree<int>> longestPath = new List<Tree<int>>();
            foreach (var childNode in treeNode.Children)
            {
                var currentPath = FindLongestPath(childNode);
                if (currentPath.Count > longestPath.Count)
                {
                    longestPath = currentPath;
                }
            }

            longestPath.Add(treeNode);
            return longestPath;
        }

        static int FindTreeSum(Tree<int> node)
        {
            int sum = node.Value;
            foreach (var child in node.Children)
            {
                sum += FindTreeSum(child);
            }

            return sum;
        }

        static List<Tree<int>> FindSubtreesWithSum(Tree<int> root, int targetSum)
        {
            var results = new List<Tree<int>>();
            var currentSum = FindTreeSum(root);
            if (currentSum == targetSum)
            {
                results.Add(root);
            }

            foreach (var child in root.Children)
            {
                results.AddRange(FindSubtreesWithSum(child, targetSum));
            }

            return results;
        }
    }
}
