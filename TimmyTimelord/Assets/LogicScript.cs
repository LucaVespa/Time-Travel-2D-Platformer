using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public Text livesDisplay;
    public SpriteRenderer filter;

    private int lives = Constants.StartingLives;

    private void Awake()
    {
        Application.targetFrameRate = Constants.TargetFrameRate;
        Time.fixedDeltaTime = Constants.FixedDeltaTime;
        QualitySettings.vSyncCount = 0;
    }

    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 9); //crawler with other crawler
        Physics2D.IgnoreLayerCollision(3, 9); //player and crawler rigid body
        Physics2D.IgnoreLayerCollision(8, 12); //platform and mob surface
        Physics2D.IgnoreLayerCollision(7, 12); //jump restrict and mob surface
        Physics2D.IgnoreLayerCollision(3, 12); //player and mob surface
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.F) && filter.enabled)
        {
            filter.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            filter.enabled = true;
        }
    }

    public void DecreaseLives(int amount)
    {
        lives -= amount;
        livesDisplay.text = lives.ToString();
    }

    public void IncreaseLives(int amount)
    {
        lives += amount;
        livesDisplay.text = lives.ToString();
    }

}
