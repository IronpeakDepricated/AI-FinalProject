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
		// Rotate left
		if (Input.GetKey(KeyCode.LeftArrow)) 
		{
            
			transform.Rotate(0, -RotationSpeed, 0);
		}
		// Rotate right
		if (Input.GetKey(KeyCode.RightArrow)) 
		{
			transform.Rotate(0, RotationSpeed, 0);
		}
        
        /*
		// Strafe left
		if (Input.GetAxis(KeyCode.A)) 
		{ 
            rb.MovePosition(transform.position - transform.right * Time.deltaTime * MovementSpeed);
			//transform.position -= new Vector3(1,0,0);
		}
		// Move forward
		if (Input.GetKey(KeyCode.W)) 
		{
			//transform.position += transform.forward*moveSpeed;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * MovementSpeed);

		} 
		// Move backward
		if (Input.GetKey(KeyCode.S)) 
		{
            rb.MovePosition(transform.position - transform.forward * Time.deltaTime * MovementSpeed);

			//transform.position -= transform.forward * moveSpeed;
		}
		// Strafe right     
		if (Input.GetKey(KeyCode.D)) 
		{
            rb.MovePosition(transform.position + transform.right * Time.deltaTime * MovementSpeed);
			//transform.position += new Vector3(1,0,0);
		} */
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement.normalized * MovementSpeed;
	}

}
