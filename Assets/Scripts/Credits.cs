using System.Collections;
using UnityEngine;

public class Credits : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(5f);
        
        GameManager.Instance.GotoMainMenu();
    }
}
