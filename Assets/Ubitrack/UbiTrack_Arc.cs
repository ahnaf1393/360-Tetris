using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FAR
{
    public class UbiTrack_Arc : MonoBehaviour
    {

        //Save the following components in our scene
        public bool simpleFacade = false;
        public bool ubiTrackComponent = false;
        public bool arc = false;
        public bool arrift = false;

        // Use this for initialization
        void Start()
        {
            saveUbiTrackItems();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Application.LoadLevel(0);
            }

            if (Input.GetKeyDown(KeyCode.F2)) 
            {
                Application.LoadLevel(1);
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                Application.LoadLevel(2);
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                Application.LoadLevel(3);
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                Application.LoadLevel(4);
            }

        }



        void saveUbiTrackItems()
        {

            if (this.arc)
            {
                DontDestroyOnLoad(this.gameObject);
            }

            if (this.simpleFacade)
            {

                UTFacade[] allSimpleFacades = GameObject.FindObjectsOfType(typeof(UTFacade)) as UTFacade[];

                foreach (UTFacade sf in allSimpleFacades)
                {
                    DontDestroyOnLoad(sf.gameObject);
                }
            }

            if (this.ubiTrackComponent)
            {
                UbiTrackComponent[] allUbiTrackComponents = GameObject.FindObjectsOfType(typeof(UbiTrackComponent)) as UbiTrackComponent[];

                foreach (UbiTrackComponent uc in allUbiTrackComponents)
                {
                    DontDestroyOnLoad(uc.gameObject);
                }
            }

            if (this.arrift)
            {
                GameObject ARRift = GameObject.Find("ARRIFT");
                if (ARRift != null)
                {
                    DontDestroyOnLoad(ARRift);
                }
                else
                {
                    Debug.LogError("ARRIFT not found");
                }
            }
        }
    }
}
    
