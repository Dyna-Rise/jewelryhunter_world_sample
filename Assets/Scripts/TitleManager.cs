using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string sceneName;            // 読み込むシーン名

    //public InputAction submitAction; //InputAction

    //void OnEnable()
    //{
    //    submitAction.Enable(); //InputActionを有効化
    //}
    //void OnDisable()
    //{
    //    submitAction.Disable(); //InputActionを無効化
    //}

    void OnSubmit(InputValue value)
    {
        Load();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        GameManager.totalScore = 0;
        SceneManager.LoadScene(sceneName);
    }
}
