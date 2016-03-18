using UnityEngine;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    public ZombieState path;
    Queue<ZombieState> states = new Queue<ZombieState>();

	void Update ()
    {
        Queue<ZombieState> available = new Queue<ZombieState>();
        for(int i = 0; i < Graph.graph.graphNodes.Count; i++)
        {
            if(Node.CanReachNode(transform.position, Graph.graph.graphNodes[i]))
            {
                available.Enqueue(new ZombieState(Graph.graph.graphNodes[i], Vector3.Distance(transform.position, Graph.graph.graphNodes[i].transform.position)));
            }
        }

        uint count = 0;
        while(available.Count != 0)
        {
            count++;
            if(count > 10000)
            {
                return;
            }
            ZombieState next = available.Dequeue();
            if(next.node.ReachPlayer)
            {
                path = next;
                return;
            }
            for(int i = 0; i < next.node.adjNodes.Count; i++)
            {
                available.Enqueue(new ZombieState(next, next.node.adjNodes[i]));
            }
        }
	}

    void OnDrawGizmos()
    {
        if(path == null)
            return;
        if(Application.isPlaying == false)
            return;
        ZombieState n = path;
        Gizmos.color = Color.black;
        Gizmos.DrawLine(n.node.transform.position, Player.player.transform.position);
        while(n.prev != null)
        {
            Gizmos.DrawLine(n.node.transform.position, n.prev.node.transform.position);
            n = n.prev;
        }
        Gizmos.DrawLine(n.node.transform.position, transform.position);
    }

}
