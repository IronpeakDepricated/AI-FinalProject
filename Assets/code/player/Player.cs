using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player player;
    public Rigidbody rb;
    
    public float MovementSpeed;
    public float RotationSpeed;


	void Awake ()
    {
        Player.player = this;
        rb = GetComponent<Rigidbody>();
        
	}

	void Update() {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement.normalized * MovementSpeed;
	}

}
