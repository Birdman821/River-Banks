using System.Collections;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;


public class WaterLevelManager : MonoBehaviour
{
    static public WaterLevelManager instance;
    [field: SerializeField] public FloorComponent waterFloorComponent { get; private set; }
    private FloorManager floorManagerSingleton;
    private SpriteRenderer currentWaterVisual;

    private float waterRate = 0f;
    private bool isIncreasing = false;
    private bool isPause = true;
    private float targetLevel;
    private float waterLevel = 0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {   
            Debug.Log("multiple waterlevelmanager singletons");
            Destroy(this);
        }
    }

    private void Start()
    {
        floorManagerSingleton = FloorManager.instance;
        waterFloorComponent.ChangeFloor(floorManagerSingleton.floors[0]); //bottom floor
        currentWaterVisual = waterFloorComponent.currentFloor.floorWaterVisual;
    }

    public float GetWaterLevel(Floor relativeFloor = null)
    {
        if(relativeFloor != null)
        {
            int floorDifference = math.max(waterFloorComponent.currentFloor.GetFloorIndex() - relativeFloor.GetFloorIndex(), 0);
            return waterLevel + floorDifference;
        }
        else
        {
            return waterLevel;
        }
    }

    public void SetLevelTransition(float newTargetLevel, float newWaterRate)
    {
        targetLevel = newTargetLevel;
        waterRate = newWaterRate;
        if (waterLevel <= newTargetLevel)
        {
            isIncreasing = true;
        }
        else
        {
            isIncreasing = false;
        }
    }

    public void Pause(bool on)
    {
        isPause = on;
    }


    public IEnumerator TimedPauseCoroutine(int duration)
    {
        isPause = true;
        yield return new WaitForSeconds(duration);
        isPause = false;
    }

    public void TimedPause(int duration)
    {
        StartCoroutine(TimedPauseCoroutine(duration));
    }

    public void Stop()
    {
        targetLevel = waterLevel;
        Pause(true);
    }

    void Update()
    {
        if (!isPause)
        {


            if (waterLevel > 1)
            {
                targetLevel -= 1;
                waterLevel -= 1;
                waterFloorComponent.ChangeFloor(floorManagerSingleton.floors[waterFloorComponent.currentFloor.GetFloorIndex() + 1]);
                currentWaterVisual = waterFloorComponent.currentFloor.floorWaterVisual;
            }
            else if (waterLevel < 0)
            {
                targetLevel += 1;
                waterLevel += 1;
                waterFloorComponent.ChangeFloor(floorManagerSingleton.floors[waterFloorComponent.currentFloor.GetFloorIndex() - 1]);
                currentWaterVisual = waterFloorComponent.currentFloor.floorWaterVisual;
            }


            if (waterLevel <= targetLevel + 0.005 && !isIncreasing | waterLevel >= targetLevel - 0.005 && isIncreasing)
            {
                waterLevel = targetLevel;
                isPause = true;
            }
            else if(isIncreasing)
            {
                //Debug.Log("increasing");
                waterLevel += Time.deltaTime * waterRate;
            }
            else
            {
                //Debug.Log("decreasing");
                waterLevel -= Time.deltaTime * waterRate;
            }

            currentWaterVisual.color = new Color(currentWaterVisual.color.r, currentWaterVisual.color.g, currentWaterVisual.color.b, waterLevel);
            //Debug.Log($"{waterLevel} waterlayer: {waterFloorComponent.currentFloor} targetLevel {targetLevel}");
        }

    }
}
