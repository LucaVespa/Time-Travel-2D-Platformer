using UnityEngine;

public class PlatformDropScript : MonoBehaviour
{
    public PlatformEffector2D platform;

    private PlayerMovement playerMovement;

    private bool switchOrder;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.S) && !Input.GetKey(KeyCode.LeftShift))
        {
            platform.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            switchOrder = true;
        }
        if(switchOrder && !playerMovement.IsJumpRestricted())
        {
            platform.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            switchOrder = false;
        }
    }
}
