using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{

    public Vector2 firstPoint;
    public Vector2 secondPoint;
    private float enemySpeed = 3.0f;
    private Vector2 point;
    private bool inPosition = false; // false - двигается в позицию secondPoint true - двигается в позицию firstPoint
    private float enemyHealth = 100;
    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        transform.position = firstPoint; //  Устанавливает стартовую позицию противника
        point = secondPoint; // Устанавливает вектор движения
        healthBar = GetComponentInChildren<HealthBar>();
    }

    public void GetDamage()
    {
        enemyHealth -= Random.Range(25, 40);
        healthBar.UpdateHealth(enemyHealth, 100);
    }

    float GetSumOfCoords(Vector3 vector)
    {
        return vector.x + vector.y;
    }

    Vector2 SetPoint(bool inPosition)
    {
        if (inPosition)
        {
            return firstPoint;
        }
        else
        {
            return secondPoint;
        }
    }

    bool CompareX(float firstX, float secondX)
    {
        if (firstX >= secondX)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void SetScale(bool inPosition)
    {
        if (CompareX(firstPoint.x, secondPoint.x) && inPosition)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }

    void AttackPlayer()
    {

    }


    void Update()
    {
        SetScale(inPosition);

        if (Vector3.Distance(transform.position, point) < 0.1f)
        {
            inPosition = !inPosition;
            point = SetPoint(inPosition);
        }
        if (GetSumOfCoords(firstPoint) != 0 || GetSumOfCoords(secondPoint) != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, enemySpeed * Time.deltaTime);
        }
        if (enemyHealth <= 0)
        {
            Debug.Log("Death Animation");
        }
    }
}
