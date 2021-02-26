using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Valve.VR.Extras;
//using VRKeys; 

public class GameOverManager : MonoBehaviour
{
    public Text finalScoreField;
    public Text playerNameField;

    private int finalScore = -1; 
    private bool submit = false;
    private string playerName = "";

    public VRUIInput[] controllers;
    //public Keyboard keyboard;
    //public Camera uiCam;
    //public Transform keyboardAnchor;

    private void Start()
    {
        finalScoreField.text = "";

        //DEBUG
        //setupKeyboard(); 
    }

    public void setFinalScore(int score)
    {
        finalScoreField.text = string.Format("Score: {0,6}", score);
        finalScore = score; 

        
        foreach(var c in controllers)
        {
            c.enabled = true; 
        }
        // setupKeyboard(); 

    }

    private void Update()
    {

    }

    void resetTransform(Transform t)
    {
        t.position = Vector3.zero;
        t.localPosition = Vector3.zero;
        t.rotation = Quaternion.identity;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one; 
    }

    public void OnSubmitName()
    {
        submit = true;
        playerName = playerNameField.text;


        //ScoreManager.getInstance().addEntry(playerName, finalScore);
        PlayerPrefs.SetInt("score", finalScore); 
        SceneManager.LoadScene("EnterName"); 

    }

    //private void setupKeyboard()
    //{
    //    keyboard.Enable();
    //    keyboard.SetPlaceholderMessage("Enter Name");
    //}

}
