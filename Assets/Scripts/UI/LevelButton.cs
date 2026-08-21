using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private SceneAsset level;

    public void OnButtonPressed()
    {
        LevelManager.instance.LoadLevel(level);
    }
}
