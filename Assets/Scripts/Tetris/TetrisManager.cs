using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/**
 * * Main game logic and animation in here
*/
public class TetrisManager : MonoBehaviour
{
    public bool DEBUG = false; 
    public GameOverManager gameOverManager; 

    private TetrisBlock[,] tetrisArray;
   

    private TetrisBrick activeBrick = null;


    private TetrisSettings settings = null;
    private MusicSelection selection = null;

    private int score = 0;
    private int completedLayers = 0; 

    private float lastLogicStep;
    private bool updateLogic = true;
    private float logicSpeed;

    public bool userSpeedup = false; 

    // Start is called before the first frame update    
    void Start()
    {
        settings = TetrisSettings.getInstance();
        selection = MusicSelection.getInstance();

        Debug.Log("Here " + selection.GetIndex());
        logicSpeed = settings.timePerLogicStep; 
        if(selection.GetIndex() == 0)
        {
            settings.gameAudio1.mute = false;
            settings.gameAudio1.Play();
        }
        else
        {
            settings.gameAudio2.mute = false;
            settings.gameAudio2.Play();
        }

        switch (settings.gameMode)
        {
            case TetrisSettings.TetrisMode.TetrisSimple:
                {
                    break; 
                }
            case TetrisSettings.TetrisMode.Tetris360:
                {
                    settings.gridWidth = settings.gridWidth * 4;

                    break; 
                }

            default:
                {
                    throw new System.Exception("Those lazy students have not implemented this gamemode yet!"); 
                }

        }

        if(settings.drawGrid)
        {
            DrawGrid(); 
        }

        lastLogicStep = Time.time; 
        tetrisArray = new TetrisBlock[settings.gridWidth, settings.gridWidth];

        updateLogic = false; 
        //for (int y = 0; y<settings.gridHeight; ++y)
        //    for (int x = 0; x<settings.gridWidth; ++x)
        //    {
        //        tetrisArray[x, y] = new TetrisCell(TetrisCell.CellState.Empty); 
        //    }


    }

    // Update is called once per frame
    void Update()
    {
        AudioUpdate();
        updateDifficulty(); 

        settings.score.text = string.Format("Score: {0,6}", score);

        // start game after ready set go 
        if (GameObject.Find("ReadySetGoManager").GetComponent<ReadySetGoManagerScript>().ReadySetGoDone)
        {
            updateLogic = true; 
        }

        //DEBUG keyboard input
        if (DEBUG)
        {
            if(Input.GetKey(KeyCode.T)) // to test animations
            {
                StartCoroutine(OnTetris(new Queue<int>()));

            }
            if(Input.GetKeyDown(KeyCode.C)) // to test speedup
            {
                ++completedLayers; 
            }
            if (Input.GetKeyDown(KeyCode.G)) // to test speedup
            {
                OnGameOver(); 
            }
        }

        // game loop 
        if(updateLogic && ! PauseMenu.GameIsPaused)
        {
            //Debug.Log("woo");

            userSpeedup = false;

            if (activeBrick != null) // user can only interact with brick, if there is a brick (there is no brick for a few secs, averytime a brick is placed. new bricks are assingned in UpdateLogic()
            {
                // User input

                if (settings.userInput.isSelectBrick())
                {
                    activeBrick.select(true);
                }
                else if(settings.userInput.isDeselectBrick())
                {
                    Debug.Log("deselect"); 
                    activeBrick.select(false); 
                }

                if (settings.userInput.isInputMoveDown())
                {
                    MoveAudio();
                    userSpeedup = true; 
                }


                if (activeBrick.isSelected() && settings.userInput.isInputRotateLeft())
                {
                    MoveAudio();
                    activeBrick.rotate90Left(tetrisArray);
                }
                if (activeBrick.isSelected() && settings.userInput.isInputRotateRight())
                {
                    MoveAudio();
                    activeBrick.rotate90Right(tetrisArray);
                }

                if (activeBrick.isSelected() && settings.userInput.isMoveLeft())
                {
                    MoveAudio();
                    activeBrick.moveLeft(tetrisArray);
                }

                if (activeBrick.isSelected() && settings.userInput.isMoveRight())
                {
                    MoveAudio();
                    activeBrick.moveRight(tetrisArray);
                }

                if(activeBrick.isSelected())
                {
                    var absX = settings.userInput.getAbsHorizontalPosition();
                    if (absX != -1 && absX != activeBrick.centerPos.x) // track brick to where user points w/ cube
                    {
                        bool success = activeBrick.moveToAbsHorizontal(tetrisArray, absX);
                        if (!success) // "drop brick"
                        {
                            Debug.Log("Dropped brick");
                            activeBrick.select(false);
                        }
                    }
                }

            }

            //game logic timer
            if (lastLogicStep + logicSpeed / (userSpeedup ? settings.userControlledSpeedupFactor : 1) < Time.time)
            {
                lastLogicStep = Time.time;
                bool gameOver = LogicUpdate();

                if (gameOver)
                {
                    OnGameOver(); 
                }

            }
        }
    }


