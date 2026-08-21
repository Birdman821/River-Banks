using UnityEngine;

public class ScopePanel : Panel
{

    private void Awake()
    {
        
    }

    override public void Enter(PlayerUIManager UIManager)
    {
        UIManager.playerInput.SwitchCurrentActionMap("Scope");
        gameObject.SetActive(true);
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
