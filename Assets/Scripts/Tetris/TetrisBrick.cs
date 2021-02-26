using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/**
 * Represents the curretnly active brick, whcih consists of blocks. 
 * provides functions for moveing, rotating, etc. 
 */

public class TetrisBrick
{
    public enum BrickShape
    {
        L, Lmirr, Box, Z, Zmirr, Long, Tri, Long_5, L_5, Z_5, L_5_mirr, Plus, Halfaquare, //LongSide, 
    };


    public BrickShape shape; 

    private TetrisBlock[] blocks;

    private TetrisSettings settings;

    private int[] blockOffsets;

    public Vector2Int centerPos;

    private bool selected = true; 

    // Start is called before the first frame update
    public TetrisBrick(TetrisBlock[,] grid, bool selected = false)
    {
        settings = TetrisSettings.getInstance();

        this.selected = selected; 

        int spawnX = settings.gridWidth / 2 + 1; 

        if(settings.randomSpawnLocations)
        {
            spawnX = Random.Range(0, settings.gridWidth); 
        }


        //TODO: Spawn based on player orientation /random
        centerPos = new Vector2Int(spawnX, settings.gridHeight);



        shape = pickRandomShape(); 


        loadShape(shape);
        updateGridIfValidConfig(centerPos, blockOffsets, grid);

    }

    public void select(bool select)
    {
        selected = select; 
        foreach(var b in blocks)
        {
            b.OnSelect(select); 
        }
    }

    public bool isSelected()
    {
        return selected; 
    }

    private BrickShape pickRandomShape()
    {
        //TODO: Add better policy? (i.e. lower chance for long pieces)
        BrickShape s;

        s = (BrickShape) Random.Range(0, ShapeOffsets.numBricks); 

        return s; 

    }

    public bool rotate90Left(TetrisBlock[,] grid)
    {
        return _rotate90(grid, false);
    }

    public bool rotate90Right(TetrisBlock[,] grid)
    {
        return _rotate90(grid, true);         
    }


    public bool moveRight(TetrisBlock[,] grid)
    {
        return updateGridIfValidConfig(new Vector2Int(centerPos.x + 1, centerPos.y), blockOffsets, grid); 
    }
    public bool moveLeft(TetrisBlock[,] grid)
    {
        return updateGridIfValidConfig(new Vector2Int(centerPos.x - 1, centerPos.y), blockOffsets, grid);
    }

    public bool moveToAbsHorizontal(TetrisBlock[,] grid, int x)
    {
        return updateGridIfValidConfig(new Vector2Int(x, centerPos.y), blockOffsets, grid);
    }


    private bool _rotate90(TetrisBlock[,] grid, bool clockwise)
    {

        if (shape == BrickShape.Box)
            return true; 

        int[] newOffsets = new int[blockOffsets.Length];

        //TODO: use vector ops
        for (int i = 0; i < blockOffsets.Length / 2; ++i)
        {

            newOffsets[2 * i] = (clockwise ? 1 : -1) * blockOffsets[2 * i + 1]; // new x
            newOffsets[2 * i + 1] = (clockwise ? -1 : 1) * blockOffsets[2 * i]; // new y

        }


        return updateGridIfValidConfig(centerPos, newOffsets, grid); 
    }


    public bool moveDown(TetrisBlock[,] grid)
    {
        return updateGridIfValidConfig(new Vector2Int(centerPos.x, centerPos.y -1), blockOffsets, grid);
    }

    public bool hasBlockAboveMaxHeight()
    {
        for (int i = 0; i < blocks.Length; ++i)
        {
            int py = blockOffsets[2 * i + 1] + centerPos.y;

            if (py >= settings.gridHeight)
            {
                return true; 
            }
        }
        return false; 
    }

    public void loadShape(BrickShape s)
    {
        shape = s;


        switch (shape)
        {
            case BrickShape.L:
                _loadShape(ShapeOffsets.L);
                break;
            case BrickShape.Lmirr:
                _loadShape(ShapeOffsets.Lmirr);
                break;
            case BrickShape.Box:
                _loadShape(ShapeOffsets.Box);
                break;
            case BrickShape.Z:
                _loadShape(ShapeOffsets.Z);
                break;
            case BrickShape.Zmirr:
                _loadShape(ShapeOffsets.Zmirr);
                break;
            case BrickShape.Long:
                _loadShape(ShapeOffsets.Long);
                break;
            case BrickShape.Long_5:
                _loadShape(ShapeOffsets.Long_5);
                break;
            case BrickShape.L_5:
                _loadShape(ShapeOffsets.L_5);
                break;
            case BrickShape.Z_5:
                _loadShape(ShapeOffsets.Z_5);
                break;
            case BrickShape.Tri:
                _loadShape(ShapeOffsets.Tri);
                break;
            case BrickShape.L_5_mirr:
                _loadShape(ShapeOffsets.L_5_mirr);
                break;
            //case BrickShape.Longside:
            //    _loadShape(ShapeOffsets.Longside);
            //    break;
            case BrickShape.Plus:
                _loadShape(ShapeOffsets.Plus);
                break;
            case BrickShape.Halfaquare:
                _loadShape(ShapeOffsets.Halfaquare);
                break;
            default:
                break;
        }
    }

