using UnityEngine;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    public ZombieState path;
    Queue<ZombieState> states = new Queue<ZombieState>();

	void Update ()
    {
        path = null;
        if(Node.CanReachNode(transform.position, Player.player.transform.position))
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.player.transform.position, Time.deltaTime * 10);
            path = null;
            return;
        }
        List<bool> marked = new List<bool>(Graph.graph.graphNodes.Count);
        for(int i = 0; i < Graph.graph.graphNodes.Count; i++)
        {
            marked.Add(false);
        }
        Queue<ZombieState> available = new Queue<ZombieState>();
        for(int i = 0; i < Graph.graph.graphNodes.Count; i++)
        {
            if(Node.CanReachNode(transform.position, Graph.graph.graphNodes[i].transform.position) && Vector3.Distance(transform.position, Graph.graph.graphNodes[i].transform.position) != 0)
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
            marked[next.node.ID] = true;
            if(next.node.ReachPlayer)
            {
                Debug.Log(count);
                path = next;
                break;
            }
            for(int i = 0; i < next.node.adjNodes.Count; i++)
            {
                if(marked[next.node.ID] == false)
                {
                    available.Enqueue(new ZombieState(next, next.node.adjNodes[i]));
                }
            }
        }
        ZombieState n = path;
        if(path != null)
        {
            while(n.prev != null)
            {
                n = n.prev;
            }
            if(n.node == null)
                Debug.Log("n is null");
            transform.position = Vector3.MoveTowards(transform.position, n.node.transform.position, Time.deltaTime * 5);
        }
	}

    void OnDrawGizmos()
    {
        if(path == null)
        {
            Gizmos.DrawLine(transform.position, Player.player.transform.position);
            return;
        }
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
