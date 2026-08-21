using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField] public BoxCollider2D lowerCollider;
    [SerializeField] public BoxCollider2D upperCollider;
    [SerializeField] public FloorComponent lowerFloorComponent;
    [SerializeField] public FloorComponent upperFloorComponent;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform waterMaskTransform;

    private void Update()
    {
        if (WaterLevelManager.instance.waterFloorComponent.currentFloor.order == lowerFloorComponent.currentFloor.order)
        {
            float waterLevel = WaterLevelManager.instance.GetWaterLevel();
            waterMaskTransform.localScale = new Vector2(waterMaskTransform.localScale.x, 1 - waterLevel);
        }
        else if(WaterLevelManager.instance.waterFloorComponent.currentFloor.order > lowerFloorComponent.currentFloor.order)
        {
            waterMaskTransform.localScale = new Vector2(waterMaskTransform.localScale.x, 0);
        }
        

    }

}
