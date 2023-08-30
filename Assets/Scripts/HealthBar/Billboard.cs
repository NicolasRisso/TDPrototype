using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(cam.position);
    }
}
