using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingFloor : MonoBehaviour
{
    public GameObject floorTiles;
    public float floorSpeed;
    private Vector3 currentFloorPos;
    public float floorLimit;

    void FixedUpdate()
    {
        currentFloorPos = floorTiles.transform.position;
        currentFloorPos.x -= floorSpeed;
        if (currentFloorPos.x < floorLimit)
        {
            currentFloorPos.x = -currentFloorPos.x;
        }
        floorTiles.transform.position = currentFloorPos;
    }
}