    void OnGameOver()
    {
        Debug.Log("Game Over!");
        updateLogic = false;
        gameOverManager.setFinalScore(score);  // Triggers GameOvermanager to do all the end scene stuff on its side! 
        gameOverManager.gameObject.SetActive(true);
    }

    //increases speed based on completed levels
    void updateDifficulty()
    {
        int lvl = completedLayers / settings.difficultyUpdateRate;

        logicSpeed = Mathf.Max(settings.minTimePerLogicStep, settings.timePerLogicStep - lvl * settings.speedupPerLevel);
    }

    void AudioUpdate()
    {
        if(activeBrick != null)
        {
            settings.brickAudio.mute = false;
            settings.brickAudio.gameObject.transform.position = TetrisBlock.getWorldPosForCell( activeBrick.centerPos.x, activeBrick.centerPos.y, 0, settings); 
        }
        else
        {
            settings.brickAudio.mute = true; 
        }

    }

    void MoveAudio()
    {
        settings.moveAudio.mute = false;
        settings.moveAudio.Play();
    }

    /**
     * core logic: 
     * Check whether a new brick needs to be spawned
     * chek whether active brick has been placed 
     * check if a line has been completed
     * check if gameover
    */
    bool LogicUpdate()
    {
        bool gameOver = false; 
       
        //  Debug.Log("Current Grid: \n" +logGrid()); 

        if (activeBrick == null) //if brick is placed -> spawn new brick in next logic step
        {

            activeBrick = new TetrisBrick(tetrisArray);
            activeBrick.select(false); 
        }

            

        Queue<int> tetrisLayers = checkForTetris(); 

        if(tetrisLayers.Count!=0) // at least one line completed
        {
            StartCoroutine(OnTetris(tetrisLayers)); 

        }

        if (!activeBrick.moveDown(tetrisArray)) //at least 1 block has another block beneath -> place brick
        {
            if(activeBrick.hasBlockAboveMaxHeight()) //place brick, while it is aboce the playing filed-> gameover
            {
                gameOver = true; 
            }
            activeBrick.select(false);
            activeBrick.setPlaced(tetrisArray);
            activeBrick = null;

            score += Random.Range(15, 25 + 1); // Rnd reward after every brick, because why not... 
            
        }

        return gameOver; 

    }

    public void setLogicActive(bool active)
    {
        updateLogic = active; 
    }

    public void suspendLogicForSeconds(float time)
    {
        if(!updateLogic)
        {
            Debug.Log("SuspendLigoc dalled, while logic was allready suspended!");
        }
        StartCoroutine(_suspendLogicForSeconds(time)); 

    }

    private IEnumerator _suspendLogicForSeconds(float time)
    {
        updateLogic = false;

        yield return new WaitForSeconds(time); 

        updateLogic = true; 

        yield return null; 
    }

