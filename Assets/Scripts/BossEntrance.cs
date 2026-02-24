using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEntrance : MonoBehaviour
{

    public static Dictionary<int, bool> stagesClear;
    public string sceneName;
    bool isOpened;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Entrance");

        //リストがない時の情報取得とセッティング
        if (stagesClear == null)
        {
            stagesClear = new Dictionary<int, bool>(); // 最初に初期化が必要

            for (int i = 0; i < obj.Length; i++)
            {
                EntranceController entranceController = obj[i].GetComponent<EntranceController>();
                if (entranceController != null)
                {
                    stagesClear.Add(
                        entranceController.doorNumber,
                        false
                    );
                }
            }
        }
        else
        {
            int sum = 0;
            for(int i = 0; i < stagesClear.Count; i++)
            {
                if (stagesClear[i])
                {
                    sum++;
                }
            }
            if(sum >= stagesClear.Count)
            {
                isOpened = true;
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && isOpened)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
