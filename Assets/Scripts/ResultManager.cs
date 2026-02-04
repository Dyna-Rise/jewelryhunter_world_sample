using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = GameManager.totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // シーンを読み込む
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
