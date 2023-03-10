using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public GameObject interactFX;
	public bool destroyOnInteract = true;
	public bool deactivateOnInteract = false;

	public abstract void OnInteract(GameObject target);
}
