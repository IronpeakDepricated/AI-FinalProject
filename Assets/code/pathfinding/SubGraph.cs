using System.Collections.Generic;

public class SubGraph
{

    public List<Node> GraphNodes { get; set; }

    public SubGraph(List<Node> nodes)
    {
        GraphNodes = new List<Node>();
        for(int i = 0; i < nodes.Count; i++)
        {
            Node node = new Node(nodes[i]);
            nodes[i].FindAdjNodes(nodes);
        }
    }

}
