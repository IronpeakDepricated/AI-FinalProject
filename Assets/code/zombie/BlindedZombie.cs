using UnityEngine;
using System.Collections;

public class BlindedZombie : MonoBehaviour {

    // Update is called once per frame
    int direction = 0;
    float total = 0;
    public ZombieState path;

    void Update()
    {
        path = null;
        if (Node.CanReach(transform.position, Player.player.transform.position, LayerMasks.CanNodeReachPlayer))
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.player.transform.position, Time.deltaTime * 10);
            path = null;
            return;
        }
        else
        {
            switch (direction)
            {
                case 0:
                    transform.position += (new Vector3(1, 0, 0)) * Time.deltaTime * 5;
                    break;
                case 1:
                    transform.position -= (new Vector3(1, 0, 0)) * Time.deltaTime * 5;
                    break;
                default:
                    break;
            }
            total += Time.deltaTime * 5;
            if (total >= 10)
            {
                if (direction == 1)
                {
                    direction = 0;
                }
                else if (direction == 0)
                {
                    direction = 1;
                }
                total = 0;
            }
        }
        
    }
}
