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
    
    private List<Animator> animators = new List<Animator>();
    private bool stepToggle = false;
    
    public AudioClip fireSound;
    AudioSource _AudioSource;

    int direction = 1;

    void Start()
    {
        CollectAnimators();
        StartCoroutine(Move());
        Enemy.OnEnemyDied += OnEnemyDied;
        
        _AudioSource = GetComponent<AudioSource>();
    }

    void CollectAnimators()
    {
        animators.Clear();

        foreach (var animator in GetComponentsInChildren<Animator>(true))
        {
            if (animator != null && animator.runtimeAnimatorController != null)
                animators.Add(animator);
        }
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

            // Toggle animation boolean every move
            stepToggle = !stepToggle;
            SetStepBoolOnAll(stepToggle);

            TryShoot();
        }
    }
    
    void SetStepBoolOnAll(bool value)
    {
        // Removes enemies that were destroyed
        for (int i = animators.Count - 1; i >= 0; i--)
        {
            if (animators[i] == null)
            {
                animators.RemoveAt(i);
                continue;
            }

            animators[i].SetBool("step", value);
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
        
        Animator animator = shooter.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("shoot");
            _AudioSource.PlayOneShot(fireSound);
        }

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