using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour, IAnimation
{

    public Vector3 axis = Vector3.up; 
    public float speed =2.5f;


    private float angle = 0; 
    public bool clockwise = true; 

    void Start()
    {
        axis = axis.normalized; 
    }

    // Update is called once per frame
    void Update()
    {
        angle += clockwise ? 1 : -1 * speed * Time.deltaTime;


        if(angle >360) // lazy
        {
            angle -= 360; 
        }else if (angle < -360)
        {
            angle += 360;
        }

        transform.rotation = Quaternion.AngleAxis(angle, axis); 

    }

}
