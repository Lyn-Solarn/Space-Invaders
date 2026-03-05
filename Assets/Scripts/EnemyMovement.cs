using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class InvaderMove : MonoBehaviour
{
    public float step = 0.5f;
    private float down = 0.5f;
    public float delay = 0.5f;
    
    public GameObject bulletPrefab;
    public float shootChance = 0.2f;
    public float bulletSpawnYOffset = -0.2f;

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

            if (nextX > 3.5f || nextX < -3.5f)
            {
                transform.position += Vector3.down * down;
                direction *= -1;
            }
            else
            {
                transform.position = new Vector3(nextX, transform.position.y, 0);
            }
            
            TryShoot();
        }
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDied -= OnEnemyDied;
    }

    void OnEnemyDied(int score)
    {
        delay = Mathf.Max(0.2f, delay - 0.05f);
        StartCoroutine(CheckDestroyedNextFrame());
    }

    IEnumerator CheckDestroyedNextFrame()
    {
        yield return null; // wait one frame so the enemy can be destroyed
        CheckIfAllDestroyed();
    }

    void TryShoot()
    {
        if (Random.value > shootChance) return;

        Transform shooter = GetRandomEnemyTransform();
        if (shooter == null) return;

        Vector3 spawnPos = shooter.position + new Vector3(0f, bulletSpawnYOffset, 0f);
        Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
    }
    
    Transform GetRandomEnemyTransform()
    {
        List<Transform> alive = new List<Transform>();

        foreach (Transform child in transform)
        {
            if (child != null && child.gameObject.activeInHierarchy)
            {
                alive.Add(child);
            }
        }

        if (alive.Count == 0) return null;

        int index = Random.Range(0, alive.Count);
        return alive[index];
    }
    
    void CheckIfAllDestroyed()
    {
        Enemy[] enemies = GetComponentsInChildren<Enemy>(true);

        if (enemies.Length > 0)
            return;

        Debug.Log("All Invaders Destroyed!");
        StopAllCoroutines();
        GameManager.Instance.GotoMainMenu();
    }
}