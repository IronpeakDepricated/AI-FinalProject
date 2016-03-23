using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
	
	void Update ()
    {
        if(Application.isPlaying == true)
        {
            return;
        }
        int x = Mathf.RoundToInt(transform.position.x);
        float y = 0.5f;
        int z = Mathf.RoundToInt(transform.position.z);
        transform.position = new Vector3(x, y, z);
    }

}
