using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRKeys; 

public class EnterScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnSubmitName(string name)
    {

        Debug.Log("SUBMIT:" + name);
        name.Replace(";", " ");

        //ScoreManager.getInstance().addEntry(playerName, finalScore);
        int score = PlayerPrefs.GetInt("score");


        ScoreManager.getInstance().addEntry(name, score); 


        SceneManager.LoadScene("Menu");

    }


    public void OnCancel()
    {
        SceneManager.LoadScene("Menu");
    }

}
