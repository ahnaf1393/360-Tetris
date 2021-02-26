using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class HighScoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI highscore; 
    // Start is called before the first frame update
    void Start()
    {
        highscore = GetComponent<TextMeshProUGUI>(); 
        highscore.text = ScoreManager.getInstance().getTopPlayerString(5); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
