using UnityEngine;
public class PlayerData
{
    public int attackNum; //0 none, 1 forward, 2 upward, 3 downward
    public bool isFlipped;
    public Vector2 position;
    public Vector2 attackPosition;
    public Vector2 attackTrajectory;

    public PlayerData(Vector2 position, Vector2 attackPosition, int attackNum, Vector2 attackTrajectory, bool isFlipped)
    {
        this.position = position;
        this.attackPosition = attackPosition;
        this.attackNum = attackNum;
        this.attackTrajectory = attackTrajectory;
        this.isFlipped = isFlipped;
    }
}
