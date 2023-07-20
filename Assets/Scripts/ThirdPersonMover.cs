using UnityEngine;
using UnityEngine.UIElements;

public class ThirdPersonMover : MonoBehaviour
{
    [SerializeField] float _turnSpeed = 1000f;
    [SerializeField] float _moveSpeed = 5f;

    Rigidbody _rigidbody;
    Animator _animator;
    float _mouseMovement;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _mouseMovement += Input.GetAxis("Mouse X");
    }

    void FixedUpdate()
    {
        transform.Rotate(0, _mouseMovement * Time.deltaTime * _turnSpeed, 0);
        _mouseMovement = 0f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
            vertical *= 2f;

        var velocity = new Vector3(horizontal, 0, vertical);
        velocity *= _moveSpeed * Time.fixedDeltaTime;
        Vector3 offset = transform.rotation * velocity;
        _rigidbody.MovePosition(transform.position + offset);

        _animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
        _animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
    }
}