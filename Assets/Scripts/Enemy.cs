using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDiedFunc(int points);
    public static event EnemyDiedFunc OnEnemyDied;

    private int points;
    
    public float deathDestroyDelay = 0.35f;

    private Animator _animator;
    private Collider2D _collider;
    private bool dying = false;
    
    public AudioClip deathSound;
    AudioSource _AudioSource;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        
        _AudioSource = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        if (gameObject.CompareTag("10 Points"))
        {
            points = 10;
        } else if (gameObject.CompareTag("20 Points"))
        {
            points = 20;
        } else if (gameObject.CompareTag("30 Points"))
        {
            points = 30;
        } else if (gameObject.CompareTag("100 Points"))
        {
            points = 100;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ouch!");

        if (dying) return;
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Destroy(collision.gameObject);
            
            dying = true;
            if (_collider != null)
            {
                _collider.enabled = false;
            }

            if (_animator != null && _animator.runtimeAnimatorController != null)
            {
                _animator.SetBool("isDead", true);
                _AudioSource.PlayOneShot(deathSound);
            }
            
            OnEnemyDied?.Invoke(points);
            StartCoroutine(DestroyAfterDelay());
        }
    }
    
    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(deathDestroyDelay);
        Destroy(gameObject);
    }
}
