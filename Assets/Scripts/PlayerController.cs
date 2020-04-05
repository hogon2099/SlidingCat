using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float MaxHeight = 20f;
	public float VerticalForce = 10;
	public float MinimalHorizontalSpeed = 2f;
	public float currentSpeed = 0;

	public LayerMask WhatIsGround;
	public float GroundCheckerRadius;

	private Rigidbody2D rgbody2d;
	private SpriteRenderer sprite;
	[SerializeField] private bool isOnGround;
	private bool isMovingAllowed = false;

	private void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		rgbody2d = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
		{
			isMovingAllowed = true;
			sprite.color = Color.black;
		}
		else
		{
			isMovingAllowed = false;
			sprite.color = Color.white;
		}
	}

	private void FixedUpdate()
	{
		isOnGround = Physics2D.OverlapCircle(transform.position, 1f, WhatIsGround);

		currentSpeed = rgbody2d.velocity.magnitude;

		if (rgbody2d.velocity.x < MinimalHorizontalSpeed)
			rgbody2d.velocity = (Vector2.right * MinimalHorizontalSpeed);

		if (transform.position.y >= MaxHeight - 2f)
			rgbody2d.AddForce((Vector2.down * (VerticalForce / 2)));

		if (isMovingAllowed)
			if (isOnGround)
				rgbody2d.AddForce(Vector2.down * VerticalForce);
			else rgbody2d.AddForce(Vector2.down * VerticalForce / 2);

	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, GroundCheckerRadius);
	}
}
