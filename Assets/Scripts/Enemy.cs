using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDiedFunc(int points);
    public static event EnemyDiedFunc OnEnemyDied;

    private int points;

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
        
        // Destroy the bullet
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            
            OnEnemyDied?.Invoke(points);
        }
        
        // todo - trigger death animation
    }
}
