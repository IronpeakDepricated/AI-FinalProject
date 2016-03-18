using UnityEngine;
using System.Collections.Generic;
using System;

public class Node : MonoBehaviour
{

    public int ID;
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
            if(this != Graph.graph.graphNodes[i] && CanReach(transform.position, Graph.graph.graphNodes[i].transform.position))
            {
                adjNodes.Add(new NodeConnection(Graph.graph.graphNodes[i], Vector3.Distance(transform.position, Graph.graph.graphNodes[i].transform.position)));
            }
        }
    }

    public static bool CanReach(Vector3 origin, Vector3 target)
    {
        Vector3 x = target - origin;
        Vector3 cross = Vector3.Cross(x, Vector3.up);
        cross = cross.normalized * 0.49f;
        Vector3 a1 = target + cross;
        Vector3 b1 = origin + cross;
        if(Physics.Linecast(a1, b1))
        {
            return false;
        }
        Vector3 a2 = target - cross;
        Vector3 b2 = origin - cross;
        if(Physics.Linecast(a2, b2))
        {
            return false;
        }
        return true;
    }

    public void OnDrawGizmosSelected()
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
