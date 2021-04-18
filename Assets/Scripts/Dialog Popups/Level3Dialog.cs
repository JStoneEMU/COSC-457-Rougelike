using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Dialogs;


public class Level3Dialog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogUI.Instance
        .SetTitle("Level 3: Sewers")
        .SetMessage("The deeper you go, the more enemies there are. There is something different down here, something malicious. You better find an exit quickly.")
        .Show();
    }

}

