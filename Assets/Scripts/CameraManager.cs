using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //カメラ制御
    public float camLeft = 0.0f;        // カメラ左スクロールリミット
    public float camRight = 0.0f;       // カメラ右スクロールリミット
    public float camTop = 0.0f;         // カメラ上スクロールリミット
    public float camBottom = 0.0f;      // カメラ下スクロールリミット

    GameObject player; //追随対象のプレイヤーオブジェクト

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイヤー情報をGameObjectクラスのFindWithTagメソッドで探し出す
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //カメラ制御
        float x;
        float y;
        // プレイヤーの位置をカメラの位置にする（ただしClampで制限をかける）
        x = Mathf.Clamp(player.transform.position.x, camLeft, camRight);
        y = Mathf.Clamp(player.transform.position.y, camBottom, camTop);
        Vector3 camPos = new Vector3(x, y, -10);        // カメラ位置のVector3を作る
        Camera.main.transform.position = camPos;        // カメラの更新座標
    }
}
