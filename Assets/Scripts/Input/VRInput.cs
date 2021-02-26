using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valve.VR;


class VRInput : MonoBehaviour, IInput
{
    private Vector3 prevPos;
    private Quaternion prevRot;

    private Vector3 startPos;
    private Quaternion startRot;

    public GameObject head; 

    public float rotateLeftThreshold = 52.5f;
    public float rotateRightThreshold = 45.0f;
    public float rotateDownThreshold = 50.0f;
    public float rotateUpThreshold = -77.5f;

    public Vector3 selectVelocityThreshold = Vector3.one; 

    public float resetTime = .5f;

    private float lastAction;
    public int horizontalOffset = 0;

    public bool useARTCoordinates = false;
    public float art_translation_scale = 10; 
  

    private InputFlags inputFlags = new InputFlags();

    private TetrisSettings settings;
    private bool restPose = true;

    public GameObject restPoseIndicator; 
 


    public void Start()
    {
        GameObject go = new GameObject();

        Transform old = go.transform;
        old.position = transform.position;
        old.rotation = transform.rotation;
        if (useARTCoordinates)
        {
            //    transform.Rotate(-90, 0, 0);
            //    transform.position *= art_translation_scale;
        }
        settings = TetrisSettings.getInstance();

        startPos = transform.localPosition;
        startRot = transform.rotation;
        lastAction = Time.time;
        //transform.position = old.position;
        //transform.rotation = old.rotation; 
    }

    public void Update()
    {
        GameObject go = new GameObject();

        Transform old = go.transform; 
        old.position = transform.position;
        old.rotation = transform.rotation;
        if (useARTCoordinates)
        {
            //transform.Rotate(-90, 0, 0);
            //transform.position *= art_translation_scale;

        }

        Vector3 deltaPos;
        Vector3 deltaRot;

        deltaPos = transform.localPosition - prevPos;
        deltaRot = transform.rotation.eulerAngles - prevRot.eulerAngles;
        prevPos = transform.localPosition;
        prevRot = transform.rotation;

        Vector3 velocity = deltaPos / Time.deltaTime; 

        inputFlags.setZero();

        Quaternion relativeRotation = transform.rotation;// * Quaternion.Inverse(startRot);
                                                         //  relativeRotation = transform.rotation; 

        // Debug.Log(Vector3.Dot(velocity, transform.forward.normalized)); 

        if(useARTCoordinates)
        {
            Vector3 eulers = relativeRotation.eulerAngles;
            eulers.x *= -1;
            relativeRotation = Quaternion.Euler(eulers); 
        }

        if (relativeRotation.eulerAngles.x > rotateDownThreshold && relativeRotation.eulerAngles.x < 120)
        {
          //  Debug.Log("MoveDown");

            inputFlags.moveDown = true;

        }

        if (Time.time - lastAction > resetTime)
        {
            // giant else if chaing -> no if -> controller is in rest pose


            if (relativeRotation.eulerAngles.x < rotateUpThreshold && relativeRotation.eulerAngles.x > -130)
            {
                Debug.Log("Pause");
                if (restPose)
                {
                    inputFlags.pause = true;
                }
            }
            //FORWARD / BACKWARD --> select/unselect
            //Debug.Log("fwd velocity: " + Vector3.Dot(velocity, transform.forward.normalized));
            else if (Vector3.Dot(velocity, transform.forward.normalized) * (useARTCoordinates?-1:1) > selectVelocityThreshold.z) 
            {
                Debug.Log("Brick Selected w/ velocity: " + Vector3.Dot(velocity, transform.forward.normalized));

                if (restPose)
                {
                    inputFlags.selectBrick = true; 
                }
            }
            //remóve deselect gesture, as it isnt really used
            //else if (Vector3.Dot(velocity, transform.forward.normalized) < -selectVelocityThreshold.z)
            //{
            //    Debug.Log("Brick DeSelected w/ velocity: " + Vector3.Dot(velocity, transform.forward.normalized));

            //    if (restPose)
            //    {
            //        inputFlags.deselectBrick = true;
            //    }
            //}
            //UP / DOWN --> move faster
            else if (Vector3.Dot(velocity, transform.up.normalized) > selectVelocityThreshold.y)
            {
                if (restPose)
                {

                    inputFlags.selectBrick = true;
                }
            }
            
            //else if (Vector3.Dot(velocity, transform.up.normalized) < -selectVelocityThreshold.y)
            //{
            //    Debug.Log("Brick MovedDown w/ velocity: " + Vector3.Dot(velocity, transform.up.normalized));

            //    if (restPose)
            //    {
            //        inputFlags.moveDown = true;
            //    }
            //}
            //ROT L/R --> rotate active Brick 
            else if (relativeRotation.eulerAngles.z > rotateLeftThreshold && relativeRotation.eulerAngles.z < 120)
            {
                Debug.Log("Rotate Left"); 
                if (restPose)
                {
                    inputFlags.rotateLeft = true;
                }
            }
            else if (relativeRotation.eulerAngles.z < (360 - rotateRightThreshold) && relativeRotation.eulerAngles.z > 210)
            {
                Debug.Log("Rotate Right");

                if (restPose)
                {
                    inputFlags.rotateRight = true;
                }
            }
            //ROT X axis forward: hold brick, backward: release brick
            //bugged TODO: figure out angles
            //else if (relativeRotation.eulerAngles.x > rotateLeftThreshold && relativeRotation.eulerAngles.z < 120)
            //{
            //    Debug.Log("Hold"); 
            //    if (restPose)
            //    {
            //        inputFlags.isHoldBrick = true;
            //    }
            //}
            
            else 
            {
                restPose = true; 
            }

            if(inputFlags.any())
            {
                lastAction = Time.time;
                restPose = false;
            }

        }


        if(restPoseIndicator)
        {
            restPoseIndicator.SetActive(restPose && Time.time - lastAction > resetTime); 
        }

        //transform.position = old.position;
        //transform.rotation = old.rotation;
    }

    public bool isInputMoveDown()
    {
        return inputFlags.moveDown;
    }

    public bool isSelectBrick()
    {
        //return true; //TEST
        return inputFlags.selectBrick; 
    }

    public bool isInputRotateLeft()
    {
        return inputFlags.rotateLeft; 
    }

    public bool isInputRotateRight()
    {
        return inputFlags.rotateRight; 
    }

    public bool isMoveLeft()
    {
        return inputFlags.moveLeft; 
    }

    public bool isMoveRight()
    {
        return inputFlags.moveRight; 
    }

    public bool isDeselectBrick()
    {
         return inputFlags.deselectBrick;
    }

    public bool isHoldBrick()
    {
        return inputFlags.isHoldBrick; 
    }

    public bool isPause()
    {
        return inputFlags.pause; 
    }

    // returns the x coord of grid cell the user is pointing at
    public int getAbsHorizontalPosition()
    {
        //Vector3 pos = head.transform.position - transform.position; 
        //Vector2 controlerPosXZ = new Vector2(pos.x, pos.z).normalized;
        Quaternion relativeRotation = transform.rotation * Quaternion.Inverse(startRot);

        float theta = relativeRotation.eulerAngles.y; 
        float anglePerBlock = 360.0f / settings.gridWidth;
        float section = (theta - anglePerBlock / 2.0f) / anglePerBlock;

        //Debug.DrawRay(transform.position, transform.forward);
        //Debug.Log("theta: " + theta);
        //Debug.Log("anglePerBlock:" + anglePerBlock); 
        //Debug.Log("sec: " + section); 
        return (Mathf.FloorToInt(section) + horizontalOffset) % settings.gridWidth;
    }



}

