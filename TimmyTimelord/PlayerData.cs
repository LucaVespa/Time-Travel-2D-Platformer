using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData
{
    public Vector2 position;

    public Vector2 attackPosition;
    public int attackNum; //0 none, 1 forward, 2 upward, 3 downward

    public Vector2 attackTrajectory;


    public PlayerData(Vector2 position, Vector2 attackPosition, int attackNum, Vector2 attackTrajectory)
    {
        this.position = position;

    }
}
