using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EntranceController : MonoBehaviour
{
    public int doorNumber;
    public string sceneName;
    public bool opened;

    bool isPlayerTouch;

    bool announcement;

    GameObject worldUI;
    GameObject talkPanel;
    TextMeshProUGUI messageText; // TextMeshProUGUIを使う場合。UI.TextならTextに変更
    World_PlayerController worldPlayerCnt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        worldPlayerCnt = GameObject.FindGameObjectWithTag("Player").GetComponent<World_PlayerController>();
        worldUI = GameObject.FindGameObjectWithTag("WorldUI");
        talkPanel = worldUI.transform.Find("TalkPanel").gameObject;
        messageText = talkPanel.transform.Find("MessageText").gameObject.GetComponent<TextMeshProUGUI>();

        if (World_UIController.keyOpened != null)
        {
            opened = World_UIController.keyOpened[doorNumber];
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isPlayerTouch && worldPlayerCnt.IsActionButtonPressed)
        {
            if (!announcement)
            {
                Time.timeScale = 0;
                if (opened)
                {
                    Time.timeScale = 1;
                    GameManager.currentDoorNumber = doorNumber;
                    SceneManager.LoadScene(sceneName);
                    return;
                }
                else if (GameManager.keys > 0)
                {
                    messageText.text = "新たなステージへの扉を開けた！";
                    GameManager.keys--;
                    opened = true;
                    World_UIController.keyOpened[doorNumber] = true;
                    announcement = true;
                }
                else
                {
                    messageText.text = "鍵が足りません！";
                    announcement = true;
                }
            }
            else
            {
                Time.timeScale = 1;
                messageText.text = sceneName + "(" + opened + ")";
                announcement = false;
            }

            //連続入力にならないように一度リセット　※次にボタンが押されるまではfalse
            worldPlayerCnt.IsActionButtonPressed = false;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerTouch = true;
            talkPanel.SetActive(true);
            messageText.text = sceneName + "(" + opened + ")";
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerTouch = false;
            if (messageText != null) // NullReferenceExceptionを防ぐ
            {
                talkPanel.SetActive(false);
                Time.timeScale = 1f; // ゲーム進行を再開
            }
        }
    }
}
