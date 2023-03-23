using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [field: SerializeField]
    public string Name { get; private set; }
    [SerializeField] private Transform MassCenter;   
    [SerializeField] private Animator[] wheelsAnimators;
    [Header("Car stats:"), SerializeField] private float initialSpeed;
    [SerializeField] private float maxSpeed, turnSpeed, breakForce;
    [Range(0f, 5f), SerializeField] private int startDelay;
    private float currSpeed;
    [Header("Destination vector")]
    [SerializeField] private Waipoint currWaypoint;
    private Rigidbody rb;
    private Vector3 destination;
    private bool isReached;
    private bool canMove = false;    

    public void allowMovement(bool value)
    {
        StartCoroutine(startDrive(value));
    }  

    private IEnumerator startDrive(bool value)
    {
        yield return new WaitForSeconds(startDelay);
        canMove = value;
        animWheels(value);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = MassCenter.transform.localPosition;
        currSpeed = initialSpeed;
        destination = currWaypoint.transform.position;
    }

    private void Update()
    {       
        if (canMove)
        {                       
            Drive();
        }                
    }     
    private void animWheels(bool value)
    {
        foreach (var anim in wheelsAnimators)
        {
            anim.SetBool("canMove", value);
        }
    }   
    private void Drive()
    {
        if(transform.position != destination)
        {
            Vector3 destVect = destination - transform.position;
            destVect.y = 0;
            float destDir = destVect.magnitude; 
            if(destDir >= breakForce)
            {
                currSpeed += 0.01f;
                if (currSpeed > maxSpeed) currSpeed = maxSpeed;
                isReached = false;
                Quaternion targetRot = Quaternion.LookRotation(destVect);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
                //setSpeed(currSpeed);
                transform.Translate(Vector3.forward * currSpeed * Time.deltaTime);
            }
            else
            {
                isReached = true;
            }
        }
        if(isReached)
        {
            currSpeed = initialSpeed;
            currWaypoint = currWaypoint.next;
            findDest(currWaypoint.getPos());
        }
    }
    private void findDest(Vector3 dest)
    {         
        destination = dest;
        isReached = false;
    }    
}