    // suspend logic and perform animations if player completes a line
    private IEnumerator OnTetris(Queue<int> tetrisLayers)
    {
        updateLogic = false;
        
        settings.tetrisAudio.Play();
        foreach (int y in tetrisLayers) // no dequeueing happening here! 
        {
            for (int x = 0; x < settings.gridWidth; ++x)
            {
                tetrisArray[x, y].setAnimationsEnabled(true);
                tetrisArray[x, y].OnSelect(true); // make glowy texture light up on tetris
            }
        }

        foreach(var o in settings.themeTetrisAnimations)
        {
            o.SetActive(true);
            ParticleSystem ps = o.GetComponent<ParticleSystem>(); 
            if(ps)
            {
                ps.Play(); 
            }
        }

        Debug.Log("Tetris!" + tetrisLayers.Count);
        //Debug.Log("Before Consolidate: \n" + logGrid());

        yield return new WaitForSeconds(settings.tetrisAudio.clip.length);

        updateLogic = true;
        consolidateLayers(tetrisLayers);
        //Debug.Log("After Consolidate: \n" + logGrid());
        score += scoreFunction(tetrisLayers.Count);
        completedLayers += tetrisLayers.Count; 

        foreach (int y in tetrisLayers) // disabling animation, just in case... 
        {
            for (int x = 0; x < settings.gridWidth; ++x)
            {
                tetrisArray[x, y].setAnimationsEnabled(false);
            }
        }
        foreach (var o in settings.themeTetrisAnimations)
        {
            ParticleSystem ps = o.GetComponent<ParticleSystem>();
            if (!ps || ps.main.loop) // let PS that play only once run out and stop loops
            {
                o.SetActive(false);
            }
        }
        yield return null;
    }


    /*
     *returns # of completed lines in current playing field 
     */
    Queue<int> checkForTetris()
    {
        bool tetris = false;
        Queue<int> tetrisLayers = new Queue<int>();
        for(int y = 0; y< settings.gridHeight; ++y)
        {
            tetris = true;
            for (int x = 0; x < settings.gridWidth; ++x)
            {
                if (tetrisArray[x, y] == null || tetrisArray[x, y].state != TetrisBlock.BlockState.Placed)
                {
                    tetris = false;
                    break; 
                }
            }
            if(tetris)
            {
                tetrisLayers.Enqueue(y); 
            }

        }

        return tetrisLayers; 
    }

    int scoreFunction(int tetrisLayers)
    {
        return tetrisLayers * tetrisLayers * settings.pointsPerTetris; 
    }


    /*
     * Called after tetris has occured
     * moves blocks down, if tetris happened below them 
     * tetrisLayers is sorted by increasing layerNr
    */
    void consolidateLayers(Queue<int> tetrisLayers)
    {
        if(tetrisLayers.Count == 0) // sanity check
        {
            Debug.LogWarning("consolidateLayers() should not be called when no tetris has occured!");
            return; 
        }

        int skipLayers = 0; // move stones n layers down, where n = number of tetris layers below current level

        for(int y = tetrisLayers.Peek(); y!= settings.gridHeight; ++y) // start at lowest tetris, everything below remains unchanged
        {
            int y_tetris = y; 
            while(tetrisLayers.Count!=0 && y_tetris == tetrisLayers.Peek()) //covers multiple tetris layers above each other
            {
                tetrisLayers.Dequeue();
                ++skipLayers;

                for (int x = 0; x < settings.gridWidth; ++x) // delete tetris layers
                {
                    if (tetrisArray[x, y_tetris] == null) // sanity check... all cells must be occupied in tetris layer
                    {
                        Debug.LogError("tetrisarray[" + x + "," + y + "] == null in tetris layer!"); 
                    }
                    Destroy(tetrisArray[x, y_tetris].gameObject);
                    tetrisArray[x, y_tetris] = null; 
                    
                }
                ++y_tetris; 
            }

            if (y + skipLayers >= settings.gridHeight)
            {
                for (int x = 0; x < settings.gridWidth; ++x) // update top layers(empty)
                {
                    tetrisArray[x, y] = null;
                }
            }
            else
            {
                for (int x = 0; x < settings.gridWidth; ++x) // move down layers
                {


                    tetrisArray[x, y] = tetrisArray[x, y + skipLayers];

                    if(tetrisArray[x,y]!=null)
                    {
                        tetrisArray[x, y].syncGameobjectToGrid(x, y); 
                    }

                }
            }
        }
    }


