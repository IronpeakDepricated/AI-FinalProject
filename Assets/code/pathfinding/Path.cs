using UnityEngine;
using System.Collections.Generic;

public class Path
{
    private float distance;
    private List<Node> nodes;

    public float Distance
    {
        get
        {
            return distance;
        }
    }

    public IList<Node> Nodes
    {
        get
        {
            return nodes.AsReadOnly();
        }
    }

    public Path(List<Node> nodes, float distance)
    {
        this.nodes = nodes;
        this.distance = distance;
    }
}
