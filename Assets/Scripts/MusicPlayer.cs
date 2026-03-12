using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip music;
    AudioSource _AudioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        _AudioSource.PlayOneShot(music);
    }
}
