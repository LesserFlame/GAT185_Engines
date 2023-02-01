using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Eyeball : MonoBehaviour
{
	public GameObject target;

	//[SerializeField, Range(0.1f, 50)] private float degreesPerSecond = 15.0f;
	[SerializeField, Range(0.1f, 10)] private float amplitude = 0.5f;
	[SerializeField, Range(0.1f, 5)] private float frequency = 1f;
	private float startFloat = 0;

	Vector3 posOffset = new Vector3();
	Vector3 tempPos = new Vector3();
	private void Start()
	{
		startFloat = Random.value * 5;
		posOffset = transform.position;
	}
	// Update is called once per frame
	void LateUpdate()
    {
		TimeManager tm = FindObjectOfType<TimeManager>();
		float timeSpeed = tm.GetComponent<TimeManager>().timeSpeed;

		tempPos = posOffset;
		tempPos.y += Mathf.Sin((Time.fixedTime * timeSpeed)* Mathf.PI * frequency + startFloat) * amplitude;
		tempPos.y = Mathf.Lerp(transform.position.y, tempPos.y, 0.01f);
		transform.position = tempPos;
		//gameObjects = flockPerception.GetGameObjects();
		//Debug.DrawRay(eye.transform.position, eye.transform.forward * 5, Color.blue);
		//var closest = Utilities.FindClosestObject(transform.position, gameObjects);

		Vector3 desiredLook;
		if (target != null)
		{
			desiredLook = target.transform.position - transform.position;
			//Debug.Log(closest.gameObject.name);
			Debug.DrawLine(transform.position, desiredLook);
		}
		else { desiredLook = transform.forward; }

		Quaternion rotation = Quaternion.LookRotation(desiredLook);
		if (timeSpeed != 0) transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2); 
	}
}
