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

       if (movementDirection != Vector3.zero)
        {
            animator.SetBool("WalkForward", true);

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speedRotation * Time.deltaTime );
        }
        else
        {
            animator.SetBool("WalkForward", false);
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
