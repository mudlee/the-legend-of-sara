using UnityEngine;

public class PlayerController : MonoBehaviour {
    private const float SPEED = 2f;
    private Animator _animator;
    private Rigidbody2D _rigidbody;

	void Start ()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _animator.SetBool("WalkRight", horizontal > 0);
        _animator.SetBool("WalkLeft", horizontal < 0);
        _animator.SetBool("WalkTop", vertical > 0);
        _animator.SetBool("WalkBottom", vertical < 0);

        _rigidbody.velocity = new Vector2(horizontal * SPEED, vertical * SPEED);
	}
}
