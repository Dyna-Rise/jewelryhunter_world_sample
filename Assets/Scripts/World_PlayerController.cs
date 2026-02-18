using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

//どちらを向いているか
public enum Direction
{
    none,
    left,
    right
}

public class World_PlayerController : MonoBehaviour
{
    [Header("プレイヤーの移動力")]
    public float speed = 3.0f; //移動スピード係数

    Vector2 moveVec;　//InputSystemからの入力値
    float angleZ;　//Playerの向き
    Rigidbody2D rbody;
    Animator animator;

    bool isActionButtonPressed;
    public bool IsActionButtonPressed
    {
        get { return isActionButtonPressed; }
        set { isActionButtonPressed = value; }
    }

    void OnActionButton(InputValue value)
    {
        IsActionButtonPressed = value.isPressed; // ボタンが押され続けている間はtrue
    }

    //InputSystemの入力値moveVecを活用してPlayerの向きを導く
    float GetAngle()
    {
        float angle = angleZ;
        if(moveVec != Vector2.zero)
        {
            float rad = Mathf.Atan2(moveVec.y,moveVec.x);
            angle = rad * Mathf.Rad2Deg;
        }
        return angle;
    }

    //自作型Directionを返す。変数angleZをもとにPlayerがどの向きかを示す。
    Direction AngleToDirection()
    {
        Direction dir;
        if(angleZ >= -89 && angleZ <= 89)
        {
            dir = Direction.right;
        }
        else if(angleZ >= 91 && angleZ <= 269)
        {
            dir = Direction.left;
        }
        else
        {
            dir = Direction.none;
        }
            return dir;
    }

    //Moveアクションの値（Vector2)を変数moveVecに取得
    void OnMove(InputValue value)
    {
        moveVec = value.Get<Vector2>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        angleZ = GetAngle();
        Direction dir = AngleToDirection();

        if(dir == Direction.right)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (moveVec != Vector2.zero)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

    }

    void FixedUpdate()
    {
        rbody.linearVelocity = moveVec * speed;    
    }
}
