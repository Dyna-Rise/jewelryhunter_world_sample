using UnityEngine;
using UnityEngine.SceneManagement;

public class Advent_ItemBox : MonoBehaviour
{
    public Sprite openImage;
    public GameObject itemPrefab;
    public bool isClosed = true;
    public AdventItemType type = AdventItemType.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(type == AdventItemType.Key)
        {
            if(GameManager.keyGot[SceneManager.GetActiveScene().name] == true)
            {
                isClosed = false;
                GetComponent<SpriteRenderer>().sprite = openImage;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isClosed && collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = openImage;
            isClosed = false;
            if(itemPrefab != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
