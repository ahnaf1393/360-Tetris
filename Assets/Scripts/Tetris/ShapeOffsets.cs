/**
 * Definitions of our bricks
 * each pair of entries corresponds to the x/y offset of a block from the bricks center
 * 
 */


public class ShapeOffsets
{
    public const int maxOffset = 5;
    public const int numBricks = 12;

    public static int[] L = {
        0,1,
        0,0,
        0,-1,
        1,-1,
    };

    public static int[] L_5 = {
        0,2,
        0,1,
        0,0,
        0,-1,
        1,-1,
    };

    public static int[] L_5_mirr = {
        0,0,
        0,1,
        0,2,
        0,-1,
        -1,-1,
    };

    public static int[] Lmirr = {
        0,1,
        0,0,
        0,-1,
        -1,-1,
    };


    public static int[] Box =
    {
        0,0,
        0,-1,
        -1,0,
        -1,-1,
    };

    public static int[] Tri =
    {
        0,0,
        -1,0,
        1,0,
        0,1,
    };

    public static int[] Z =
    {
        0,0,
        0,1,
        -1,1,
        1,0,
    };

    public static int[] Z_5 = {
        -1,1,
        0,1,
        0,0,
        0,-1,
        1,-1,
    };

    public static int[] Zmirr =
    {
        0,0,
        0,1,
        1,1,
        0,-1
    };

    public static int[] Long =
    {
        0,0,
        -1,0,
        1,0,
        2,0
    };

    public static int[] Long_5 =
    {
        -2,0,
        -1,0,
        0,0,
        1,0,
        2,0
    };

    //public static int[] Longside =
    //{
    //    0,0,
    //    1,1,
    //    -1,-1,
    //    -2,-2
    //};

    public static int[] Plus =
    {
        0,0,
        1,0,
        0,1,
        -1,0
    };

    public static int[] Halfaquare =
    {
        1,0,
        1,1,
        0,1,
        -1,1,
        -1,0
    };

}