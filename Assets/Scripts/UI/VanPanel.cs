using TMPro;
using UnityEngine;

public class VanPanel : Panel
{

    [SerializeField] private TextMeshProUGUI playerCashDisplay;
    [SerializeField] private TextMeshProUGUI vanCashDisplay;
    [SerializeField] private CashComponent vanCashComponent;
    private CashComponent playerCashComponent;
    
    private void Awake()
    {
        playerCashComponent = UIManager.playerCashComponent;
    }

    public void OnEscapeButton()
    {
        GameManager.instance.PauseGame(true); //replace with end game
        UIManager.SwitchPanel(UIManager.GetawayPanel);
    }

    public void OnDepositButton()
    {
        playerCashComponent.TransferTo(vanCashComponent, playerCashComponent.GetCashHeld());
        UpdateCashDisplay(playerCashDisplay, playerCashComponent);
        UpdateCashDisplay(vanCashDisplay, vanCashComponent);
    }

    override public void Enter(PlayerUIManager UIManager)
    {
        UIManager.playerInput.SwitchCurrentActionMap("Default Panel");
        gameObject.SetActive(true);
        UpdateCashDisplay(playerCashDisplay, playerCashComponent);
        UpdateCashDisplay(vanCashDisplay, vanCashComponent);
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

