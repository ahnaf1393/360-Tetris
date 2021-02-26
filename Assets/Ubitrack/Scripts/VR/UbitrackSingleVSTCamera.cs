using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAR;

public class UbitrackSingleVSTCamera : MonoBehaviour {
    
	public bool DisableOpenVRTracking = true;

    public CameraProjectionMatrixFrom3x3Matrix Eye;
    public Transform eyeOffset;

    public Texture CameraImage = null;
	public ImageTextureUpdate CameraTexture;
    
    public Material ARMaterial;
    public string ARMaterialTexName = "_MainTex";

    protected Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();

		UnityEngine.XR.InputTracking.disablePositionalTracking = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        updateVRSettings();

		//transform.localPosition = Vector3.zero;
		//transform.localRotation = Quaternion.identity;

		//UnityEngine.VR.InputTracking.Recenter();

    }

    public void updateVRSettings()
    {


    }

    void OnPreRender()
    //void OnRenderObject()
    {
		
        StereoTargetEyeMask currentEye = cam.stereoTargetEye;

        Matrix4x4 camPose = cam.worldToCameraMatrix;

		Matrix4x4 parentPose = transform.parent.worldToLocalMatrix;

        Matrix4x4 viewOffset = new Matrix4x4();

		Vector3 eyePos = new Vector3 ();
		Quaternion eyeRot = new Quaternion ();
		UbiMeasurementUtils.coordsysemChange (eyeOffset.transform.localPosition, ref eyePos);
		UbiMeasurementUtils.coordsysemChange (eyeOffset.transform.localRotation, ref eyeRot);


		//viewOffset.SetTRS(eyePos, eyeRot, new Vector3(1, 1, -1));
		//viewOffset.SetTRS(eyeOffset.transform.localPosition, eyeOffset.transform.localRotation, new Vector3(1, 1, -1));
		viewOffset.SetTRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1));

		if (DisableOpenVRTracking) {
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}


		Matrix4x4 viewMatrix = new Matrix4x4 ();


		viewMatrix = viewOffset * eyeOffset.worldToLocalMatrix;

		//viewMatrix = viewMatrix.inverse;
	
		//Matrix4x4 newProjectionMatrix = Eye.projectionMatrix * viewOffset;
		Matrix4x4 newProjectionMatrix = Eye.projectionMatrix;

		Matrix4x4 parentMatrix = transform.parent.worldToLocalMatrix;

//		Debug.Log (name + " : "+ cam.GetStereoViewMatrix (Camera.StereoscopicEye.Left).ToString ("0.0000"));

		cam.projectionMatrix = newProjectionMatrix;
		//cam.projectionMatrix = Eye.projectionMatrix;

		transform.localPosition = eyeOffset.transform.localPosition;
		transform.localRotation = eyeOffset.transform.localRotation;

		if (CameraImage == null)
			CameraImage = CameraTexture.getImageTexture ();

        ARMaterial.SetTexture(ARMaterialTexName, CameraImage);

        switch (currentEye)
        {
            case StereoTargetEyeMask.None:
            case StereoTargetEyeMask.Both:
            case StereoTargetEyeMask.Left:
                
			//cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, newProjectionMatrix);
			//cam.SetStereoViewMatrix(Camera.StereoscopicEye.Left, viewOffset);  

                //Matrix4x4 viewLeft = new Matrix4x4();
                //viewLeft.SetTRS(Eye.transform.localPosition, Eye.transform.localRotation, Vector3.one);
			cam.SetStereoViewMatrix(Camera.StereoscopicEye.Left, viewMatrix);
                break;
            case StereoTargetEyeMask.Right:
			//cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, newProjectionMatrix);
			cam.SetStereoViewMatrix(Camera.StereoscopicEye.Right, viewMatrix);  
                break;
            
        }
   
    }

    void OnPostRender()
    {
        //cam.ResetStereoViewMatrices();
        //cam.ResetStereoProjectionMatrices();
    }

}
