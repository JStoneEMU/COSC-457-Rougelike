using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Uses SeekAI component to find target, then shoots it

public class ShootEnemy : MonoBehaviour
{
    private SeekAI seekComponent;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        seekComponent = GetComponent<SeekAI>();
        if (seekComponent != null)
            target = seekComponent.target;
    }

    void Update()
    {
        if (target != null && seekComponent.CurrentState == SeekAI.State.Found)
        {
            Vector2 pos = transform.position;
            Vector2 targetPos = target.transform.position;
            Vector2 attackAngle = targetPos - pos;
            print("Shooting at: " + attackAngle);
        }
    }

}
