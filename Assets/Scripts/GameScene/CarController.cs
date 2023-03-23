using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Wheels")]
    [SerializeField] private WheelCollider[] wheelsCol;
    [SerializeField] private GameObject[] wheelsMesh;
    [Header("Car stats")]
    [SerializeField] private Transform MassCenter;
    [SerializeField] private float speed, turnRadius, downForce = 50f;  
    private Rigidbody rb;
    private float horizontal, vertical;
    private bool canMove = false;

    public void allowMovement(bool value)
    {
        canMove = value;
    }
    public void updSpeed(int value)
    {
        speed += value;
    }
    private void applySpeed(float value)
    {
        foreach (var wheel in wheelsCol)
        {
            wheel.motorTorque = value;
        }
    }
    private void addDownForce()
    {
        rb.AddForce(-transform.up * downForce * rb.velocity.magnitude);
    }
    private void turnWheels(float input)
    {
        //ACKERMAN STEERING FORMULA
        if (input > 0)
        {
            wheelsCol[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (turnRadius + (1.5f / 2))) * input;
            wheelsCol[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (turnRadius - (1.5f / 2))) * input;
        }
        else if(input < 0)
        {
            wheelsCol[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (turnRadius - (1.5f / 2))) * input;
            wheelsCol[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (turnRadius + (1.5f / 2))) * input;
        }
        else
        {    
            wheelsCol[0].steerAngle = 0;
            wheelsCol[1].steerAngle = 0;
        }       
    }
    private void animWheels()
    {
        Vector3 wheelPos;
        Quaternion wheelRot;
        for (int i = 0; i < 4; i++)
        {
            wheelsCol[i].GetWorldPose(out wheelPos, out wheelRot);
            wheelsMesh[i].transform.position = wheelPos;
            wheelsMesh[i].transform.rotation = wheelRot;
        }
    }    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = MassCenter.localPosition;
    }
    private void Update()
    {
        getInput();
        addDownForce();
        if (canMove)
        {
            animWheels();
            applySpeed(vertical * speed);
            turnWheels(horizontal);           
        }
    }

    private void getInput()
    {
        #if UNITY_STANDALONE
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        #endif
        #if UNITY_IOS || UNITY_ANDROID
        horizontal = SimpleInput.GetAxis("Horizontal");
        vertical = SimpleInput.GetAxis("Vertical");
        #endif
    }
}
