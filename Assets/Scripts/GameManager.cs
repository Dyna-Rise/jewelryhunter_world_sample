using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState           // ゲームの状態
{
    InGame,                     // ゲーム中
    GameClear,                  // ゲームクリア
    GameOver,                   // ゲームオーバー
    GameEnd,                    // ゲーム終了
}

public class GameManager : MonoBehaviour
{
    // ゲームの状態
    public static GameState gameState;
    public string nextSceneName;            // 次のシーン名

    // スコア追加
    public static int totalScore;       // 合計スコア

    AudioSource soundPlayer; //AudioSource
    public AudioClip meGameClear; //ゲームクリア
    public AudioClip meGameOver; //ゲームオーバー

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.InGame;               // ゲーム中にする

        soundPlayer = GetComponent<AudioSource>(); //AudioSourceコンポーネントの取得
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameState);
        //もしゲームクリア状態なら
        if (gameState == GameState.GameClear)
        {
            Debug.Log("クリア");
            soundPlayer.Stop(); //一度曲を止める
            soundPlayer.PlayOneShot(meGameClear); //一度だけ鳴らす
            GameManager.gameState = GameState.GameEnd;
        }
        else if(gameState == GameState.GameOver)  //もしゲームオーバー状態なら
        {
            Debug.Log("オーバー");
            soundPlayer.Stop();//一度曲を止める
            soundPlayer.PlayOneShot(meGameOver); //一度だけ鳴らす
            GameManager.gameState = GameState.GameEnd;

        }
    }

    //リスタート
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //次へ
    public void Next()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
