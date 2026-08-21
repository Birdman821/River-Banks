using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultPanel : Panel
{

    [SerializeField] private TextMeshProUGUI cashDisplay;

    override public void Enter(PlayerUIManager UIManager)
    {
        UIManager.playerInput.SwitchCurrentActionMap("Default");
        gameObject.SetActive(true);
        UpdateCashDisplay(cashDisplay, UIManager.playerCashComponent);
    }

    override public void UpdatePanel(PlayerUIManager UIManager)
    {
        
    }

    override public void FixedUpdatePanel(PlayerUIManager UIManager)
    {

    }

    public override void Exit(PlayerUIManager UIManager)
    {
        gameObject.SetActive(false);
    }

}

