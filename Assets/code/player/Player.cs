using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player player;
    public Rigidbody rb;
    
    public float MovementSpeed;
    public float RotationSpeed;
    
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    
    private float nextFire;


	void Awake ()
    {
        Player.player = this;
        rb = GetComponent<Rigidbody>();
        
	}

	void Update() 
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
	}
    
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement.normalized * MovementSpeed;
    }

}
