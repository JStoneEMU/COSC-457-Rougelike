using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Dialogs;


public class Tutorial : MonoBehaviour
{
  
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
        DialogUI.Instance
        .SetTitle("How to play:")
        .SetMessage("1. Click to shoot \n 2. W, A, S, D to move \n 3. R to reload \n \n There are powerups, new guns and healthpacks scattered across each level. Good luck!")
        .Show();
        }
    }
}