using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public GameObject player;
    public int sceneNumber;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == player)
            SceneManager.LoadScene(sceneNumber);
    }
}
