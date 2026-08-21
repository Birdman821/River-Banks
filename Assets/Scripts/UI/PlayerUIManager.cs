using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerUIManager : MonoBehaviour
{
    [field: SerializeField] public Canvas canvas { get; private set; }

    [Header("Panels")] 
    [field: SerializeField] public DefaultPanel defaultPanel { get; private set; }
    [field: SerializeField] public CashGrabPanel cashGrabPanel { get; private set; }
    [field: SerializeField] public PausePanel PausePanel { get; private set; }
    [field: SerializeField] public ScopePanel ScopePanel { get; private set; }
    [field: SerializeField] public GetawayPanel GetawayPanel { get; private set; }
    [field: SerializeField] public VanPanel VanPanel { get; private set; }

    [Header("Player Components")]
    [field: SerializeField] public PlayerInputManager playerInputManager { get; private set; }
    [field: SerializeField] public CashComponent playerCashComponent { get; private set; }
    [field: SerializeField] public PlayerInput playerInput { get; private set; }
    
    public Panel currentPanel { get; private set; }

    void Start()
    {
        currentPanel = ScopePanel;
        currentPanel.Enter(this);
    }

    public void SwitchPanel(Panel nextPanel)
    {
        currentPanel.Exit(this);
        currentPanel = nextPanel;
        nextPanel.Enter(this);
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        
        if (context.started)
        {
            cashGrabPanel.StartGrab();

        }
        else if (context.canceled)
        {
            cashGrabPanel.EndGrab();
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed && playerInputManager.currentCameraTransition == null)
        {
            if (currentPanel != defaultPanel)
            {
                playerInputManager.UndoTransition();
                SwitchPanel(defaultPanel);
            }
        }

    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchPanel(PausePanel);
            GameManager.instance.PauseGame(true);
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchPanel(defaultPanel);
            GameManager.instance.PauseGame(false);
        }
    }

    private void Update()
    {
        if(currentPanel != null)
        {
            currentPanel.UpdatePanel(this);
        }
        
    }
    private void FixedUpdate()
    {
        if(currentPanel != null)
        {
            currentPanel.FixedUpdatePanel(this);
        }
        
    }

}
