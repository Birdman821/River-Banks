using UnityEditor;
using UnityEngine;

public class PausePanel : Panel
{
    [SerializeField] private SceneAsset mainMenu;
    public override void Enter(PlayerUIManager UIManager)
    {
        gameObject.SetActive(true);
        UIManager.playerInput.SwitchCurrentActionMap("Pause");
    }

    public override void Exit(PlayerUIManager UIManager)
    {
        gameObject.SetActive(false);
    }

    public void onMainMenuButtonPressed()
    {
        LevelManager.instance.LoadLevel(mainMenu);
    }



}
