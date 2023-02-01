using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Coin : Interactable
{
    void Start()
    {
        GetComponent<CollisionEvent>().onEnter += OnInteract;
    }

    public override void OnInteract(GameObject go)
    {
        var player = FindObjectOfType<RollerPlayer>();
        {
            player.AddPoints(100);
        }
        if (interactFX != null) Instantiate(interactFX, transform.position, Quaternion.identity);
        if (destroyOnInteract) Destroy(gameObject);
        if (deactivateOnInteract) gameObject.SetActive(false);
    }
}
