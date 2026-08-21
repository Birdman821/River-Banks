using UnityEngine;

public class Key : MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Door door;
    private bool doorInitialized = false;
    public bool pickedUp { get; private set; }

    private void Awake()
    {
        pickedUp = false;
    }

    public void InitializeDoor(Door newDoor)
    {
        if (!doorInitialized)
        {
            door = newDoor;
            doorInitialized = true;
        }
        else
        {
            Debug.Log("Door already initialized!");
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pickedUp && collision.CompareTag("Player"))
        {
            pickedUp = true;
            if (door)
            {
                spriteRenderer.enabled = false;
                door.RemoveKey(this);
            }
            else
            {
                Debug.Log("No door!");
            }
        }
    }

}
