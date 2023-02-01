using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveMovement : MonoBehaviour
{
	public float amplitude;
	public float rate;
	public float spinRate;

	Vector3 initialPosition;
	float time;
	float angle;

	//Vector3 posOffset = new Vector3();
	Vector3 tempPos = new Vector3();

	void Start()
	{
		time = Random.Range(0f, 5f);
		angle = Random.Range(0f, 360f);
		initialPosition = transform.position;
	}

	void Update()
	{
		TimeManager tm = FindObjectOfType<TimeManager>();
		float timeSpeed = tm.GetComponent<TimeManager>().timeSpeed;

		time += Time.deltaTime * rate;
		angle += Time.deltaTime * spinRate * timeSpeed;

		if (timeSpeed == 0) tempPos = transform.position;

		Vector3 offset = initialPosition;
		offset.y +=  Mathf.Sin(time * timeSpeed + initialPosition.y) * amplitude;
		offset.y = Mathf.Lerp(transform.position.y, offset.y, 0.01f);
		//Debug.Log(offset.y);
		if (timeSpeed != 0)transform.position = offset;
		else transform.position = tempPos;

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
	}
}
