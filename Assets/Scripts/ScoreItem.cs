using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public ItemData itemData;   // アイテムデータ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.itemSprite;
    }
}
