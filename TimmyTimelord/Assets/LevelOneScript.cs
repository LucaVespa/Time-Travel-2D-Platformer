using UnityEngine;

public class LevelOneScript : MonoBehaviour
{
    public GameObject crawler;

    private CloneScript cloneScript;
    private AttackScript attackScript;

    void Start()
    {
        attackScript = GameObject.FindGameObjectWithTag("AttackLogicTag").gameObject.GetComponent<AttackScript>();
        cloneScript = GameObject.FindGameObjectWithTag("CloneTag").GetComponent<CloneScript>();

        //CreateCrawler(0, 0);
        //CreateCrawler(15, 15);
        //CreateCrawler(6, 8);
        CreateCrawler(-6, 8);
    }

    private void CreateCrawler(int x, int y)
    {
        GameObject tempCrawler = Instantiate(crawler, new Vector3(x, y, 0), transform.rotation);
        tempCrawler.transform.Find("CrawlerBody").gameObject.AddComponent<CrawlerScript>();
        tempCrawler.transform.Find("CrawlerBody").gameObject.GetComponent<CrawlerScript>().SetObjects(cloneScript, attackScript, tempCrawler);
    }
}
