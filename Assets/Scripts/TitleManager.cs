using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class TitleManager : MonoBehaviour
{
    public string sceneName;            // 読み込むシーン名

    public GameObject startButton;
    public GameObject continueButton;
    bool selectStart = true;
    //public InputAction submitAction; //InputAction

    //void OnEnable()
    //{
    //    submitAction.Enable(); //InputActionを有効化
    //}
    //void OnDisable()
    //{
    //    submitAction.Disable(); //InputActionを無効化
    //}

    void OnSelect(InputValue value)
    {
        if (value.isPressed)
        {
            // そもそもセーブデータがなければボタンを切り替えられない
            string jsonData = PlayerPrefs.GetString("SaveData");
            if (string.IsNullOrEmpty(jsonData))
            {
                return;
            }

            if (selectStart)
            {
                startButton.GetComponent<Button>().interactable = false;
                continueButton.GetComponent<Button>().interactable = true;
                selectStart = false;
            }
            else
            {
                startButton.GetComponent<Button>().interactable = true;
                continueButton.GetComponent<Button>().interactable = false;
                selectStart = true;
            }
        }
    }

    void OnSubmit(InputValue value)
    {
        if (selectStart)
        {
            Load();
        }
        else
        {
            ContinueLoad();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // PlayerPrefsからJSON文字列をロード
        string jsonData = PlayerPrefs.GetString("SaveData");

        // JSONデータが存在しない場合、エラーを回避し処理を中断
        if (string.IsNullOrEmpty(jsonData))
        {
            continueButton.GetComponent<Button>().interactable = false; //ボタン機能を無効
        }

        continueButton.GetComponent<Button>().interactable = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard kb = Keyboard.current; //current(デバイス情報)をkbに代入
        //if(kb != null)　//接続されているデバイス情報があれば
        //{
        //    if (kb.enterKey.wasPressedThisFrame) //Enterが押された瞬間
        //    {
        //        Load(); //シーン切り替え
        //    }
        //}

        //if (submitAction.WasPressedThisFrame())
        //{
        //    Load();
        //}
    }

    // シーンを読み込む
    public void Load()
    {
        //GameManager.totalScore = 0;
        SaveDataManager.Initialize(); //セーブデータを初期化する
        SceneManager.LoadScene(sceneName);
    }

    // シーンを読み込む
    public void ContinueLoad()
    {
        SaveDataManager.LoadGameData(); //セーブデータを読み込む
        SceneManager.LoadScene(sceneName);
    }
}
