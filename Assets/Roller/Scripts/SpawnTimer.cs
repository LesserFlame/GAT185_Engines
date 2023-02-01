using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    //[SerializeField] EventRouter spawnerEvent;
    [SerializeField] private float spawnerTimerMax = 10;
    public float spawnerTimer = 10;
    [SerializeField] GameObject prefab;
    void Update()
    {
        var timeSpeed = FindObjectOfType<TimeManager>().GetComponent<TimeManager>().timeSpeed;
        spawnerTimer -= Time.deltaTime * timeSpeed;
        if (spawnerTimer <= 0)
        {
            spawnerTimer = spawnerTimerMax;
            Instantiate(prefab, transform);
        }
    }
}
