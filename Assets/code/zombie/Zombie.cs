using UnityEngine;

public class Zombie : MonoBehaviour
{

	void Start ()
    {
        var Queue;
        for(int i = 0; i < Graph.graph.graphNodes.Count; i++)
        {
            if(Node.CanReachNode(transform.position, Graph.graph.graphNodes[i]))
            {
                Queue.enqueue(Graph.graph.graphNodes[i]);
            }
        }
	}

}