    // spawns blocks based on selected shape
    private void _loadShape(int[] shapeOffsets)
    {

        if((shapeOffsets.Length %2 )!=0)
        {
            throw new System.Exception("Shape Offsets must have even number of elements! (x1y1x2y2...)"); 
        }

        this.blocks = new TetrisBlock[shapeOffsets.Length/2];
        blockOffsets = (int[])shapeOffsets.Clone();


        if (settings.blockMaterials.Count == 0) // dont forget to add Materials! 
        {
            throw new System.Exception("No materials for tetris blocks available!");
        }

        int materialIndex = Random.Range(0, settings.blockMaterials.Count);

        for (int i = 0; i< blocks.Length; ++i)
        {
            blocks[i] = GameObject.Instantiate(settings.blockPrefab).GetComponent<TetrisBlock>();
            blocks[i].mat_idx = materialIndex; 
            blocks[i].GetComponent<MeshRenderer>().material = settings.blockMaterials[materialIndex];


        }
    }

    // move and rotate describe, how the blocks would change, this function checks, if the changes are valid, if so it performs them
    // returns true of it could perform the update, else false
    bool updateGridIfValidConfig(Vector2Int newCenterPos, int[] newOffsets,  TetrisBlock[,] grid)
    {
        for (int i = 0; i < newOffsets.Length / 2; ++i)
        {
            //new positions in grid
            int px = newOffsets[2 * i] + newCenterPos.x ;

            int py = newOffsets[2 * i+1] + newCenterPos.y;
 

            if(px < 0 || px >= settings.gridWidth)
            {
                switch (settings.gameMode)
                {
                    case TetrisSettings.TetrisMode.TetrisSimple:
                        return false; 
                    case TetrisSettings.TetrisMode.Tetris4X:
                        throw new NotImplementedException(); 
                        break;
                    case TetrisSettings.TetrisMode.Tetris360:
                        {
                            px = (px + settings.gridWidth)% settings.gridWidth;
                            break;
                        }
                    case TetrisSettings.TetrisMode.Tetris3D:
                        throw new NotImplementedException(); 
                        break;
                    default:
                        break;
                }
            }


            if (py < 0)
            {
                return false; 
            }
            else if (py >= settings.gridHeight)
            {
                continue; 
            }

           // Debug.Log(px + "/" + py); 


            if (grid[px, py]!= null && grid[px, py].state == TetrisBlock.BlockState.Placed)
            {
                return false; 
            }
        }

        //Delete Blocks from old Positions
        for(int i = 0; i < blocks.Length; ++i)
        {
            int px = blockOffsets[2 * i] + centerPos.x;
            int py = blockOffsets[2 * i + 1] + centerPos.y;

            if(settings.gameMode == TetrisSettings.TetrisMode.Tetris360)
            {
                px = (px+settings.gridWidth)% settings.gridWidth; 
            }


            if(py < settings.gridHeight && px < settings.gridWidth)
            {
                grid[px, py] = null;
            }
        }


        this.centerPos = newCenterPos;
        this.blockOffsets = newOffsets;


        //readd blocks on new positions
        for (int i = 0; i < blocks.Length; ++i)
        {
            int px = newOffsets[2 * i] + newCenterPos.x;
            int py = newOffsets[2 * i + 1] + newCenterPos.y;
            if (py > settings.gridHeight)
            {
                continue;
            }

            if (settings.gameMode == TetrisSettings.TetrisMode.Tetris360)
            {
                px = (px + settings.gridWidth) % settings.gridWidth;
            }

            blocks[i].syncGameobjectToGrid(new Vector2Int(px,py));
    //        Debug.Log("Adding@: " + px + "/" + py); 
            grid[px, py] = blocks[i];

        }

        return true; 
    }

    // if a brick is moved down and hits a block of a previously placed brick, or the ground it is placed and a new brick is spawned. 
    public void setPlaced(TetrisBlock[,] grid)
    {
        for (int i = 0; i < blocks.Length; ++i)
        {
            //new positions in grid
            int px = blockOffsets[2 * i] + centerPos.x;
            int py = blockOffsets[2 * i + 1] + centerPos.y;

            if (settings.gameMode == TetrisSettings.TetrisMode.Tetris360)
            {
                px = (px + settings.gridWidth) % settings.gridWidth;
            }

            if(py>=settings.gridHeight)
            {
                continue; 
            }

            grid[px, py].state = TetrisBlock.BlockState.Placed; 
            
        }
    }



}
