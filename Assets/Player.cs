using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed=10f;
    [SerializeField] float turnSpeed=1f;
    float xInput;
    float yInput;
   

    float turnInputx;
    float turnInputy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xInput=Input.GetAxis("Horizontal");
        yInput=Input.GetAxis("Vertical");
        transform.Translate(new Vector3(xInput,0,yInput)*movementSpeed*Time.deltaTime);

        turnInputx=Input.GetAxis("Mouse X");
        turnInputy=Input.GetAxis("Mouse Y");
        

        transform.RotateAround(transform.position,Vector3.up*3,turnInputx*turnSpeed*Time.deltaTime);
        //transform.RotateAround(transform.position,Vector3.right*1.5f,turnInputy*turnSpeed*Time.deltaTime);
    }
    
}
