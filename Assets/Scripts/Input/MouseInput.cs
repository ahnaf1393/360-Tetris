using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

class MouseInput : MonoBehaviour, IInput
{

    public float moveTriggerDelta = 1f;
    public float moveTriggerFrequence = 1f;
    public float xClampMin = -65;
    public float xClampMax = 40; 

    float prevMouseX;
    float deltaMouseX;

    float lastMoveEvent;

    public float moveSpeed = 5;
    public float lookSpeed = 3;
    private Vector2 rotation = Vector2.zero;

    private bool select = false;
    private bool select_toggle = false;
    private bool hold = false, hold_toggle = false; 

    void Start()
    {
        lastMoveEvent = Time.time; 
        prevMouseX =Input.GetAxis("Mouse X"); 
    }


    void Update()
    {
        select = false;
        hold = false; 
        float mouseX = Input.GetAxis("Mouse X");

        deltaMouseX = (mouseX - prevMouseX) * Time.deltaTime;
        prevMouseX = mouseX;
        
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, xClampMin, xClampMax);
        var qr = Quaternion.Euler(rotation.x, rotation.y, 0);

        transform.rotation =  Quaternion.Slerp(transform.rotation, qr, lookSpeed * Time.deltaTime); //Smooth Update

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero; 
        if(Input.GetKey(KeyCode.W))
        {
            rb.velocity += transform.forward * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity -= transform.forward * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity -= transform.right * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity += transform.right * moveSpeed;
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            select = true; 
            select_toggle = !select_toggle;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            hold = true;
            hold_toggle = !hold_toggle; 
        }
       

    }


    public bool isInputMoveDown()
    {
        return Input.GetMouseButtonDown(3) || Input.GetKeyDown(KeyCode.Space); //MMB
    }

    public bool isInputRotateLeft()
    {
        return Input.GetMouseButtonDown(0); //LMB
    }

    public bool isInputRotateRight()
    {
        return Input.GetMouseButtonDown(1); //RMB
    }

    public bool isMoveLeft()
    {
        //if (-deltaMouseX < moveTriggerDelta && Time.time - lastMoveEvent > moveTriggerFrequence)
        //{
        //    lastMoveEvent = Time.time;
        //    return true;
        //}
        //return false; 
        return Input.GetKeyDown(KeyCode.Q); 
    }

    public bool isMoveRight()
    {
        //if (deltaMouseX > moveTriggerDelta && Time.time - lastMoveEvent > moveTriggerFrequence)
        //{
        //    lastMoveEvent = Time.time;
        //    return true;
        //}
        //return false;
        return Input.GetKeyDown(KeyCode.E);

    }
    public int getAbsHorizontalPosition()
    {
        return -1;
    }

    public bool isSelectBrick()
    {
        if(select)
        {
            return select_toggle; 
        }
        return false; 
    }


    public bool isDeselectBrick()
    {
        if (select)
        {
            return !select_toggle;
        }
        return false;
    }

    public bool isHoldBrick() {
        if (hold)
        {
            return hold_toggle;
        }
        return false;
    }
    public bool isPause()
    {
        if (hold)
        {
            return !hold_toggle;
        }
        return false;
    }



}

