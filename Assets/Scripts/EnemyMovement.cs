using UnityEngine;
using System.Collections;

public class InvaderMove : MonoBehaviour
{
    public float step = 0.5f;
    private float down = 0.5f;
    public float delay = 0.5f;

    int direction = 1;

    void Start()
    {
        StartCoroutine(Move());
        Enemy.OnEnemyDied += OnEnemyDied;
    }

    IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            float nextX = transform.position.x + direction * step;

            if (nextX > 5f || nextX < -5f)
            {
                transform.position += Vector3.down * down;
                direction *= -1;
            }
            else
            {
                transform.position = new Vector3(nextX, transform.position.y, 0);
            }
        }
    }

    void OnEnemyDied(float score)
    {
        delay -= 0.05f;
    }
}