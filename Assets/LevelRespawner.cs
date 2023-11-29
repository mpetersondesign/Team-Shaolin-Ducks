using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRespawner : MonoBehaviour
{
    public GameObject entitiesPrefab;
    private GameObject currentEntities;

    void Start()
    {
        // Instantiate the initial Entities on level start
        currentEntities = Instantiate(entitiesPrefab, transform);
    }

    public void RespawnEntities()
    {
        // Destroy the old Entities
        if (currentEntities != null)
        {
            Destroy(currentEntities);
        }

        // Instantiate a new Entities and update the reference
        currentEntities = Instantiate(entitiesPrefab, transform);
    }
}