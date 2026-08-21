using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class FloorManager : MonoBehaviour
{
    static public FloorManager instance;

    private Floor collidableFloor;
    [SerializeField] public List<Floor> floors;

    private void OrderFloors()
    {
        floors.Sort((floorA, floorB) => floorA.order.CompareTo(floorB.order));
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("multiple FloorManager singletons");
            Destroy(this);
        }

        floors = GetComponentsInChildren<Floor>().ToList(); //serialize?
        OrderFloors();
        collidableFloor = floors[0];
    }

    void Start()
    {
        
    }

    public void SwitchCollidableFloor(Floor newFloor)
    {
        collidableFloor.EnableFloor(false);
        collidableFloor = newFloor;
        collidableFloor.EnableFloor(true);
    }

    public void SetCollidableFloor(Floor newFloor)
    {
        floors.ForEach((floor) => floor.EnableFloor(false));
        collidableFloor = newFloor;
        collidableFloor.EnableFloor(true);
    }

    void Update()
    {

    }
}
