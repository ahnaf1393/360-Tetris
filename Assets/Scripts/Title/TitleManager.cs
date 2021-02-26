using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR; 


public class TitleManager : MonoBehaviour
{
    
    public AudioSource buttonAudio;
    public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.GetBooleanAction("InteractUI");
    public SteamVR_Behaviour_Pose pose;

    void Update()
    {
        if (pose == null)
            pose = this.GetComponent<SteamVR_Behaviour_Pose>();
        if (pose == null)
            Debug.LogError("No SteamVR_Behaviour_Pose component found on this object");

        if (interactWithUI == null)
            Debug.LogError("No ui interaction action has been set on this component.");

        if (Input.anyKey || interactWithUI.GetStateUp(pose.inputSource))
        {
            ButtonAudio();
            StartCoroutine(DoChangeScene("Menu", 1f));
        }
    }

    void ButtonAudio()
    {
        buttonAudio.mute = false;
        buttonAudio.Play();
    }

    IEnumerator DoChangeScene(string sceneToChangeTo, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToChangeTo);
    }
}