using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EntranceController : MonoBehaviour
{
    public int doorNumber;
    public string sceneName;
    public bool opened;
    bool isTouch;
    bool messageDisplay;
    GameObject worldUI;
    GameObject talkPanel;
    TextMeshProUGUI messageText; // TextMeshProUGUIを使う場合。UI.TextならTextに変更

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        worldUI = GameObject.FindGameObjectWithTag("WorldUI");
        talkPanel = worldUI.transform.Find("TalkPanel").gameObject;
        messageText = talkPanel.transform.Find("MessageText").gameObject.GetComponent<TextMeshProUGUI>();

        opened = World_UIController.keyopens[doorNumber];
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouch && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!messageDisplay)
            {
                Time.timeScale = 0;
                if (opened)
                {
                    talkPanel.SetActive(true);
                    Time.timeScale = 1;
                    GameManager.doorNumber = doorNumber;
                    SceneManager.LoadScene(sceneName);
                }
                else if (GameManager.key > 0)
                {
                    talkPanel.SetActive(true);
                    messageText.text = "新たなステージへの扉を開けた！";
                    GameManager.key--;
                    World_UIController.keyopens[doorNumber] = true;
                    opened = true;
                    messageDisplay = true;
                }
                else
                {
                    talkPanel.SetActive(true);
                    messageText.text = "鍵が足りません！";
                    messageDisplay = true;
                }
            }
            else
            {
                Time.timeScale = 1;
                messageText.text = sceneName;
                messageDisplay = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = true;
            talkPanel.SetActive(true);
            messageText.text = sceneName;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = false;
            if (messageText != null) // NullReferenceExceptionを防ぐ
            {
                messageText.text = ""; // メッセージを解除
                Time.timeScale = 1f; // ゲーム進行を再開
            }
        }
    }
}
