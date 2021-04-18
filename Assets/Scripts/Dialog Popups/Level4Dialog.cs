using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Dialogs;


public class Level4Dialog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogUI.Instance
        .SetTitle("Level 4: The Escape")
        .SetMessage("You've gone through hell and you can see the other side. There is a way out! Only one problem... the horde of enemies between you and the exit. You know what to do.")
        .Show();

    }

}
