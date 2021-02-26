using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRandomMaterial : MonoBehaviour
{
    TetrisSettings settings; 

    [HideInInspector]
    public int mat_idx; 
    // Start is called before the first frame update
    void Start()
    {
        settings = TetrisSettings.getInstance(); 

        if (settings.blockMaterials.Count == 0) // dont forget to add Materials! 
        {
            throw new System.Exception("No materials for tetris blocks available!"); 
        }

        mat_idx = Random.Range(0, settings.blockMaterials.Count);
        gameObject.GetComponent<MeshRenderer>().material = settings.blockMaterials[mat_idx]; 


    }

    //TODO: Maybe change color 
    // Update is called once per frame
    void Update()
    {
        
    }
}
