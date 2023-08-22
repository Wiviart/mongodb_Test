using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction moveAction;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 100f;
    float angle = 0f;

    void Start()
    {
        moveAction.Enable();
    }

    void Update()
    {
        Movement();
        Rotation();

    }

    void Movement()
    {
        float inputZ = moveAction.ReadValue<Vector2>().y;
        transform.position += inputZ * moveSpeed * Time.deltaTime * transform.forward;
    }

    void Rotation()
    {
        float inputX = moveAction.ReadValue<Vector2>().x;
        angle += inputX * rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
