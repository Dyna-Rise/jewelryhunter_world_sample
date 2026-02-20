using UnityEngine;

public class BossController : MonoBehaviour
{
    public int hp = 10;
    public float reactionDistance = 10.0f;
    public GameObject bulletPrefab;
    public float shootSpeed = 5.0f;
    public float bossSpeed = 3.0f;
    Animator animator;
    GameObject player;

    public GameObject gate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            if (player != null)
            {
                Vector2 playerPos = player.transform.position;
                float dist = Vector2.Distance(transform.position, playerPos);
                animator.SetBool("InAttack", dist <= reactionDistance);
            }
            else
            {
                animator.SetBool("InAttack", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            ArrowController arrow = collision.gameObject.GetComponent<ArrowController>();
            hp -= arrow.attackPower;
            if (hp <= 0)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                animator.SetTrigger("IsDead");                
                Invoke("BossSpriteOff", 1.0f);
            }
        }
    }

    private void FixedUpdate()
    {
        float val = Mathf.Sin(Time.time);

        transform.position -= new Vector3(val * bossSpeed, 0, 0);
    }

    void Attack()
    {
        if (player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;
            float rad = Mathf.Atan2(dy, dx);
            float angle = rad * Mathf.Rad2Deg;

            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);

            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }

    void BossSpriteOff()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Invoke("BossDestroy", 3.0f);
    }

    void BossDestroy()
    {
        player.GetComponent<PlayerController>().Goal();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, reactionDistance);
    }


}
