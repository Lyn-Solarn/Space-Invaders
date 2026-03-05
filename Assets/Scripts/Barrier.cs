using System;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private int health = 3;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy Bullet"))
        {
            Destroy(collision.gameObject);
            health--;
            SpriteDamageUpdate();
        }
    }

    void SpriteDamageUpdate()
    {
        if (health == 2)
        {
            _spriteRenderer.color = new Color32(130, 130, 130, 255);
        }
        else if (health == 1)
        {
            _spriteRenderer.color = new Color32(50, 50, 50, 255);
        }
    }
}
