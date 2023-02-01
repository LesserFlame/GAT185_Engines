using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    public GameObject destroyFX;
    [SerializeField] float lifeSpan = 10;
    [SerializeField] bool explodes = false;
    void Update()
    {
        var timespeed = FindObjectOfType<TimeManager>().GetComponent<TimeManager>().timeSpeed;
        lifeSpan -= Time.deltaTime * timespeed;
        if (lifeSpan <= 0)
        {			
            OnExplode();
        }
    }
    public void OnExplode()
    {
		var timespeed = FindObjectOfType<TimeManager>().GetComponent<TimeManager>().timeSpeed;
        if (destroyFX != null)
        {
            Instantiate(destroyFX, transform.position, Quaternion.identity);
            //var player = FindObjectOfType<RollerPlayer>();
            //if (player != null) destroyFX.GetComponent<AudioSource>().volume = 1 / Vector3.Distance(transform.position, player.transform.position);
        }
        if (explodes && timespeed != 0) 
        {
			Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
			foreach (Collider hit in colliders)
			{
                //Debug.Log(hit.gameObject);
				Rigidbody rb = hit.GetComponent<Rigidbody>();

				if (rb != null) rb.AddExplosionForce(500, transform.position, 10, 0.1f);
                if (hit.TryGetComponent<Health>(out Health health)) health.OnApplyDamage(20);
                
			}
		}
		Destroy(gameObject);

    }
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
        {
            if (TryGetComponent<AudioSource>(out AudioSource source)) source.Play();
            OnExplode();
        }
	}
}
