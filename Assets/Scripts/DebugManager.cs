using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("multiple debug manager singletons");
        }
    }

    public void Log(string message, bool enabled)
    {
        if (enabled)
        {
            Debug.Log(message);
        }
    }
    
}
