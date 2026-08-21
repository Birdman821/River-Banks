using NUnit.Framework;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class FloorComponent : MonoBehaviour
{
    [SerializeField] private Floor initialFloor;
    [SerializeField] public bool isFloorObject = true;

    [SerializeField] private bool debugOn = false;
    [SerializeField] private List<Collider2D> floorObjectColliders = new List<Collider2D>();
    public Floor currentFloor { get; private set; }

    private void Awake()
    {
        currentFloor = initialFloor;
        if (isFloorObject)
        {
            currentFloor.AddFloorObject(this);
        }

    }

    public void ChangeFloor(Floor newFloor)
    {
        if (isFloorObject)
        {
            currentFloor.RemoveFloorObject(this);
            newFloor.AddFloorObject(this);
        }
        currentFloor = newFloor;

        //change sorting layer
    }

    public void Enable(bool on)
    {
        DebugManager.instance.Log($"{gameObject} enabled: {on}", debugOn);
        if(isFloorObject && floorObjectColliders.Count != 0)
        {
            foreach(Collider2D floorObjectCollider in floorObjectColliders)
            {
                floorObjectCollider.enabled = on;
            }

        }
    }
}
