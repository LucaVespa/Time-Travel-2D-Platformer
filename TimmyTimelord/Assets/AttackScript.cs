using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public GameObject player;
    public GameObject forwardAttack;
    public GameObject upwardAttack;
    public GameObject downwardAttack;

    private SpriteRenderer upwardAttackSprite;
    private CircleCollider2D upwardAttackHitbox;
    private float upwardAttackXOffset = 0.85f;
    private float upwardAttackYOffset = 0.85f;
    private bool upwardAttackStart = false;

    private SpriteRenderer downwardAttackSprite;
    private CircleCollider2D downwardAttackHitbox;
    private float downwardAttackXOffset = 0.85f;
    private float downwardAttackYOffset = 0.85f;
    private bool downwardAttackStart = false;

    private SpriteRenderer forwardAttackSprite;
    private CircleCollider2D forwardAttackHitbox;
    private float forwardAttackOffset = 0.85f;
    private bool forwardAttackStart = false;

    private Vector2 attackTrajectory;

    private float attackDuration = 0f;

    void Start()
    {
        forwardAttackHitbox = forwardAttack.GetComponent<CircleCollider2D>();
        forwardAttackSprite = forwardAttack.GetComponent<SpriteRenderer>();
        forwardAttackSprite.enabled = false;
        forwardAttackHitbox.enabled = false;

        upwardAttackHitbox = upwardAttack.GetComponent<CircleCollider2D>();
        upwardAttackSprite = upwardAttack.GetComponent<SpriteRenderer>();
        upwardAttackHitbox.enabled = false;
        upwardAttackSprite.enabled = false;

        downwardAttackHitbox = downwardAttack.GetComponent<CircleCollider2D>();
        downwardAttackSprite = downwardAttack.GetComponent<SpriteRenderer>();
        downwardAttackHitbox.enabled = false;
        downwardAttackSprite.enabled = false;
    }


    void FixedUpdate()
    {
        HandleUpwardAttack();
        HandleDownwardAttack();
        HandleForwardAttack();
    }

    public Vector2 getAttackTrajectory()
    {
        return attackTrajectory;
    }   

    private void HandleUpwardAttack()
    {
        if (((Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.I)) || Input.GetKeyDown(KeyCode.O)) && !player.GetComponent<PlayerMovement>().IsOnAttackCooldown())
        {

            player.GetComponent<PlayerMovement>().EnterAttackCooldown(0.3f);
            upwardAttackStart = true;
            attackTrajectory = new Vector2(-3, 25);
        }
        if (upwardAttackStart)
        {
            attackDuration += Time.fixedDeltaTime;

            if (attackDuration >= Frame(14))
            {
                upwardAttackStart = false;
                upwardAttackSprite.enabled = false;
                upwardAttackHitbox.enabled = false;
                attackDuration = 0f;
            }
            else if (attackDuration >= Frame(5))
            {
                upwardAttackSprite.enabled = true;
                upwardAttackHitbox.enabled = true;
                upwardAttackXOffset = 1.8f * Mathf.Cos((60f*attackDuration - 5) * Mathf.PI / 8);
                upwardAttackYOffset = 1.8f * Mathf.Sin((60f*attackDuration - 5) * Mathf.PI / 8);
            }

            if (!player.GetComponent<SpriteRenderer>().flipX)
            {
                upwardAttack.transform.position = new Vector3(player.transform.position.x + upwardAttackXOffset, player.transform.position.y + upwardAttackYOffset, player.transform.position.z - 1);
            }
            else
            {
                upwardAttack.transform.position = new Vector3(player.transform.position.x + upwardAttackXOffset * -1, player.transform.position.y + upwardAttackYOffset, player.transform.position.z - 1);
            }
        }
    }

    private void HandleDownwardAttack()
    {
        if (((Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.I)) || Input.GetKeyDown(KeyCode.P)) && !player.GetComponent<PlayerMovement>().IsOnAttackCooldown())
        {

            player.GetComponent<PlayerMovement>().EnterAttackCooldown(0.3f);
            downwardAttackStart = true;
            attackTrajectory = new Vector2(-5, -40);
        }

        if (downwardAttackStart)
        {
            attackDuration += Time.fixedDeltaTime;

            if (attackDuration >= Frame(10))
            {
                downwardAttackStart = false;
                downwardAttackSprite.enabled = false;
                downwardAttackHitbox.enabled = false;
                attackDuration = 0f;
            }
            else if (attackDuration >= Frame(5))
            {
                downwardAttackSprite.enabled = true;
                downwardAttackHitbox.enabled = true;
                downwardAttackXOffset = 1.8f * Mathf.Cos((60f*attackDuration - 5) * Mathf.PI / 8);
                downwardAttackYOffset = -1.8f * Mathf.Sin((60f * attackDuration - 5) * Mathf.PI / 8);   
            }

            if (!player.GetComponent<SpriteRenderer>().flipX)
            {
                downwardAttack.transform.position = new Vector3(player.transform.position.x + downwardAttackXOffset, player.transform.position.y + downwardAttackYOffset, player.transform.position.z - 1);
            }
            else
            {
                downwardAttack.transform.position = new Vector3(player.transform.position.x + downwardAttackXOffset * -1, player.transform.position.y + downwardAttackYOffset, player.transform.position.z - 1);
            }
        }
    }

    private void HandleForwardAttack()
    {
        if (Input.GetKeyDown(KeyCode.I) && !player.GetComponent<PlayerMovement>().IsOnAttackCooldown())
        {

            player.GetComponent<PlayerMovement>().EnterAttackCooldown(0.3f);
            forwardAttackStart = true;
            attackTrajectory = new Vector2(-20, 15);
        }

        if (forwardAttackStart)
        {
            attackDuration += Time.fixedDeltaTime;
            if (attackDuration >= Frame(8))
            {
                forwardAttackStart = false;
                forwardAttackSprite.enabled = false;
                forwardAttackHitbox.enabled = false;
                attackDuration = 0f;
            } 
            else if (attackDuration >= Frame(4))
            {
                forwardAttackSprite.enabled = true;
                forwardAttackHitbox.enabled = true;
                forwardAttackOffset = (60f*attackDuration - 4) * 0.5f + 0.45f;
            }

            if (!player.GetComponent<SpriteRenderer>().flipX)
            {
                forwardAttack.transform.position = new Vector3(player.transform.position.x + forwardAttackOffset, player.transform.position.y, player.transform.position.z - 1);
            }
            else
            {
                forwardAttack.transform.position = new Vector3(player.transform.position.x + forwardAttackOffset * -1, player.transform.position.y, player.transform.position.z - 1);
            } 
        }
    }

    //Calculates a float representation of a frame (Out of 60 fps)
    private float Frame(int frame)
    {
        return (float)((float)frame / 60f);
    }

    public PlayerData GetPlayerState()
    {
        int attackNum = 0;
        Vector2 attackPosition = new Vector2(0, 0);
        if (forwardAttackSprite.enabled)
        {
            attackNum = 1;
            attackPosition = forwardAttack.transform.position;
        }
        else if (upwardAttackSprite.enabled)
        {
            attackNum = 2;
            attackPosition = upwardAttack.transform.position;
        }
        else if (downwardAttackSprite.enabled)
        {
            attackNum = 3;
            attackPosition = downwardAttack.transform.position;
        }
        return new PlayerData(player.transform.position, attackPosition, attackNum, attackTrajectory, player.gameObject.GetComponent<SpriteRenderer>().flipX);
    }
}
