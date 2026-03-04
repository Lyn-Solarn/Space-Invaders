using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootOffsetTransform;

    public float speed;
    public float maxDistance;
    
    Animator _animator;

    void Start()
    {
        // todo - get and cache animator
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GameObject shot = Instantiate(bulletPrefab, shootOffsetTransform.position, Quaternion.identity);
            Debug.Log("Bang!");

            // todo - destroy the bullet after 3 seconds
            Destroy(shot, 3f);
            // todo - trigger shoot animation
        }

        if (Keyboard.current != null && Keyboard.current.leftArrowKey.isPressed)
        {
            Vector3 newPosition = transform.position += new Vector3(-speed, 0f, 0f) * Time.deltaTime;
            newPosition.x = Math.Clamp(newPosition.x, -maxDistance, maxDistance);
            transform.position = newPosition;
        }
        
        if (Keyboard.current != null && Keyboard.current.rightArrowKey.isPressed)
        {
            Vector3 newPosition = transform.position += new Vector3(speed, 0f, 0f) * Time.deltaTime;
            newPosition.x = Math.Clamp(newPosition.x, -maxDistance, maxDistance);
            transform.position = newPosition;
        }
    }
}
