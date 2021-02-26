using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/**
 * * BROKEN, DO NOT USE
*/
class ClassicInput : IInput
{
    public bool isHoldBrick() { return false; }
    public bool isPause(){ return false; }
    public bool isDeselectBrick()
    {
        return false ;
    }
    public int getAbsHorizontalPosition()
    {
        return -1;
    }
    
    public bool isSelectBrick()
    {
        return true; 
    }

    public bool isInputMoveDown()
    {
        return Input.GetKeyDown(KeyCode.Space); 
    }

    public bool isInputRotateLeft()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    public bool isInputRotateRight()
    {
        return Input.GetKeyDown(KeyCode.S);
    }

    public bool isMoveLeft()
    {
        return Input.GetKeyDown(KeyCode.A);
    }

    public bool isMoveRight()
    {
        return Input.GetKeyDown(KeyCode.D);
    }
}

