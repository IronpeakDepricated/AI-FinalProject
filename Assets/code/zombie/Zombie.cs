using UnityEngine;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    public ZombieState path;

	void Update ()
    {
        path = null;
        if(Node.CanReach(transform.position, Player.player.transform.position))
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.player.transform.position, Time.deltaTime * 10);
            path = null;
            return;
        }
        else
        {
            bool[] marked = Graph.graph.GetMarked();
            PriorityQueue<ZombieState> queue = new PriorityQueue<ZombieState>();
            for(int i = 0; i < Graph.graph.TotalNodeCount(); i++)
            {
                float distance = Vector3.Distance(transform.position, Graph.graph.GetNode(i).transform.position);
                if(Node.CanReach(transform.position, Graph.graph.GetNode(i).transform.position) && distance != 0)
                {
                    marked[Graph.graph.GetNode(i).ID] = true;
                    ZombieState state = new ZombieState(Graph.graph.GetNode(i), distance);
                    queue.Push(state.distance, state);
                }
            }
            
            int expansions = 0;
            while(queue.IsEmpty() == false)
            {
                expansions++;
                ZombieState next = queue.Pop();
                if(next.node.CanReachPlayer)
                {
                    Debug.Log(expansions);
                    path = next;
                    break;
                }
                for(int i = 0; i < next.node.adjNodes.Count; i++)
                {
                    if(marked[next.node.adjNodes[i].node.ID] == false)
                    {
                        marked[next.node.adjNodes[i].node.ID] = true;
                        ZombieState state = new ZombieState(next, next.node.adjNodes[i]);
                        queue.Push(state.distance, state);
                    }
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
