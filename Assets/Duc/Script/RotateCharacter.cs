using UnityEngine;
using System.Collections;

public class RotateCharacter : MonoBehaviour 
{
	private int speed = 10;
	private float lerpSpeed = 3f;

	private float yDeg;
	private float xDeg;
	private Quaternion fromRotation;
	private Quaternion toRotation;
	private bool isDragging = false;

	void OnMouseDown() 
	{		
		isDragging = true;
	}
	void Update()
	{
		if (Input.GetMouseButton (0)&& isDragging) 
		{
			xDeg -= Input.GetAxis("Mouse X")* speed;
			fromRotation = transform.rotation;
			toRotation = Quaternion.Euler(yDeg, xDeg, 0);
			transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);
		}
	}
}