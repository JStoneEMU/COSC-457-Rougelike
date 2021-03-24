using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    
    Vector2 movement;

    // Update is called once per frame
    void Update() //Put all input in here
    {
        
        movement.x = Input.GetAxisRaw("Horizontal"); //Left input gives -1, right givs 1, nothing gives 0
        
        if(Input.GetAxisRaw("Horizontal") < 0) {
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        if(Input.GetAxisRaw("Horizontal") > 0) {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        movement.y = Input.GetAxisRaw("Vertical");
        if(Input.GetAxisRaw("Vertical") < 0) {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if(Input.GetAxisRaw("Vertical") > 0) {
            transform.localRotation = Quaternion.Euler(0, 0, -180);
        }
        
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);//Time.fixedDeltaTime ensures our movement speed is consistent no matter how many times FixedUpdate is called
    }

    private void OnTriggerEnter2D(Collider2D other) //This doodad is for picking up items
    {
        if (other.gameObject.CompareTag("Item"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
