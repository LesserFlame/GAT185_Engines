using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class RollerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Range(2, 20)] private float distance;
    [SerializeField, Range(20, 80)] private float pitch;
    [SerializeField, Range(0.1f, 10)] private float sensitivity;

	[SerializeField] private Transform startingTransform;
    [SerializeField] private LayerMask layermask;
	private float yaw = 0;

	public void SetTarget(Transform target)
	{
		this.target = target;
        yaw = target.rotation.eulerAngles.y;
	}

	void Start()
    {
        
    }

    void LateUpdate()
    {
        if (target == null) return;
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -20, 89);

		Quaternion qyaw = Quaternion.AngleAxis(yaw, Vector3.up); // rotates around y axis (left, right)
        Quaternion qpitch = Quaternion.AngleAxis(pitch, Vector3.right); // rotates around x axis (up, down)
        Quaternion rotation = qyaw * qpitch;

        Ray ray = new Ray(target.transform.position, -(target.transform.position - transform.position));
        Debug.DrawRay(target.transform.position, -(target.transform.position - transform.position), Color.red);
        float tempDis = 0;
        if (Physics.SphereCast(target.transform.position, 0.5f, -(target.transform.position - transform.position), out RaycastHit hit, distance, layermask))
        {
            tempDis = Vector3.Distance(target.transform.position, hit.point) - 0.1f;
        } else tempDis = distance;
		Vector3 offset = rotation * Vector3.back * tempDis;
        transform.position = target.position + offset;
        transform.rotation = Quaternion.LookRotation(-offset);
    }

	public void ResetView()
	{
        transform.position = startingTransform.position;
        transform.rotation = startingTransform.rotation;
	}
}
