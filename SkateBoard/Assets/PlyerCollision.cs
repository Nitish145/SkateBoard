using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyerCollision : MonoBehaviour
{
    public SimpleSkateController controller;
    public GameObject canvas;

    void Start()
    {
        canvas.SetActive(false);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacles")
        {
            canvas.SetActive(true);
        }
    }
}
