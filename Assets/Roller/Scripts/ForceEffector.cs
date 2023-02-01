using UnityEngine;

[RequireComponent(typeof(CollisionEvent))]

public class ForceEffector : Interactable

{

	[SerializeField] Transform forceTransform;

	[SerializeField] float force;

	[SerializeField] bool oneTime = true;
	[SerializeField] bool effectGravity = false;



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

			if(effectGravity && target.TryGetComponent<RollerPlayer>(out RollerPlayer player)) 
			{
				player.gravityDirection = forceTransform.rotation * Vector3.forward;
				//rb.useGravity = false;
			}
		}

	}

}