using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public int delay = 1;

    private bool ready = false;

    void Start()
    {
        Invoke("Ready", delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (ready && Input.anyKeyDown)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }       
    }

    private void Ready()
    {
        ready = true;
    }
}
