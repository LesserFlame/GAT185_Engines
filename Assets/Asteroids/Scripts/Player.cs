using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(1, 100)] public float speed = 5;
    [Range(1, 360)] public float rotationRate = 180;
    public GameObject prefab;
    public Transform[] bulletSpawnLocation;
    private void Awake()
    {
        Debug.Log("awake");
    }

    void Start()
    {
        Debug.Log("start");  ///unity for cout  
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;
        direction.z = Input.GetAxis("Vertical");

        Vector3 rotation = Vector3.zero;
        rotation.y = Input.GetAxis("Horizontal");

        Quaternion rotate = Quaternion.Euler(rotation * rotationRate * Time.deltaTime);
        transform.rotation = transform.rotation * rotate;
        transform.Translate(direction * speed * Time.deltaTime);
        //Debug.Log(direction.x);
        //transform.position += direction * speed * Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
            //GetComponent<AudioSource>().Play();

            for (int i = 0; i < bulletSpawnLocation.Length; i++)
            {
                GameObject go = Instantiate(prefab, bulletSpawnLocation[i].position, bulletSpawnLocation[i].rotation);
            }
        }

    }
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			FindObjectOfType<AsteroidGameManager>()?.SetGameOver();
		}
	}
}