    // Debug: returns playing field as string
    string logGrid()
    {
        string str = "";

        for(int y = settings.gridHeight-1; y >=0 ; --y)
        {
            for (int x = 0; x < settings.gridWidth; ++x)
            {
                str += tetrisArray[x, y] == null ? " " :  tetrisArray[x, y].state == TetrisBlock.BlockState.Active ? "A" : "P";
                str += " "; 
            }
            str += "\n"; 
        }

        return str; 
    }

    // draws lines of the grid hinting the player where blocks are
    void DrawGrid()
    {

        switch (settings.gameMode)
        {
            case TetrisSettings.TetrisMode.TetrisSimple:

                //Vertical Lines
                for (int x = 0; x < settings.gridWidth; ++x)
                {
                    Vector3 start = TetrisBlock.getWorldPosForCell(x,0, 0, settings, false);
                    Vector3 end = TetrisBlock.getWorldPosForCell(x, settings.gridHeight+1, 0, settings, false); 
                    DrawLine(start, end);
                }
                //Horizontal Lines
                for (int y = 0; y < settings.gridWidth; ++y)
                {
                    Vector3 start = TetrisBlock.getWorldPosForCell(0, y, 0, settings, false);
                    Vector3 end = TetrisBlock.getWorldPosForCell(settings.gridWidth+1, y, 0, settings, false);
                    DrawLine(start, end);
                }


                break;
            case TetrisSettings.TetrisMode.Tetris4X:
                break;
            case TetrisSettings.TetrisMode.Tetris360:
                //Vertical Lines
                settings.gridWidth += 4; 
                for (int x = 0; x < settings.gridWidth; ++x)
                {
                    Vector3 v0 = TetrisBlock.getWorldPosForCell(x, 0, 0, settings);
                    Vector3 v1 = TetrisBlock.getWorldPosForCell(x, settings.gridHeight, 0, settings);
                    v0.y -= settings.cellSize / 2;
                    v1.y -= settings.cellSize / 2; 
                    DrawLine(v0, v1);
                }
                //Horizontal Lines
                for (int y = 0; y < settings.gridHeight+1; ++y)
                {
                    Vector3 v0 = TetrisBlock.getWorldPosForCell(0, y, 0, settings);
                    Vector3 v1 = TetrisBlock.getWorldPosForCell(settings.gridWidth/4 , y, 0, settings);
                    Vector3 v2 = TetrisBlock.getWorldPosForCell(settings.gridWidth/2, y, 0, settings);
                    Vector3 v3 = TetrisBlock.getWorldPosForCell((settings.gridWidth/4)*3 , y, 0, settings);
                    v0.y -= settings.cellSize / 2;
                    v1.y -= settings.cellSize / 2;
                    v2.y -= settings.cellSize / 2;
                    v3.y -= settings.cellSize / 2;
                    DrawLine(v0, v1);
                    DrawLine(v1, v2);
                    DrawLine(v2, v3);
                    DrawLine(v3, v0);
                }
                settings.gridWidth -= 4; 
                break;
            case TetrisSettings.TetrisMode.Tetris3D:
                break;
            default:
                break;
        }
    }

    

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject myLine = Instantiate(settings.gridLineRenderer);
        myLine.transform.position = start;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
