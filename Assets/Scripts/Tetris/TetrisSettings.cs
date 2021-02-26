using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

//TODO: Organize the values nicely !!! before tweaking !!! <<---- LOL
public class TetrisSettings : MonoBehaviour
{
    public enum TetrisMode { TetrisSimple, Tetris4X, Tetris360, Tetris3D};
    public enum InputMode { Keyboard, Mouse, Vive, ControlCube }; 

    public enum BlockMaterialType { Simple, Glow }; 

    public TetrisMode gameMode = TetrisMode.TetrisSimple;
    public InputMode inputMode = InputMode.Mouse;
    public BlockMaterialType materialType = BlockMaterialType.Glow; 

    public int gridHeight = 10;
    public int gridWidth = 12;

    public float timePerLogicStep = .5f;
    public float speedupPerLevel = .15f;
    public float minTimePerLogicStep = .25f;
    public int difficultyUpdateRate = 5;
    public float userControlledSpeedupFactor = 4f; 
    public float waitUntilSpawningNewBrick = 1.0f;

    public AudioSource brickAudio;
    public AudioSource tetrisAudio;
    public AudioSource gameAudio1;
    public AudioSource gameAudio2;
    public AudioSource moveAudio;


    public List<Material> matsSimple, matsGlow, matsGlow_off; 

    public int pointsPerTetris = 50;


    public GameObject blockPrefab;

    public GameObject cameraRig;
    public GameObject vrController;
    public GameObject artController; 
    public GameObject gridLineRenderer;
    public GameObject gameOverOverlay;
    public Text score; 

    public bool randomSpawnLocations = true; 
    public bool drawGrid = true; 
    public float cellSize = 1f;
    public float blockSize = 1f;

    [HideInInspector]
    public List<Material> blockMaterials;
    [HideInInspector]
    public IInput userInput;


    public GameObject[] themeTetrisAnimations;

    public TetrisSettings self = null; 

    [HideInInspector]
    private static TetrisSettings instance = null;


    public static TetrisSettings getInstance()
    {

        if(instance == null)
        {
            Debug.Log("TetrisSettings.getInstance() called before instance was initialized!");
            //TetrisSettings.instance = self; 
            //instance.Awake(); 
        }

        return instance; 
    }


    // Start is called before the first frame update
    void Awake()
    {
        TetrisSettings.instance = self;

        switch (inputMode)
        {
            case InputMode.Keyboard:
                userInput = new ClassicInput(); 
                break;
            case InputMode.Mouse:
                userInput = cameraRig.GetComponent<MouseInput>();
                if (userInput == null)
                    throw new Exception("Mouse input script not attached to settings script object"); 
                break;
            case InputMode.Vive:
                userInput = vrController.GetComponent<VRInput>();
                if (userInput == null)
                    throw new Exception("Vr input script not attached to settings script object");

                break;
            case InputMode.ControlCube:
                userInput = artController.GetComponent<VRInput>(); 
                if (userInput == null)
                    throw new Exception("Vr input script not attached to settings script object");
                break;
            default:
                break;
        }

        switch (materialType)
        {
            case BlockMaterialType.Simple:
                blockMaterials = matsSimple; 
                break;
            case BlockMaterialType.Glow:
                blockMaterials = matsGlow; 
                break;
            default:
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}