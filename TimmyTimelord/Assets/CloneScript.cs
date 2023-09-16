using System.Collections.Generic;
using UnityEngine;

public class CloneScript : MonoBehaviour
{
    public GameObject clone;
    public SpriteRenderer filter;
    public GameObject attackLogic;

    private GameObject forwardAttack;
    private GameObject upwardAttack;
    private GameObject downwardAttack;
    private Vector2 cloneAttackTrajectory;

    private AttackScript attackScript;
    private Queue<PlayerData> positionQueue = new Queue<PlayerData>();
    private bool abilityActive;
    private bool abilityReady;
    private GameObject currentClone;
    private const int maxQueueSize = Constants.MaxQueueSize;

    void Start()
    {
        attackScript = attackLogic.gameObject.GetComponent<AttackScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!abilityActive)
        {
            positionQueue.Enqueue(attackScript.GetPlayerState());
        }
        else
        {
            PlayerData tempData = positionQueue.Dequeue();
            currentClone.transform.position = tempData.position;
            currentClone.gameObject.GetComponent<SpriteRenderer>().flipX = tempData.isFlipped;

            forwardAttack.GetComponent<CircleCollider2D>().enabled = false;
            forwardAttack.GetComponent<SpriteRenderer>().enabled = false;

            upwardAttack.GetComponent<CircleCollider2D>().enabled = false;
            upwardAttack.GetComponent<SpriteRenderer>().enabled = false;

            downwardAttack.GetComponent<CircleCollider2D>().enabled = false;
            downwardAttack.GetComponent<SpriteRenderer>().enabled = false;

            cloneAttackTrajectory = tempData.attackTrajectory;

            if (tempData.attackNum == 1)
            {
                forwardAttack.GetComponent<CircleCollider2D>().enabled = true;
                forwardAttack.GetComponent<SpriteRenderer>().enabled = true;
                forwardAttack.transform.position = tempData.attackPosition;
            }
            else if (tempData.attackNum == 2)
            {
                upwardAttack.GetComponent<CircleCollider2D>().enabled = true;
                upwardAttack.GetComponent<SpriteRenderer>().enabled = true;
                upwardAttack.transform.position = tempData.attackPosition;
            }
            else if (tempData.attackNum == 3)
            {
                downwardAttack.GetComponent<CircleCollider2D>().enabled = true;
                downwardAttack.GetComponent<SpriteRenderer>().enabled = true;
                downwardAttack.transform.position = tempData.attackPosition;
            }
        }


        if (positionQueue.Count == 0)
        {
            Destroy(currentClone);
            abilityActive = false;
            filter.enabled = false;
        }
        else if (positionQueue.Count > maxQueueSize)
        {
            positionQueue.Dequeue();
            abilityReady = true;
        }

        if (Input.GetKeyDown(KeyCode.J) && positionQueue.Count == Constants.MaxQueueSize)
        {
            abilityReady = false;
            filter.enabled = true;
            abilityActive = true;
            Vector2 tempVect = positionQueue.Dequeue().position;
            currentClone = Instantiate(clone, new Vector3(tempVect.x, tempVect.y, 0), transform.rotation);
            forwardAttack = currentClone.transform.Find("ForwardAttack").gameObject;
            upwardAttack = currentClone.transform.Find("UpwardAttack").gameObject;
            downwardAttack = currentClone.transform.Find("DownwardAttack").gameObject;
        }
    }

    public bool GetAbilityReady()
    {
        return abilityReady;
    }

    public bool GetAbilityActive()
    {
        return abilityActive;
    }

    public Vector2 GetAttackTrajectory()
    {
        return cloneAttackTrajectory;
    }
}
