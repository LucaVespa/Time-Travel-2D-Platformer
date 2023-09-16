using System.Collections.Generic;
using UnityEngine;

class CrawlerData
{
    public Vector2 timeTravelPosition;
    public Vector2 timeTravelVelocity;
    public int timeTravelDirection;

    public CrawlerData(Vector2 timeTravelPosition, Vector2 timeTravelVelocity, int timeTravelDirection)
    {
        this.timeTravelPosition = timeTravelPosition;
        this.timeTravelVelocity = timeTravelVelocity;
        this.timeTravelDirection = timeTravelDirection;
    }
}

public class CrawlerScript : MonoBehaviour
{
    private Rigidbody2D crawler;
    private GameObject hitbox;

    private Queue<CrawlerData> positionQueue = new Queue<CrawlerData>();
    private CloneScript cloneScript;
    private AttackScript attackScript;


    private bool attacked = false;
    private float attackedTimer = 0f;
    private int direction = -1;
    private bool onGround;
    private bool groundTouch;

    private float stuckTimer = 0f;
    private float tempX1 = 1f;
    private float tempX2 = 2f;

    private float previousFrameXVelocity = 0f;
    private float previousFrameYVelocity = 0f;


    private bool abilityActive = false;
    private bool attackedByFuture;

    void FixedUpdate()
    {
        HandleTimeTravel();

        if(groundTouch && crawler.velocity.y == 0 && previousFrameYVelocity == 0)
        {
            onGround = true;
        }
        previousFrameXVelocity = crawler.velocity.x;
        previousFrameYVelocity = crawler.velocity.y;
        StuckCheck();
        hitbox.transform.position = crawler.transform.position;

        if (attacked)
        {
            attackedTimer += Time.fixedDeltaTime;
            if (attackedTimer >= 0.5f && onGround && crawler.velocity.y > -20)
            {
                attacked = false;
                attackedTimer = 0f;
            }
        }
        else
        {
            crawler.velocity = new Vector2(5 * direction, crawler.velocity.y);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 13 && !attacked && onGround)
        {
            direction = direction * -1;
            crawler.gameObject.GetComponent<SpriteRenderer>().flipX = (direction == 1) ? true : false;
        }
        if (collision.gameObject.layer == 16)
        {
            if(abilityActive)
            {
                attackedByFuture = true;
            }

            attacked = true;
            crawler.velocity = attackScript.getAttackTrajectory();
            if (collision.transform.parent.position.x < crawler.position.x)
            {
                crawler.velocity = new Vector2(-1 * crawler.velocity.x, crawler.velocity.y);
            } 
            if(crawler.velocity.y < 0 && onGround) 
            {
                crawler.velocity = new Vector2(crawler.velocity.x, -1 * crawler.velocity.y * 0.6f);
            }
        }

        if (collision.gameObject.layer == 17)
        {
            attacked = true;
            crawler.velocity = cloneScript.GetAttackTrajectory();
            if (collision.transform.parent.position.x < crawler.position.x)
            {
                crawler.velocity = new Vector2(-1 * crawler.velocity.x, crawler.velocity.y);
            }
            if (crawler.velocity.y < 0 && onGround)
            {
                crawler.velocity = new Vector2(crawler.velocity.x, -1 * crawler.velocity.y * 0.6f);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
        {
            groundTouch = true;
        }
        if (collision.gameObject.layer == 6 && !attacked && onGround)
        {
            direction = direction * -1;
            crawler.gameObject.GetComponent<SpriteRenderer>().flipX = (direction == 1) ? true : false;
        }
        else if (collision.gameObject.layer == 6)
        {
            crawler.velocity = new Vector2((-1 * previousFrameXVelocity)*0.6f, crawler.velocity.y);
        }
        if ((collision.gameObject.layer == 8 || collision.gameObject.layer == 12) && attacked && previousFrameYVelocity < -40)
        {
            crawler.velocity = new Vector2(previousFrameXVelocity, ( -1 * previousFrameYVelocity) * 0.6f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
        {
            onGround = false;
            groundTouch = false;
        }
    }

    private void StuckCheck()
    {
        stuckTimer += Time.fixedDeltaTime;
        if (stuckTimer > 0.6f)
        {
            stuckTimer = 0f;
            if (crawler.position.x == tempX1 && tempX1 == tempX2)
            {
                direction = direction * -1;
            }
        }
        else if (stuckTimer > 0.4f)
        {
            tempX2 = crawler.position.x;
        }
        else if (stuckTimer > 0.2f)
        {
            tempX1 = crawler.position.x;
        }
    }

    private void HandleTimeTravel()
    {
        
        if ((cloneScript.GetAbilityActive() || cloneScript.GetAbilityReady()) && Input.GetKeyDown(KeyCode.J) && positionQueue.Count == Constants.MaxQueueSize)
        {
            attackedByFuture = false;
            abilityActive = true;
        }
        if (!abilityActive)
        {
            positionQueue.Enqueue(new CrawlerData(crawler.transform.position, new Vector2(crawler.velocity.x, crawler.velocity.y), direction));

            if (positionQueue.Count > Constants.MaxQueueSize)
            {
                positionQueue.Dequeue();
            }
        } 

        else if (positionQueue.Count == 0 || attackedByFuture)
        {
            abilityActive = false;           
        } 
        else
        {
            CrawlerData tempData = positionQueue.Dequeue();
            crawler.transform.position = tempData.timeTravelPosition;
            crawler.velocity = tempData.timeTravelVelocity;
            direction = tempData.timeTravelDirection;
            crawler.gameObject.GetComponent<SpriteRenderer>().flipX = (tempData.timeTravelDirection == 1) ? true : false;
        }
    }

    public void SetObjects(CloneScript c, AttackScript a, GameObject tempCrawler)
    {
        cloneScript = c;
        attackScript = a;
        crawler = tempCrawler.transform.Find("CrawlerBody").gameObject.GetComponent<Rigidbody2D>();
        hitbox = tempCrawler.transform.Find("CrawlerHitbox").gameObject;
    }
}
