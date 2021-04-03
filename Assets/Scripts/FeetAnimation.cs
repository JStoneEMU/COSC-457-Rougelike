using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetAnimation : MonoBehaviour
{
    Vector2 movement;

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //Left input gives -1, right givs 1, nothing gives 0
        movement.y = Input.GetAxisRaw("Vertical");

 

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Verticle", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }
}
