using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Dialogs;


public class Level1Dialog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogUI.Instance
        .SetTitle("Aliens have invaded!!")
        .SetMessage("You wake up disoriented in a hospital to the sounds of a horde of aliens. With your trusty pistol, you must find a way out of here. I heard there was a staircase somewhere here that leads... down.")
        .Show();

        DialogUI.Instance
        .SetTitle("How to play:")
        .SetMessage("1. Click to shoot \n 2. W, A, S, D to move \n 3. R to reload \n \n There are powerups, new guns and healthpacks scattered across each level. Good luck!")
        .Show();
    }

}
