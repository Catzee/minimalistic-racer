using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cam;
    public Transform followTransform;
    public Vector3 followTransformOffset;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 250;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cam.position = followTransform.position + followTransformOffset;
        cam.LookAt(followTransform);
    }
}
