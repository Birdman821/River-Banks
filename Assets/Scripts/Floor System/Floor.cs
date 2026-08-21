using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Floor : MonoBehaviour
{
    [SerializeField] private GameObject floorTilemap;
    [field: SerializeField] public SpriteRenderer floorWaterVisual { get; private set; }
    [field: SerializeField] public int order { get; private set; }
    [field: SerializeField] public string sortingLayerName { get; private set; }
    [SerializeField] private bool debugOn = false;

    private List<FloorComponent> floorObjects = new List<FloorComponent>();

    void Start()
    {
        DebugManager.instance.Log($"{gameObject.name} floorobjects: {floorObjects.ToCommaSeparatedString()}", debugOn);
    }

    public int GetFloorIndex()
    {
        return FloorManager.instance.floors.FindIndex((floor) => floor == this);
    }

    public void AddFloorObject(FloorComponent floorObject)
    {
        if(floorObject != null)
        {
            floorObjects.Add(floorObject);
            //floorobjects are added on awake debug manager singleton is intitialised on awake
            //DebugManager.instance.Log($"added {floorObject.gameObject} to {gameObject}", debugOn); 
        }
        else
        {
            Debug.Log("attempted to set null as a floor object");
        }
    }

    public void RemoveFloorObject(FloorComponent floorObject)
    {
        if (floorObject != null)
        {
            floorObjects.Remove(floorObject);
            DebugManager.instance.Log($"removed {floorObject.gameObject} from {gameObject}", debugOn);
        }
        else
        {
            Debug.Log("attempted to remove null as a floor object");
        }
    }

    public void EnableFloor(bool on)
    {
        DebugManager.instance.Log($"{gameObject} enabled: {on}", debugOn);
        if(floorObjects.Count != 0)
        {
            foreach(FloorComponent floorObject in floorObjects)
            {
                if(floorObject != null)
                {
                    floorObject.Enable(on);
                }
            }
        }
        else
        {
            DebugManager.instance.Log($"{gameObject} has no floor objects", debugOn);
        }
    }

    void Update()
    {
        
    }
}
