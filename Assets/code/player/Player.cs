using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player player;
    public Rigidbody rb;
    
    public float MovementSpeed;

	void Awake ()
    {
        Player.player = this;
        rb = GetComponent<Rigidbody>();
        
	}

	void Update() 
    {

	}
    
    void FixedUpdate()
    {
        float forward = Input.GetAxis("Vertical");
        float right = Input.GetAxis("Horizontal");

        Vector3 direction = transform.forward * forward + transform.right * right;
        rb.MovePosition(transform.position + direction.normalized * MovementSpeed * Time.deltaTime);
        rb.velocity = Vector3.zero;
    }

}
