using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public uint ID;
    public bool ReachPlayer;
    public List<NodeConnection> adjNodes = new List<NodeConnection>();

    void Awake()
    {
        Graph.graph.graphNodes.Add(this);
    }

    void Start()
    {
        for(int i = 0; i < Graph.graph.graphNodes.Count; i++)
        {
            if(this != Graph.graph.graphNodes[i] && CanReachNode(transform.position, Graph.graph.graphNodes[i]))
            {
                adjNodes.Add(new NodeConnection(Graph.graph.graphNodes[i], Vector3.Distance(transform.position, Graph.graph.graphNodes[i].transform.position)));
            }
        }
    }

    public static bool CanReachNode(Vector3 origin, Node node)
    {
        Vector3 x = node.transform.position - origin;
        Vector3 cross = Vector3.Cross(x, Vector3.up);
        cross = cross.normalized * 0.49f;
        Vector3 a1 = node.transform.position + cross;
        Vector3 b1 = origin + cross;
        if(Physics.Linecast(a1, b1))
        {
            return false;
        }
        Vector3 a2 = node.transform.position - cross;
        Vector3 b2 = origin - cross;
        if(Physics.Linecast(a2, b2))
        {
            return false;
        }
        return true;
    }

    void OnDrawGizmos()
    {
        if(Application.isPlaying == false)
            return;
        Gizmos.color = Color.green;
        for(int i = 0; i < Graph.graph.graphNodes.Count; i++)
        {
            if(this != Graph.graph.graphNodes[i])
            {
                DrawLine(Graph.graph.graphNodes[i]);
            }
        }
    }

    void DrawLine(Node node)
    {
        Vector3 x = node.transform.position - transform.position;
        Vector3 cross = Vector3.Cross(x, Vector3.up);
        cross = cross.normalized * 0.49f;
        Vector3 a1 = node.transform.position + cross;
        Vector3 b1 = transform.position + cross;
        Vector3 a2 = node.transform.position - cross;
        Vector3 b2 = transform.position - cross;
        if(Physics.Linecast(a1, b1) || Physics.Linecast(a2, b2))
            return;
        Gizmos.DrawLine(a1, b1);
        Gizmos.DrawLine(a2, b2);
    }

}
