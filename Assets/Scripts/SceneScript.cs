using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public int sceneNumber;
    void OnTriggerEnter2D()
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
