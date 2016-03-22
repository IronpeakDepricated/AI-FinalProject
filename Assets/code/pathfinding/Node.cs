using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Node
{

    public int ID;
    public bool CanReachPlayer;
    public List<NodeConnection> adjNodes = new List<NodeConnection>();

    public int Selected;
    public NodeComponent Component;

    public Node(NodeComponent Component)
    {
        Graph.graph.GraphNodes.Add(this);
        this.CanReachPlayer = false;
        this.Component = Component;
        this.Selected = 0;
        this.ID = 0;
    }

    public Node(Node node)
    {
        this.CanReachPlayer = node.CanReachPlayer;
        this.Component = node.Component;
        this.Selected = node.Selected;
        this.ID = node.ID;
    }

    public void FindAdjNodes(List<Node> nodes)
    {
        for(int i = 0; i < Graph.graph.GraphNodes.Count; i++)
        {
            if(this != Graph.graph.GraphNodes[i] && CanReach(Component.transform.position, Graph.graph.GraphNodes[i].Component.transform.position, LayerMasks.FindAdjNodes))
            {
                adjNodes.Add(new NodeConnection(Graph.graph.GraphNodes[i], Vector3.Distance(Component.transform.position, Graph.graph.GraphNodes[i].Component.transform.position)));
            }
        }
    }

    public static bool CanReach(Vector3 origin, Vector3 target, LayerMask mask)
    {
        Vector3 x = target - origin;
        Vector3 cross = Vector3.Cross(x, Vector3.up);
        cross = cross.normalized * 0.49f;
        Vector3 a1 = target + cross;
        Vector3 b1 = origin + cross;
        if(Physics.Linecast(a1, b1, mask))
        {
            return false;
        }
        Vector3 a2 = target - cross;
        Vector3 b2 = origin - cross;
        if(Physics.Linecast(a2, b2, mask))
        {
            return false;
        }
        return true;
    }

}
