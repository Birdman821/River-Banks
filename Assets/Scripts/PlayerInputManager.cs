using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerInputManager : MonoBehaviour
{
    FloorManager floorManager;
    WaterLevelManager waterLevelManager;

    [Header("Movement Values")]
    [SerializeField] [Min(0f)] float baseMoveSpeed;

    [Header("Player Components")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer cigaretteSprite;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject playerCameraPivot;
    [SerializeField] private GameObject playerFlyPivot;
    [SerializeField] private Rigidbody2D playerCameraRigidBody;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private GameManager gameManager;
    [field: SerializeField] public PlayerUIManager uiManager { get; private set; }
    [field : SerializeField] public FloorComponent playerFloorComponent { get; private set; }
    [field: SerializeField] public CashComponent playerCashComponent { get; private set; }

    public BoxCollider2D currentStairCollider { get; private set; }
    public BoxCollider2D currentCashPileCollider { get; private set; }
    public Coroutine currentCameraTransition { get; private set; }
    public TilemapCollider2D currentMudCollider { get; private set; }
    public BoxCollider2D currentVanCollider { get; private set; }

    private Vector2 moveVector = Vector2.zero;

    private float moveSpeed;
    private float elevation = 0f;

    private void Awake()
    {
        moveSpeed = baseMoveSpeed;
    }

    void Start()
    {
        floorManager = FloorManager.instance;
        waterLevelManager = WaterLevelManager.instance;
        
        floorManager.SetCollidableFloor(playerFloorComponent.currentFloor);
    }


    private void ChangePlayerFloor(Floor floor)
    {
        foreach(SpriteRenderer sprite in playerSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.sortingLayerID = SortingLayer.NameToID(floor.sortingLayerName);
        }
        foreach(ParticleSystemRenderer particles in playerSprite.GetComponentsInChildren<ParticleSystemRenderer>())
        {
            particles.sortingLayerID = SortingLayer.NameToID(floor.sortingLayerName);
        }
        playerFloorComponent.ChangeFloor(floor);
        floorManager.SwitchCollidableFloor(floor);

    }

    public void ToggleScope(bool on)
    {
        
        if (on)
        {
            cigaretteSprite.gameObject.SetActive(true);
            gameManager.PauseGame(true);
            currentCameraTransition = StartCoroutine(CameraTransition(playerCamera, playerFlyPivot.transform, 8f, 0f));
            uiManager.SwitchPanel(uiManager.ScopePanel);
        }
        else
        {
            cigaretteSprite.gameObject.SetActive(false);
            gameManager.PauseGame(false);
            uiManager.SwitchPanel(uiManager.defaultPanel);
            playerCameraRigidBody.simulated = false;
            UndoTransition();
        }
        
        
    }

    // gives values roughly from 0 to 1
    // if on lower collider gives progress from the bottom
    // if on top breifly gives progress from top and transitions to bottom if low enough
    private void HandleStairs()
    {
        Stairs currentStairs = currentStairCollider.transform.GetComponentInParent<Stairs>(); // !!!
        if (currentStairCollider == currentStairs.lowerCollider)
        {
            // no idea how this works
            float progress = currentStairCollider.transform.InverseTransformPoint(transform.position).y - (currentStairCollider.size.y * -0.5f); 
            elevation = progress / currentStairCollider.size.y;
            if (elevation > 1)
            {
                ChangePlayerFloor(currentStairs.upperFloorComponent.currentFloor);
            }
        }
        else
        {
            // nor how this works but its flipped
            float progress = (currentStairCollider.size.y * 0.5f) - currentStairCollider.transform.InverseTransformPoint(transform.position).y;
            elevation = progress / currentStairCollider.size.y;
            if (elevation > 0f)
            {
                ChangePlayerFloor(currentStairs.lowerFloorComponent.currentFloor);
            }
            
        }
        //Debug.Log($"Elevation : {elevation}");
    }

    // ----------------------- Camera -----------------------------

    private IEnumerator CameraTransition(Camera camera, Transform pivot, float targetZoom, float duration)
    {
        //if you ever implement a callback, note the transistion lasts longer than the duration for some reason
        camera.transform.SetParent(pivot);
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, timeElapsed / duration);
            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, new Vector3(0f, 0f, camera.transform.localPosition.z), timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        camera.orthographicSize = targetZoom;
        camera.transform.localPosition = new Vector3(0f, 0f, camera.transform.localPosition.z);
        currentCameraTransition = null;
    }

    public void UndoTransition()
    {
        if(currentCameraTransition == null)
        {
            currentCameraTransition = StartCoroutine(CameraTransition(playerCamera, playerCameraPivot.transform, 8f, 0.5f));
        }
        
    }

    // ----------------------- MONOBEHAVIOUR -----------------------------

    //Replace tags checks with component checks
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            currentStairCollider = collision.ConvertTo<BoxCollider2D>();
        }
        if (collision.CompareTag("Cash Pile"))
        {
            currentCashPileCollider = collision.ConvertTo<BoxCollider2D>();
        }
        if (collision.CompareTag("Mud"))
        {
            currentMudCollider = collision.ConvertTo<TilemapCollider2D>();
        }
        if (collision.CompareTag("Van"))
        {
            currentVanCollider = collision.ConvertTo<BoxCollider2D>();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            currentStairCollider = null;
            // clean up stair related values here maybe
        }
        if (collision.CompareTag("Cash Pile"))
        {
            currentCashPileCollider = null;
        }
        if (collision.CompareTag("Mud"))
        {
            currentMudCollider = null;
        }
        if (collision.CompareTag("Van"))
        {
            currentVanCollider = null;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        if(moveVector != Vector2.zero)
        {
            playerAnimator.SetBool("Walking", true);
            if(moveVector.x > 0)
            {
                playerAnimator.SetBool("FacingRight", true);
            }
            else if(moveVector.x < 0)
            {
                playerAnimator.SetBool("FacingRight", false);
            }
        }
        else
        {
            playerAnimator.SetBool("Walking", false);
        }
    }

    public void OnFly(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnAscend(InputAction.CallbackContext context)
    {
        //unused
    }

    public void OnDescend(InputAction.CallbackContext context)
    {
        //unused
    }

    public void OnInteract(InputAction.CallbackContext context) // rework into own component
    {
        if (uiManager.currentPanel == uiManager.defaultPanel && currentCameraTransition == null)
        {
            if (currentCashPileCollider)
            {
                Transform cameraPivot = currentCashPileCollider.transform.Find("CameraPivot");
                currentCameraTransition = StartCoroutine(CameraTransition(playerCamera, cameraPivot, 2f, 0.5f));
                uiManager.SwitchPanel(uiManager.cashGrabPanel);
            }
            else if (currentVanCollider)
            {
                //playerCashComponent.TransferTo(currentVanCollider.GetComponent<CashComponent>(), playerCashComponent.GetCashHeld());
                //uiManager.defaultPanel.UpdateCashDisplay();
                Transform cameraPivot = currentVanCollider.transform.Find("CameraPivot");
                currentCameraTransition = StartCoroutine(CameraTransition(playerCamera, cameraPivot, 2f, 0.5f));
                uiManager.SwitchPanel(uiManager.VanPanel);

            }
            
        }
    }

    private void FixedUpdate()
    {
        if (playerFloorComponent.currentFloor.order <= waterLevelManager.waterFloorComponent.currentFloor.order)
        {
            moveSpeed = baseMoveSpeed * (1 - Mathf.Clamp(waterLevelManager.GetWaterLevel(playerFloorComponent.currentFloor) - elevation, 0, 1));
        }
        else
        {
            moveSpeed = baseMoveSpeed;
        }
        if (currentMudCollider != null)
        {
            moveSpeed *= 0.4f;
        }

        if (uiManager.currentPanel == uiManager.ScopePanel)
        {
            playerCameraRigidBody.linearVelocity = moveVector * moveSpeed * Time.fixedDeltaTime;
        }
        else if (uiManager.currentPanel == uiManager.defaultPanel)
        {
            playerRigidbody.linearVelocity = moveVector * moveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            playerRigidbody.linearVelocity = Vector2.zero;
        }
        
    }

    void Update()
    {
        if (currentStairCollider != null)
        {
            HandleStairs();
        }

        

    }
}
