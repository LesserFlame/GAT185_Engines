using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeComponent : MonoBehaviour
{
    //public bool isFrozen = false;
    private Vector3 currentMotion;
    private Vector3 futureMotion;
    private Vector3 currentPosition;
    private Quaternion currentRotation;
    //private Vector3 
    public void StopTime ()
    {
        futureMotion = Vector3.zero;
        if(TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
			//currentMotion = rb.GetPointVelocity(transform.position);
			currentMotion = rb.velocity;

			rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        currentPosition = transform.position;
        currentRotation = transform.rotation;
		//Debug.Log("Freeze");
    }
    public void StartTime()
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
			//rb.velocity = Vector3.zero;
			rb.velocity = currentMotion;
            rb.AddForce(futureMotion, ForceMode.Impulse);
            futureMotion = Vector3.zero;
            rb.useGravity = true;
        }
		
	}
	private void Update()
	{
        TimeManager tm = FindObjectOfType<TimeManager>();
        float timeSpeed = tm.GetComponent<TimeManager>().timeSpeed;

        if (timeSpeed == 0)
        {
            if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
				transform.position = currentPosition;
				transform.rotation = currentRotation;
			}
		    //if (TryGetComponent<AudioSource>(out AudioSource audioSource))
      //      {
      //          audioSource.Pause();
      //      }
		}
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
		{
			audioSource.pitch = Mathf.Lerp(audioSource.pitch, timeSpeed, 0.2f);
		}
		if (TryGetComponent<Animator>(out Animator animator))
		{
			animator.speed = Mathf.Lerp(animator.speed, timeSpeed, 0.2f);
		}
		if (TryGetComponent<ParticleSystem>(out ParticleSystem particles))
		{
			particles.playbackSpeed = Mathf.Lerp(particles.playbackSpeed, timeSpeed, 0.2f);
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
        //Debug.Log("X: " + futureMotion.x + "Y: " + futureMotion.y + "Z: " + futureMotion.z);
        futureMotion += (collision.relativeVelocity);
	}
}
