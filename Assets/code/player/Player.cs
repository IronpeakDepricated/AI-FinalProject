using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player player;

	void Awake ()
    {
        Player.player = this;
	}

}
