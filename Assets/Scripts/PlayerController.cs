using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedChar = 15;
    [SerializeField] private float speedRotation = 7;
    private float horizontalInput;
    private float verticalInput;

    [SerializeField] private Transform cameraTransform;

    private Animator animator;
    private Rigidbody playerRb;
    private bool walking;
    private bool jumping;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude) * speedChar;

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        transform.Translate(movementDirection * inputMagnitude * Time.deltaTime, Space.World);

       if (movementDirection != Vector3.zero )
        {
            if (verticalInput > 0)
            {
                animator.SetBool("WalkForward", true);
                animator.SetBool("WalkBackward", false);
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speedRotation * Time.deltaTime);

                walking = true;
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    speedChar *= 4;
                    animator.SetBool("WalkForward", false);
                    animator.SetBool("RunForward", true);
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    speedChar /= 4;
                    animator.SetBool("WalkForward", true);
                    animator.SetBool("RunForward", false);
                }
            }
            else if (verticalInput < 0)
            {
                animator.SetBool("WalkForward", false);
                animator.SetBool("WalkBackward", true);
                Quaternion toRotation = Quaternion.LookRotation(-movementDirection, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speedRotation * Time.deltaTime);
                walking = true;
            }
            else
            {
                animator.SetBool("WalkForward", false);
                animator.SetBool("WalkBackward", false);
                walking = false;
            }
        }
        
        else
        {
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkBackward", false);
        }


    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
