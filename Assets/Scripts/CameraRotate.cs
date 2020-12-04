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
    
    //<-----ZOOM--MOUSE--->
    public float zoomSpeedMouse = 10f;
    public float _zoomAmountMouse = 10f;
    private float _maxToClamp = -5f;
    
    public static float ClampAngle(float angle,float min , float max)
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
        
        //Getting distance in Vector3 for zoom
        lastPos = transform.position;
        
        //current angles of camera ( make sure its looking at what you want to rotate around)
        rotYAxis = transform.eulerAngles.y;
        rotXAxis = transform.eulerAngles.x;
        
        //distance between camera and what im rotate around
        distance = Vector3.Magnitude(initialVector);
    }
    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            // get your inputs
            rotYAxis += Input.GetAxis("Mouse X") * camSmooth;
            rotXAxis -= Input.GetAxis("Mouse Y") * camSmooth;
        
            //clamp the angle
            rotXAxis = ClampAngle(rotXAxis, yMin, yMax);
        
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
        else
        {
            //Getting ScrollWheel input
            _zoomAmountMouse += Input.GetAxis("Mouse ScrollWheel");
            _zoomAmountMouse = Mathf.Clamp(_zoomAmountMouse, _maxToClamp, -_maxToClamp);

            //creating Z value while zoom with Scroll
            var translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")), 10 - Mathf.Abs(_zoomAmountMouse));
            
            //assigning for Camera Object just created translate Z value
            transform.Translate(0, 0, translate * zoomSpeedMouse * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));
            
            //Distance from Camera and Object getting new distance like that
            distance = Vector3.Magnitude(transform.position - target.position);


        }
    }
}
