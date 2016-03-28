using UnityEngine;
using System.Collections;

public class ZombieAnimation : MonoBehaviour
{

    public Zombie zombie;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
	
	void Update ()
    {
        animator.SetFloat("speed", zombie.Speed);
        if(zombie.Alive == false)
        {
            animator.Play("death");
            this.enabled = false;
        }
	}

}
