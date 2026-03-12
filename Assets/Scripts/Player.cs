using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootOffsetTransform;

    public float speed = 10;
    public float maxDistance = 9.5f;

    public float deathDestroyDelay = 0.50f;

    Animator _animator;
    private bool _isDead = false;
    
    public AudioClip fireSound;
    public AudioClip deathSound;
    AudioSource _AudioSource;

    void Start()
    {
        _animator = GetComponent<Animator>();
        
        _AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_isDead) return;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GameObject shot = Instantiate(bulletPrefab, shootOffsetTransform.position, Quaternion.identity);
            
            Destroy(shot, 3f);
            
            _animator.SetTrigger("shoot");
            _AudioSource.PlayOneShot(fireSound);
        }

        if (Keyboard.current != null && Keyboard.current.leftArrowKey.isPressed)
        {
            Vector3 newPosition = transform.position + new Vector3(-speed, 0f, 0f) * Time.deltaTime;
            newPosition.x = Math.Clamp(newPosition.x, -maxDistance, maxDistance);
            transform.position = newPosition;
        }

        if (Keyboard.current != null && Keyboard.current.rightArrowKey.isPressed)
        {
            Vector3 newPosition = transform.position + new Vector3(speed, 0f, 0f) * Time.deltaTime;
            newPosition.x = Math.Clamp(newPosition.x, -maxDistance, maxDistance);
            transform.position = newPosition;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Stops double-triggers
        if (_isDead) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy Bullet"))
        {
            Destroy(collision.gameObject);

            _isDead = true;
            _animator.SetBool("isDead", true);
            _AudioSource.PlayOneShot(deathSound);

            Debug.Log("You Died.");
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(deathDestroyDelay);
        
        Destroy(gameObject);
        GameManager.Instance.GotoCredits();
    }
}