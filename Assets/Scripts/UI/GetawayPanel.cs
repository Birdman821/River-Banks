using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GetawayPanel : Panel
{
    [SerializeField] private TextMeshProUGUI PossibleCashDisplay;
    [SerializeField] private TextMeshProUGUI totalCashDisplay;
    [SerializeField] private Selectable continueButton;

    override public void Enter(PlayerUIManager UIManager)
    {
        UIManager.playerInput.SwitchCurrentActionMap("Getaway Panel");
        gameObject.SetActive(true);
        UpdateCashDisplay(PossibleCashDisplay, UIManager.playerCashComponent);
        UpdateCashDisplay(totalCashDisplay, UIManager.playerCashComponent);
    }

    public void OnNextSlide(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!totalCashDisplay.gameObject.activeSelf)
            {
                totalCashDisplay.gameObject.SetActive(true);
                continueButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnContinueButtonPressed()
    {
        LevelManager.instance.LoadNextLevel();
    }

    public override void Exit(PlayerUIManager UIManager)
    {
        gameObject.SetActive(false);
    }
}
