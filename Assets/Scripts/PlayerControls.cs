using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    private float horizontalMovement;
    private float speed = 10.0f;
    private float jumpRange = 6.0f;
    private float jumpCooldown = 0f;
    private Animator mAnimator;
    public Transform attackPoint;
    private float attackRange = 0.9f;
    private float attackCooldown = 0f;
    public LayerMask enemyLayers;
    private float playerHealth = 100;
    [SerializeField] private HealthBar healthBar;
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    public void GetDamage()
    {
        playerHealth -= 10;
        healthBar.UpdateHealth(playerHealth, 100);
    }

    void PlayerWalk()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalMovement * speed * Time.deltaTime);
        mAnimator.SetFloat("Speed", Mathf.Abs(horizontalMovement) * speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }


    }

    void PlayerJump()
    {
        jumpCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && jumpCooldown < 0)
        {
            mAnimator.SetTrigger("Jump");
            gameObject.GetComponent<Animator>().Play("Jump_Player");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpRange), ForceMode2D.Impulse);
            jumpCooldown = 1.7f;

        }
    }

    void PlayerAttack()
    {
        attackCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F) && attackCooldown < 0)
        {
            mAnimator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyMovement>().GetDamage();
            }
            attackCooldown = 0.5f;
        }
    }

    void Update()
    {
        PlayerWalk();

        PlayerJump();

        PlayerAttack();
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
