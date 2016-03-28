using UnityEngine;
using System.Collections;

public class PlayerGun : MonoBehaviour
{

    public float RateOfFire = 0.2f;
    public LayerMask HitMask;

    private float LastFireTime = -1;

    void Update()
    {
        if((LastFireTime + RateOfFire) <= Time.time && Input.GetButtonDown("Fire1"))
        {
            LastFireTime = Time.time;
            RaycastHit hit;
            Ray ray = PlayerCamera.FPSCamera.ScreenPointToRay(new Vector3(PlayerCamera.FPSCamera.pixelWidth, 0, PlayerCamera.FPSCamera.pixelHeight));
            if(Physics.Raycast(PlayerCamera.FPSCamera.transform.position, PlayerCamera.FPSCamera.transform.forward, out hit, 100, HitMask))
            {
                if(hit.transform.tag == "Enemy")
                {
                    IHealth health = hit.transform.GetComponent<IHealth>();
                    Debug.Log("Hit Enemy");
                    health.DealDamage();
                }
            }
        }
    }

}
