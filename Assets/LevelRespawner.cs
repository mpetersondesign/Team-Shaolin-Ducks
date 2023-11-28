using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRespawner : MonoBehaviour
{
    public GameObject EntityFolder;
    public Dictionary<Vector3, GameObject> RespawnableObject = new Dictionary<Vector3, GameObject>();

    public void Start()
    {
        List<GameObject> entities = new List<GameObject>();
        for (int i = 0; i < EntityFolder.transform.childCount; i++)
        {
            entities.Add(EntityFolder.transform.GetChild(i).gameObject);
        }

        foreach(GameObject entity in entities)
        {
            RespawnableObject.Add(entity.transform.position, entity);
        }
    }

    public void RespawnEntities()
    {
        // Destroy all current entities
        for (int i = EntityFolder.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(EntityFolder.transform.GetChild(i).gameObject);
        }

        // Instantiate new entities
        foreach (KeyValuePair<Vector3, GameObject> entry in RespawnableObject)
        {
            Instantiate(entry.Value, entry.Key, Quaternion.identity, EntityFolder.transform);
        }
    }
}
