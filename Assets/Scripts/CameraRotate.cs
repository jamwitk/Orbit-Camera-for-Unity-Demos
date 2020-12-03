using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR;

public class CameraRotate : MonoBehaviour
{

   
    public float yMin = -4f;
    public float yMax = 80f;
    
    public Transform target;
    private Vector3 initialVector;
    public float distance = 5f;
    public float rotYAxis = 0f;
    public float rotXAxis = 0f;
    public float camSmooth = 5f;
    private Vector3 lastPos;
    private void Orbit()
    {
        // get your inputs
        
        rotYAxis += Input.GetAxis("Mouse X") * camSmooth;
        rotXAxis += Input.GetAxis("Mouse Y") * camSmooth;
        //clamp the angle
        rotXAxis = CampleAngle(rotXAxis, yMin, yMax);
        //Convert it to quaternions
        Quaternion toRotation = Quaternion.Euler(rotXAxis, rotYAxis, 0);
        Quaternion rotation = toRotation;
        //figure out what your distance should be 
        Vector3 negDistance = new Vector3(0,0,-distance);
        Vector3 position = rotation * negDistance + Vector3.zero;
        
        // and apply
        transform.rotation = rotation;
        transform.position = position;
    }

    public static float CampleAngle(float angle,float min , float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
    private void Start()
    {
        // vector from where I m to what Im rotating around 
        initialVector = transform.position - target.position;
        transform.LookAt(target);
        //current angles of camera ( make sure its looking at what you want to rotate around)
        rotYAxis = transform.eulerAngles.y;
        rotXAxis = transform.eulerAngles.x;
        
        //distance between me and what im rotate around
        distance = Vector3.Magnitude(initialVector);
    }


 
    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Orbit();
        }
    }
}
