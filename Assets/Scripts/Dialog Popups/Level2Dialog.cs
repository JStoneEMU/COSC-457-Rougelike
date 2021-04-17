using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Dialogs;


public class Level2Dialog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogUI.Instance
        .SetTitle("Level 2: Underground")
        .SetMessage("Suddenly, you're in an underground maze. It's crawling with enemies, and you need to make it through... maybe you'll need more than this pistol. You should look around for something with a little more power.")
        .Show();
    }

}
