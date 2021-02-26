using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{

    public enum BlockState { Active, Placed };
    public BlockState state;
    TetrisSettings settings = null;

    [HideInInspector]
    public int mat_idx = 0; 




    void Awake()
    {
        settings = TetrisSettings.getInstance();
        transform.localScale = Vector3.one * settings.blockSize; 

        state = BlockState.Active;

        //comment this out to see animations on all blocks (i.e. for testing) 
        setAnimationsEnabled(false); 
    }



    public void OnSelect(bool select)
    {
        if(settings.materialType == TetrisSettings.BlockMaterialType.Glow)
        {
            if (select)
            {
                gameObject.GetComponent<MeshRenderer>().material = settings.blockMaterials[mat_idx];
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = settings.matsGlow_off[mat_idx];
            }
        }
    }


    public void setAnimationsEnabled(bool enableAnimations)
    {
        foreach(MonoBehaviour a in GetComponents<IAnimation>())
        {
            a.enabled = enableAnimations; 
        }

    }

    public void syncGameobjectToGrid(Vector2Int index)
    {
        syncGameobjectToGrid(index.x, index.y); 
    }

    public void syncGameobjectToGrid(int ix, int iy, int iz=0)
    {
        if(iy >= settings.gridHeight)
        {
            transform.position = new Vector3(0, -1000, 0); 
        }
        else 
            transform.position = getWorldPosForCell(ix,iy,iz, settings);
    }

    public Vector3 getWorldPosForCell(Vector3Int idx)
    {
        return getWorldPosForCell(idx.x, idx.y, idx.z);
    }
    public Vector3 getWorldPosForCell(int x, int y, int z)
    {
        return getWorldPosForCell(x, y, z, settings);
    }

    // computed position of a block in 3d spaced based on its coordinates (z is unused -> 3D Tetris) and the size of a cell and the current game mode 
    // 
    public static Vector3 getWorldPosForCell(int x, int y, int z, TetrisSettings settings, bool center= true)
    {
        Vector3 pos = Vector3.zero; 

        switch (settings.gameMode)
        {
            case TetrisSettings.TetrisMode.TetrisSimple:
                pos = new Vector3(
            x * settings.cellSize,
            y * settings.cellSize,
            z);
                break;
            case TetrisSettings.TetrisMode.Tetris4X:
                throw new System.NotImplementedException(); 
                break;
            case TetrisSettings.TetrisMode.Tetris360:
                float py = y;
                float px = 0, pz =0;

                int sideWitdh = settings.gridWidth / 4; 
                int quadrant = (x / sideWitdh)%4;
                float walloffset = sideWitdh / 2.0f ;
                float x_side = (x % sideWitdh) - walloffset;


                switch (quadrant)
                {
                    case 0: // front
                        px = x_side;
                        pz = walloffset; 

                        break;  
                    case 1: // right
                        px = walloffset;
                        pz = -x_side; 
                        break; 
                    case 2: // back 
                        px = -x_side;
                        pz = -walloffset; 
                        break; 
                    case 3: // left
                        px = -walloffset;
                        pz = x_side; 
                        break; 
                }
                pos = new Vector3(px, py, pz) * settings.cellSize; 


                break;
            case TetrisSettings.TetrisMode.Tetris3D:
                throw new System.NotImplementedException(); 
                break;
            default:
                break;
        }
        if(center)
            pos += Vector3.one * settings.cellSize / 2.0f; // ground offset, so that bottom face of blocks at bottom layer are at y=0

        return pos; 

    }


}
