using System.Collections.Generic;

[System.Serializable]
public class Graph : IGraph
{

    public static Graph graph;

    public GraphComponent Component;
    public List<Node> GraphNodes { get; set; }
    public List<SubGraph> SubGraphs;

    public Graph(GraphComponent Component)
    {
        Graph.graph = this;
        this.Component = Component;
        GraphNodes = new List<Node>();
    }

    public void SetNodeIDs()
    {
        for(int i = 0; i < GraphNodes.Count; i++)
        {
            GraphNodes[i].ID = i;
        }
    }

    public void GenerateGraph()
    {
        for(int i = 0; i < GraphNodes.Count; i++)
        {
            GraphNodes[i].FindAdjNodes(GraphNodes);
        }
    }

    public void SetPlayerReachableNodes()
    {
        for (int i = 0; i < GraphNodes.Count; i++)
        {
            GraphNodes[i].CanReachPlayer = Component.CanReachPlayer(GraphNodes[i]);
        }
    }

    public void GenerateSubgraphs(Player player)
    {
        SubGraphs = new List<SubGraph>();
        SubGraphs.Add(new SubGraph(GraphNodes));
    }

}
