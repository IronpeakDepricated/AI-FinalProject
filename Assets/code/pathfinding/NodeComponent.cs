using UnityEngine;
using System.Collections.Generic;

public class NodeComponent : MonoBehaviour
{

    public Node Node;

    void Awake()
    {
        Node = new Node(this);
    }

    void Start()
    {
        Node.FindAdjNodes(Graph.graph.GraphNodes);
    }

    public void OnDrawGizmosSelected()
    {
        if (Application.isPlaying == false)
            return;
        DrawNodeConnections(Node.adjNodes);
    }

    public void DrawNodeConnections(List<NodeConnection> connections)
    {
        Gizmos.color = Color.green;
        for(int i = 0; i < connections.Count; i++)
        {
            Vector3 x = connections[i].node.Component.transform.position - transform.position;
            Vector3 cross = Vector3.Cross(x, Vector3.up);
            cross = cross.normalized * 0.49f;
            Vector3 a1 = connections[i].node.Component.transform.position + cross;
            Vector3 b1 = transform.position + cross;
            Vector3 a2 = connections[i].node.Component.transform.position - cross;
            Vector3 b2 = transform.position - cross;
            Gizmos.DrawLine(a1, b1);
            Gizmos.DrawLine(a2, b2);
        }
    }

}
