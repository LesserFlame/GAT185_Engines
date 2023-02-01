using UnityEngine;

[RequireComponent(typeof(CollisionEvent))]

public class GravityEffector : Interactable

{

	[SerializeField] Transform forceTransform;

	[SerializeField] float force;

	[SerializeField] bool oneTime = true;



	private void Start()

	{

		GetComponent<CollisionEvent>().onEnter = OnInteract;

		if (!oneTime) GetComponent<CollisionEvent>().onStay = OnInteract;

	}



	public override void OnInteract(GameObject target)

	{

		if (target.TryGetComponent<Rigidbody>(out Rigidbody rb))

		{

			ForceMode mode = (oneTime) ? ForceMode.Impulse : ForceMode.Force;

			rb.AddForce(forceTransform.rotation * Vector3.forward * force, mode);

		}

	}

}