using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public Transform t;
    private const float Y = (float)1.5;
   
    // Start is called 0before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.localRotation.eulerAngles.x;
        float y = transform.localRotation.eulerAngles.y + 180 ;
        float z = transform.localRotation.eulerAngles.z;
        float yPos = t.position.y + Y;

        transform.position = new Vector3(t.position.x, yPos , t.position.z);
        transform.localEulerAngles = new Vector3(x, y, z);

    }
}
