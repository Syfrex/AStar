using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int GridX, GridY, MoveCost, ManhattanCost;
    public bool isWall;
    public Vector3 Position;
    public Node Parent;

    public int FCost
    {
        get
        {
            return MoveCost + ManhattanCost;
        }
    }

    public Node(bool p_isAWall, Vector3 p_position, int p_gridX, int p_gridY)
    {
        isWall = p_isAWall;
        Position = p_position;
        GridX = p_gridX;
        GridY = p_gridY;
    }
}
