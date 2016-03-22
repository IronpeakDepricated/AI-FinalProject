using System.Collections.Generic;

public class SubGraph : IGraph
{

    public List<Node> GraphNodes { get; set; }

    public SubGraph(List<Node> nodes)
    {
        GraphNodes = new List<Node>();
        for(int i = 0; i < nodes.Count; i++)
        {
            Node node = new Node(nodes[i]);
            node.FindAdjNodes(nodes);
            GraphNodes.Add(node);
        }
    }

}
