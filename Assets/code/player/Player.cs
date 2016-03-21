using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player player;
	private float moveSpeed = 1;

	void Awake ()
    {
        Player.player = this;
	}

	void Update() {
		// Rotate left
		if (Input.GetKey(KeyCode.LeftArrow)) 
		{
			transform.Rotate(0, -1, 0);
		}
		// Rotate right
		if (Input.GetKey(KeyCode.RightArrow)) 
		{
			transform.Rotate(0, 1, 0);
		}
		// Strafe left
		if (Input.GetKey (KeyCode.A)) 
		{ 
			transform.position -= new Vector3(1,0,0);
		}
		// Move forward
		if (Input.GetKey (KeyCode.W)) 
		{
			transform.position += transform.forward*moveSpeed;
		} 
		// Move backward
		if (Input.GetKey (KeyCode.S)) 
		{
			transform.position -= transform.forward * moveSpeed;
		}
		// Strafe right     
		if (Input.GetKey (KeyCode.D)) 
		{
			transform.position += new Vector3(1,0,0);
		} 
	}

}
