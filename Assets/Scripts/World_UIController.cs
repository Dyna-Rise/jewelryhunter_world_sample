using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class World_UIController : MonoBehaviour
{
    public static Dictionary<int, bool> keyOpened;

    public TextMeshProUGUI keyText;
    int currentKeys;
    public TextMeshProUGUI arrowText;
    int currentArrows;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Entrance");

        //リストがない時の情報取得とセッティング
        if (keyOpened == null)
        {
            keyOpened = new Dictionary<int, bool>(); // 最初に初期化が必要

            for (int i = 0; i < obj.Length; i++)
            {
                EntranceController entranceController = obj[i].GetComponent<EntranceController>();
                if (entranceController != null)
                {
                    keyOpened.Add(
                        entranceController.doorNumber,
                        entranceController.opened
                    );
                }
            }
        }

        //プレイヤーの位置
        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 currentPlayerPos = Vector2.zero;
        for(int i = 0; i < obj.Length; i++)
        {
            if(obj[i].GetComponent<EntranceController>().doorNumber == GameManager.currentDoorNumber)
            {
                currentPlayerPos = obj[i].transform.position;
            }
        }
        player.transform.position = currentPlayerPos;

    }

    // Update is called once per frame
    void Update()
    {
        if(currentKeys != GameManager.keys)
        {
            currentKeys = GameManager.keys;
            keyText.text = currentKeys.ToString();
        }
        if (currentArrows != GameManager.arrows)
        {
            currentArrows = GameManager.arrows;
            arrowText.text = currentArrows.ToString();
        }
    }
}
