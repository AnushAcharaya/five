using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 look;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float movement = 5f;
    [SerializeField] float mouseSensitivity = 3f;
    [SerializeField] float mass = 1f;
    [SerializeField] float jumSpeed = 5f;
    CharacterController characterController;
    Vector3 velocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGravity();
        UpdateMovement();
        UpdateLook();
    }

    void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;

        velocity.y = characterController.isGrounded ? -1f : velocity.y + gravity.y;
    }

    void UpdateMovement()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var input = new Vector3();
        input += transform.forward * y;
        input += transform.right * x;

        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            velocity.y = jumSpeed;
        }
        //transform.Translate(input * movement * Time.deltaTime, Space.World);
        characterController.Move((input * movement + velocity) * Time.deltaTime);
    }

    void UpdateLook()
    {
        look.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        look.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
        look.y = Mathf.Clamp(look.y, -89f, 89f);
        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);

    }
}