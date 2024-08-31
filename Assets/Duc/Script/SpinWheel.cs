using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpinWheel: MonoBehaviour
{
	public AnimationCurve animationCurve;
	private bool spinning;

 	void Start()
	{
		spinning = false;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0) && !spinning) 
		{
			StartCoroutine(DoSpin(10f, Random.Range(2000,3000)));
		}
	}

	public IEnumerator DoSpin(float time, float angle)
	{
		spinning = true;
		float timer = 0;
		float startAngle = transform.eulerAngles.z;

		while (timer<time) 
		{
			float endAngel = animationCurve.Evaluate(timer/time) * angle;
			transform.eulerAngles = new Vector3(0f, 0f,-(endAngel + startAngle));
			timer += Time.deltaTime;
			yield return 0;
		}

		spinning = false;
	}
}