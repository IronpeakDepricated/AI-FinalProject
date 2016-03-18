using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
	
	void Update ()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);
        int z = Mathf.RoundToInt(transform.position.z);
        transform.position = new Vector3(x, y, z);
    }

}
