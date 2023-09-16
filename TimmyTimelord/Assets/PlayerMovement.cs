using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D player;
    public Camera mainCamera; 

    private LogicScript logic;


    private int xSpeed = Constants.MovementSpeed;
    private bool onGround = false;
    private int jumps = 0;
    private int fastFalls = 0;
    private float shiftMod = 1f;
    private float airMultiplier = 1f;
    private float jumpSquatTimer = 0f;
    private bool jumpHeld = false;

    private float disableTimer = 0f;
    private bool disabled = false;

    private bool invulnerable = false;
    private float invulnTimer = 0f;
    private float invulnDuration = 0f;

    private bool jumpRestrict = false;

    private bool onCooldown = false;
    private float attackCooldown = 0f;
    private float attackCooldownDuration = 0f;


    private void Start()
    {
        logic = GameObject.FindGameObjectWithTag("LogicTag").GetComponent<LogicScript>();
    }

    void FixedUpdate()
    {
        HandleCamera();
        TimerChecks();
        AdjustShift();
        HandleJumps();
        HandleHorizontalMovement();
        HandleDownwardMovement();

        if (!onGround)
        {
            jumps = 0;
        }
        else
        {
            jumps = 1;
            fastFalls = 1;
        }
    }

    private void TimerChecks()
    {
        if (disabled)
        {
            disableTimer += Time.fixedDeltaTime;
            if (disableTimer >= .2)
            {
                disabled = false;
                disableTimer = 0;
            }
        }
        if (jumpHeld)
        {
            jumpSquatTimer += Time.fixedDeltaTime;
        }

        if (onCooldown)
        {
            attackCooldown += Time.fixedDeltaTime;
            if (attackCooldown >= attackCooldownDuration)
            {
                attackCooldown = 0f;
                onCooldown = false;
            }
        }

        if (invulnerable)
        {
            invulnTimer += Time.fixedDeltaTime;
            if (invulnTimer >= invulnDuration)
            {
                invulnTimer = 0f;
                invulnerable = false;
            }
        }
    }

    private void HandleJumps()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumps == 1 && !disabled && !jumpRestrict)
        {
            jumpHeld = true;
        }
        if (jumpSquatTimer > Constants.JumpSquatLength)
        {
            player.velocity = new Vector2(player.velocity.x, 40);
            jumps = 0;
            jumpHeld = false;
            jumpSquatTimer = 0;
        }
        if (Input.GetKeyUp(KeyCode.Space) && jumpSquatTimer <= Constants.JumpSquatLength && jumpSquatTimer != 0)
        {
            player.velocity = new Vector2(player.velocity.x, 30);
            jumps = 0;
            jumpHeld = false;
            jumpSquatTimer = 0;
        }
    }

    private void HandleHorizontalMovement()
    {
        //Limits maximum air velocity
        if (!onGround && (player.velocity.x > xSpeed * airMultiplier * shiftMod || player.velocity.x < -xSpeed * airMultiplier * shiftMod))
        {
            player.velocity = new Vector2(player.velocity.x * airMultiplier, player.velocity.y);
        }

        //Move Right
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !disabled)
        {
            if (onGround)
            {
                if (!onCooldown)
                {
                    player.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
                player.velocity = new Vector2(xSpeed * shiftMod * airMultiplier, player.velocity.y);
            }
            else if (player.velocity.x <= xSpeed * airMultiplier)
            {
                player.velocity += new Vector2(300 * shiftMod * airMultiplier * Time.fixedDeltaTime, 0);
            }
        }

        //Move Left
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)  && !disabled)
        {
            if (onGround)
            {
                if (!onCooldown)
                {
                    player.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                player.velocity = new Vector2(-xSpeed * shiftMod * airMultiplier, player.velocity.y);
            }
            else if (player.velocity.x >= -xSpeed * airMultiplier)
            {
                player.velocity += new Vector2(-300 * shiftMod * airMultiplier * Time.fixedDeltaTime, 0);
            }
        }
    }

    private void HandleDownwardMovement()
    {
        if (Input.GetKeyDown(KeyCode.S) && fastFalls > 0 && !onGround && player.velocity.y <= 10 && player.velocity.y > -20 && !disabled)
        {
            player.velocity = new Vector2(player.velocity.x, -20);
            fastFalls--;
        }
    }

    //Adjusts movement values based on whether or not the "Shift" key is held
    private void AdjustShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftMod = Constants.ShiftMulipliier;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shiftMod = 1f;
        }
    }

    private void HandleCamera()
    {

        if (mainCamera.transform.position.x +  5 < player.position.x)
        {
            mainCamera.transform.position = new Vector3(player.position.x - 5, mainCamera.transform.position.y, -10);
        }
        else if (mainCamera.transform.position.x - 5 > player.position.x)
        {
            mainCamera.transform.position = new Vector3(player.position.x + 5, mainCamera.transform.position.y, -10);
        }

        if (mainCamera.transform.position.y + 4 < player.position.y)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, player.position.y - 4, - 10);
        }
        else if (mainCamera.transform.position.y - 4 > player.position.y)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, player.position.y + 4, -10);
        }
    }

    public void EnterAttackCooldown(float duration)
    {
        onCooldown = true;
        attackCooldownDuration = duration;
    }

    public bool IsDisabled()
    {
        return disabled;
    }

    public bool IsJumpRestricted()
    {
        return jumpRestrict;
    }

    public bool IsOnAttackCooldown()
    {
        return onCooldown;
    }

    public bool IsInvuln()
    {
        return invulnerable;
    }

    public void SetDisabled(bool b)
    {
        disabled = b;
    }

    public void SetInvuln(bool b)
    {
        invulnerable = b;
        invulnDuration = 0.5f;
    }

    public void SetInvuln(bool b, float duration)
    {
        invulnerable = b;
        invulnDuration = duration;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && player.velocity.y <= 0)
        {
            onGround = true;
            airMultiplier = 1f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            onGround = false;
            airMultiplier = Constants.AirMultiplier;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            jumpRestrict = true;
        }

        if (collision.gameObject.layer == 10 && !IsInvuln())
        {
            if (collision.transform.position.x < player.position.x)
            {
                player.velocity = new Vector2(20, 25);
            }
            else
            {
                player.velocity = new Vector2(-20, 25);
            }
            SetDisabled(true);
            SetInvuln(true, 0.4f);
            logic.DecreaseLives(1);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            jumpRestrict = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            player.GetComponent<Collider2D>().enabled = false;
            player.GetComponent<Collider2D>().enabled = true;
        }
    }
}