using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;

    void LateUpdate()
    {
        if (cam != null)
        {
            transform.position = new Vector3(
                cam.transform.position.x,
                cam.transform.position.y,
                transform.position.z
            );
        }
    }
}
