using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform Player;
	public Transform Marker;

	public float MinCameraSize = 5f;
	public float MaxCameraSize = 15f;

	public float StartCameratHeight = 2f;
	//Высота, на которой перс обычно отлетает от поверхности
	public float FlyOffHeight = 5f;

	private float maxCameraHeight = 12f; 
	private float distanceFromPlayerToCenterOfCameraByX = 10f;

	private float currentCameraHeight;
	private float relativeHeightOfCamera;

    private Camera playerCamera;
	private Rigidbody2D playerRgBody2d;

	void Start()
    {
		playerCamera = GetComponent<Camera>();
		distanceFromPlayerToCenterOfCameraByX = transform.position.x - Marker.position.x;
		maxCameraHeight = Player.GetComponent<PlayerController>().MaxHeight - (Marker.position.y - transform.position.y);
		playerRgBody2d = Player.GetComponent<Rigidbody2D>();
	}


	private void Update()
	{
		transform.position = new Vector3(
			(float) Math.Round(transform.position.x, 2), 
			(float)Math.Round(transform.position.y, 2), 
			transform.position.z);
		currentCameraHeight = transform.position.y;

		// Чтобы камера держалась впереди на расстояние
		if ((transform.position.x - Player.position.x) < distanceFromPlayerToCenterOfCameraByX)
		{
			transform.position = new Vector3(Player.position.x + distanceFromPlayerToCenterOfCameraByX, transform.position.y, transform.position.z);
		}
		// если выше планки
		if (Player.position.y > FlyOffHeight)
		{
			relativeHeightOfCamera = (currentCameraHeight - StartCameratHeight) / (maxCameraHeight - StartCameratHeight);
			float temp = (MinCameraSize + (MaxCameraSize - MinCameraSize) * relativeHeightOfCamera);

			// relativeHeightOfCamera немного косоёбит тудаю-сюда, поэтопу проверочка, чтобы шло в одну сторону только

			if (playerRgBody2d.velocity.y > 0 && playerCamera.orthographicSize < temp)
				playerCamera.orthographicSize = temp;

			if (playerRgBody2d.velocity.y < 0 && playerCamera.orthographicSize > temp)
				playerCamera.orthographicSize = temp;

			float scalingCoeff = playerCamera.orthographicSize / MinCameraSize;
			transform.localScale = new Vector3(scalingCoeff, scalingCoeff, 1);

			transform.position = new Vector3(
				Player.position.x + (transform.position.x - Marker.position.x),
				Player.position.y - (Marker.position.y - transform.position.y),
				transform.position.z
				);

			if (relativeHeightOfCamera >= 1)
				playerCamera.orthographicSize = MaxCameraSize;
		}
		//если ниже
		else
		{
			transform.position = new Vector3(transform.position.x, StartCameratHeight, transform.position.z);
			playerCamera.orthographicSize = MinCameraSize;
			transform.localScale = new Vector3(1, 1, 1);
		}
	}
}
