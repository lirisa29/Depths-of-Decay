using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    void Update() 
    {
        transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, -10);	
    }
}
