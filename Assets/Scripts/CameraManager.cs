using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //カメラ制御
    public float camLeft = 0.0f;        // カメラ左スクロールリミット
    public float camRight = 0.0f;       // カメラ右スクロールリミット
    public float camTop = 0.0f;         // カメラ上スクロールリミット
    public float camBottom = 0.0f;      // カメラ下スクロールリミット

    GameObject player; //追随対象のプレイヤーオブジェクト
    PlayerController playerController; //プレイヤーコントローラー

    //サブ背景
    public GameObject subBack1;
    public GameObject subBack2;
    public float subBackScrollSpeed = 0.005f; //サブ背景スクロール速度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイヤー情報をGameObjectクラスのFindWithTagメソッドで探し出す
        player = GameObject.FindWithTag("Player");
        //取得してきたプレイヤー情報に付随するPlayerControllerコンポーネントを取得する
        playerController = player.GetComponent<PlayerController>();

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


        //サブ背景スクロール処理
        //もしも水平入力があったら
        if (playerController.GetAxisH() != 0)
        {
            //カメラのX座標がリミット内にある場合のみスクロール処理を行う
            if (x > camLeft && x < camRight)
            {
                subBack1.transform.localPosition -= new Vector3(playerController.GetAxisH() * subBackScrollSpeed, 0, 0);
                subBack2.transform.localPosition -= new Vector3(playerController.GetAxisH() * subBackScrollSpeed, 0, 0);

                //サブ背景のループ処理
                //右方向スクロール時
                if (playerController.GetAxisH() > 0)
                {
                    //もしもサブ背景1のローカルX座標が-19.2f以下になったら
                    if (subBack1.transform.localPosition.x <= -19.2f)
                    {
                        //誤差記録用
                        float diff = -19.2f - subBack1.transform.localPosition.x;
                        //サブ背景1のローカルX座標を19.2fから誤差調整した位置にワープする
                        subBack1.transform.localPosition = new Vector3(
                            19.2f - diff,
                            subBack1.transform.localPosition.y,
                            subBack1.transform.localPosition.z);
                    }
                    //サブ背景2についてもサブ背景1と同様の処理を行う
                    if (subBack2.transform.localPosition.x <= -19.2f)
                    {
                        float diff = -19.2f - subBack2.transform.localPosition.x;
                        subBack2.transform.localPosition = new Vector3(
                            19.2f - diff,
                            subBack2.transform.localPosition.y,
                            subBack2.transform.localPosition.z);
                    }
                }
                //左方向スクロール時
                if (playerController.GetAxisH() < 0)
                {
                    //もしもサブ背景1のローカルX座標が19.2f以上になったら
                    if (subBack1.transform.localPosition.x >= 19.2f)
                    {
                        //誤差記録用
                        float diff = subBack1.transform.localPosition.x - 19.2f;
                        //サブ背景1のローカルX座標を-19.2fから誤差調整した位置にワープする
                        subBack1.transform.localPosition = new Vector3(
                            -19.2f + diff,
                            subBack1.transform.localPosition.y,
                            subBack1.transform.localPosition.z);
                    }
                    //サブ背景2についてもサブ背景1と同様の処理を行う
                    if (subBack2.transform.localPosition.x >= 19.2f)
                    {
                        float diff = subBack2.transform.localPosition.x - 19.2f;
                        subBack2.transform.localPosition = new Vector3(
                            -19.2f + diff,
                            subBack2.transform.localPosition.y,
                            subBack2.transform.localPosition.z);
                    }
                }
            }
        }
    }
}
