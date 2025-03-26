using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody2D bodyRb;
    Rigidbody2D feetRb;
    public float speed = 4.5f;
    public float jumpForce = 10f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private LayerMask groundLayer;


    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public void setIsGrounded(bool value)
    {
        isGrounded = value;
    }

    void Start()
    {
        bodyRb = transform.Find("Body").GetComponent<Rigidbody2D>();
        bodyRb.freezeRotation = true;

        feetRb = transform.Find("Feet").GetComponent<Rigidbody2D>();
        feetRb.freezeRotation = true;
    }

    void Update()
    {
        // Mejora la detección de suelo
        this.setIsGrounded(Physics2D.Raycast(bodyRb.position, Vector2.down, 0.6f, groundLayer));

        Debug.Log("Is grounded: " + isGrounded + " " + groundLayer);

        float horizontalInput = Input.GetAxis("Horizontal");
        bool isJumping = Input.GetKeyDown(KeyCode.UpArrow);

        bodyRb.velocity = new Vector2(horizontalInput * speed, bodyRb.velocity.y);

        if (isJumping && isGrounded)
        {
            bodyRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}