using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;              // Rigidbody2D型の変数
    float axisH = 0.0f;             // 入力
    public float speed = 3.0f;      // 移動速度
    public float jump = 9.0f;       // ジャンプ力
    public LayerMask groundLayer;   // 着地できるレイヤー
    bool goJump = false;            // ジャンプ開始フラグ
    bool onGround = false;          // 地面フラグ

    // アニメーション対応
    Animator animator; // アニメーター

    //値はあくまでアニメーションクリップ名
    public string stopAnime = "Idle"; 
    public string moveAnime = "Run";
    public string jumpAnime = "Jump";
    public string goalAnime = "Goal";
    public string deadAnime = "Dead";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0; //スコア

    InputAction moveAction; //Moveアクション
    InputAction jumpAction; //Jumpアクション
    PlayerInput input; //PlayerInputコンポーネント

    GameManager gm; //GameManager

    //体力の管理
    public static int playerLife = 10;

    //ダメージ管理
    bool inDamage = false;

    //矢の発射
    public float shootSpeed = 12.0f;
    public float shootDelay = 0.25f;
    public GameObject arrowPrefab;
    InputAction attackAction;
    public GameObject gate;

    //void OnLongPressStarted(InputAction.CallbackContext context)
    //{
    //    Debug.Log("Started:");
    //}
    //void OnLongPressPerformed(InputAction.CallbackContext context)
    //{
    //    Debug.Log("Performed:");
    //}
    //void OnAttackCallback(InputAction.CallbackContext context)
    //{
    //    if(GameManager.arrows > 0)
    //    {
    //        ShootArrow();
    //    }

    //    void ShootArrow()
    //    {
    //        GameManager.arrows--;
    //        Quaternion r;
    //        if(transform.localScale.x > 0) { 
    //            r = Quaternion.Euler(0, 0, 0);
    //        }
    //        else
    //        {
    //            r = Quaternion.Euler(0, 0, 180);
    //        }
    //        GameObject arrowObj = Instantiate(arrowPrefab, gate.transform.position,r);
    //        Rigidbody2D arrowRbody = arrowObj.GetComponent<Rigidbody2D>();
    //        arrowRbody.AddForce(new Vector2(transform.localScale.x,0) * shootSpeed, ForceMode2D.Impulse);

    //    }
    //}
    void OnAttack(InputValue value)
    {
        if (GameManager.arrows > 0)
        {
            ShootArrow();
        }        
    }

    void ShootArrow()
    {
        GameManager.arrows--;
        Quaternion r;
        if (transform.localScale.x > 0)
        {
            r = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            r = Quaternion.Euler(0, 0, 180);
        }
        GameObject arrowObj = Instantiate(arrowPrefab, gate.transform.position, r);
        Rigidbody2D arrowRbody = arrowObj.GetComponent<Rigidbody2D>();
        arrowRbody.AddForce(new Vector2(transform.localScale.x, 0) * shootSpeed, ForceMode2D.Impulse);
    }

    //ボタンを押したとき
    void OnSubmit(InputValue value)
    {
        //もしゲーム中でなければ
        if (GameManager.gameState != GameState.InGame)
        {
            gm.GameEnd();　//GameEndメソッドを発動してNext()かRestart()
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        axisH = moveInput.x; // X成分をaxisHに代入
    }

    void OnJump(InputValue value)
    {
        // ジャンプボタンが押されたときのみgoJumpをtrueにする
        // value.isPressed を使用して、ボタンが押された瞬間だけ処理を行う
        if (value.isPressed)
        {
            goJump = true; // ジャンプフラグを立てる
        }
    }

    static public void PlayerRecovery(int life)
    {
        playerLife += life;
        if(playerLife > 10)
        {
            playerLife = 10;
        }
    }

    //static public void PlayerDamage(int damage)
    //{
    //    playerLife -= damage;
    //    if(playerLife < 0)
    //    {
    //        playerLife = 0;
    //    }
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();   // Rigidbody2Dを取ってくる
        animator = GetComponent<Animator>();        // Animator を取ってくる
        nowAnime = stopAnime;                       // 停止から開始する
        oldAnime = stopAnime;                       // 停止から開始する

        input = GetComponent<PlayerInput>(); //PlayerInput取得
        moveAction = input.currentActionMap.FindAction("Move"); //Moveアクション情報を取得
        jumpAction = input.currentActionMap.FindAction("Jump"); //Jumpアクション情報を取得
        InputActionMap uiMap = input.actions.FindActionMap("UI"); //UIマップ取得
        uiMap.Disable(); //UIマップ無効化しておく

        gm = GameObject.FindFirstObjectByType<GameManager>();

        //体力をもとにもどす
        playerLife = 10;

        //Attackアクションにコールバック
        //PlayerInput aInput = GetComponent<PlayerInput>();
        //attackAction = aInput.currentActionMap.FindAction("Attack");
        //attackAction.started += OnAttack;
        //attackAction.started += OnLongPressStarted;
        //attackAction.performed += OnLongPressPerformed;
        //attackAction.canceled += OnAttackCallback;
    }
    void OnDisable()
    {
        if(attackAction != null)
        {
            //attackAction.started -= OnAttack;
            //attackAction.started -= OnLongPressStarted;
            //attackAction.performed -= OnLongPressPerformed;
            //attackAction.canceled -= OnAttackCallback;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.InGame || inDamage)
        {
            if (inDamage)
            {
                float val = Mathf.Sin(Time.time * 50);
                if(val > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            return;
        }

        // 地上判定
        onGround = Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        groundLayer);          // 検出するレイヤー
                                                               // キャラクターをジャンプさせる

        //---- Input Manager ----
        //if (Input.GetButtonDown("Jump"))
        //---- InputAction ----
        //if (jumpAction.WasPressedThisFrame())
        //{
        //    goJump = true; // ジャンプフラグを立てる
        //}


        //---- Input Manager ----
        //axisH = Input.GetAxisRaw("Horizontal");     //水平方向の入力をチェックする
        //---- InputAction ----
        //axisH = moveAction.ReadValue<Vector2>().x;


        if (axisH > 0.0f)                           // 向きの調整
        {
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }

        // アニメーション更新
        if (onGround)       // 地面の上
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime; // 停止中
            }
            else
            {
                nowAnime = moveAnime; // 移動
            }
        }
        else                // 空中
        {
            nowAnime = jumpAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // アニメーション再生
        }

        //ライフが0になったらゲームオーバー
        if(playerLife <= 0)
        {
            GameOver();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.gameState != GameState.InGame || inDamage)
        {
            return;
        }

        if (onGround || axisH != 0)     // 地面の上 or 速度が 0 ではない
        {
            //速度を更新する
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)         // 地面の上でジャンプキーが押された
        {
            // ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);          // ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    // 瞬間的な力を加える
            goJump = false;                                 // ジャンプフラグを下ろす
        }
    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();         // ゴール！！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     // ゲームオーバー
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // スコアアイテム
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();  // ScoreItemを得る			
            score = item.itemData.value;                // スコアを得る
            UIController ui = Object.FindFirstObjectByType<UIController>();      // UIControllerを探す
            if (ui != null)
            {
                ui.UpdateScore(score);                  // スコア表示を更新する
            }
            score = 0; //次に備えてスコアをリセット
            Destroy(collision.gameObject);              // アイテム削除する
        }
    }
    // ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        GameManager.gameState = GameState.GameClear;
        GameStop();             // ゲーム停止
    }
    // ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
        GameManager.gameState = GameState.GameOver;
        GameStop();             // ゲーム停止

        // ゲームオーバー演出
        GetComponent<CapsuleCollider2D>().enabled = false;      // 当たりを消す
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // 上に少し跳ね上げる

        Destroy(gameObject, 2.0f); // 2秒後にヒエラルキーからオブジェクトを抹消
    }
    // ゲーム停止
    void GameStop()
    {
        rbody.linearVelocity = new Vector2(0, 0);           // 速度を0にして強制停止

        //InputSystemのPlayerマップとUIマップの切り替え
        input.currentActionMap.Disable(); //いったん現状のPlayerマップを無効化
        input.SwitchCurrentActionMap("UI"); //アクションマップをUIに切り替え
        input.currentActionMap.Enable(); //UIマップを有効化
    }

    //プレイヤーのaxisH()の値を取得
    public float GetAxisH()
    {
        return axisH;
    }

    void GetDamage(GameObject target)
    {
        if(GameManager.gameState == GameState.InGame)
        {
            playerLife -= 1;
            if(playerLife > 0)
            {
                rbody.linearVelocity = new Vector2(0, 0);
                Vector3 v = (transform.position - target.transform.position).normalized;
                rbody.AddForce(new Vector2(v.x * 4, v.y * 4), ForceMode2D.Impulse);
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                GameOver();
            }
        }
    }

    void DamageEnd()
    {
        inDamage = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
