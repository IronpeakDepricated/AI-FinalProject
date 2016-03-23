using System.Collections.Generic;

public class SubGraph : IGraph
{

    public List<Node> GraphNodes { get; set; }

    public SubGraph()
    {
        GraphNodes = new List<Node>();
    }

    public SubGraph(List<Node> g)
    {
        GraphNodes = g;
    }

    public bool ContainsID(int ID)
    {
        for(int i = 0; i < GraphNodes.Count; i++)
        {
            if(GraphNodes[i].ID == ID)
            {
                return true;
            }
        }
        return false;
    }

}
