using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
	public float Force = 10f;
	private Rigidbody2D rigidbody2D;

	public float Speed_ms;
	public float Speef_curr;

	private float horizontal;
	private float vertical;
	private Vector2 direction;
	private bool move = false;

    void Start()
    {
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	private void Update()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		direction = new Vector3(horizontal, vertical, 0);
		if (Input.GetMouseButtonDown(0)) move = true;
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		Speef_curr = rigidbody2D.velocity.magnitude;

		if (move)
		{
			move = false;
		}
			rigidbody2D.AddForce(direction * Force);
	}
}
