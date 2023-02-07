using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
	[SerializeField] AudioSource audioSource;
	//[SerializeField] Transform effectTransform;

	public void OnPlayAudio(string name)
	{
		Debug.Log(name);
		audioSource.Play();
	}
	public void OnPlayEffect(GameObject go)
	{
		Instantiate(go, transform.position, transform.rotation);
	}
}
