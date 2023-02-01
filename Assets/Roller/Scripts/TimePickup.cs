using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionEvent))]
public class TimePickup : Interactable
{
	[SerializeField] private float time = 10;

	void Start()
	{
		GetComponent<CollisionEvent>().onEnter += OnInteract;

	}
	public override void OnInteract(GameObject target)
	{
		var gm = FindObjectOfType<RollerGameManager>();
		if (gm != null)
		{
			gm.AddTime(time);
		}
		if (interactFX != null) Instantiate(interactFX, transform.position, Quaternion.identity);
		if (destroyOnInteract) Destroy(gameObject);
		if (deactivateOnInteract) gameObject.SetActive(false);
	}

	// Start is called before the first frame update
}
