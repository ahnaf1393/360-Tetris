using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
public class InputFlags
{
    public bool moveLeft = false;
    public bool moveRight = false;
    public bool rotateLeft = false;
    public bool rotateRight = false;
    public bool selectBrick = false;
    public bool deselectBrick = false;
    public bool moveDown = false;
    public bool isHoldBrick = false;
    public bool pause = false; 

    public void setZero()
    {
        moveLeft = false;
        moveRight = false;
        rotateLeft = false;
        rotateRight = false;
        selectBrick = false;
        deselectBrick = false;
        moveDown = false;
        isHoldBrick = false;
        pause = false;
}
    public bool any()
    {
        return moveLeft || moveRight || rotateLeft || rotateRight || selectBrick || deselectBrick|| moveDown || isHoldBrick || pause; 
    }

}
public interface IInput
{

    bool isInputRotateLeft();
    bool isInputRotateRight();
    bool isInputMoveDown();
    bool isMoveRight();
    bool isMoveLeft();
    int getAbsHorizontalPosition();
    bool isSelectBrick();
    bool isDeselectBrick();

    bool isHoldBrick();
    bool isPause(); 






}

