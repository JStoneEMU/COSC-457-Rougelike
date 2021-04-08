using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuControl : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

}
