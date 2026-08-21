using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool paused = true;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("multiple game managers");
            Destroy(this);
        }
    }

    void Start()
    {
        WaterLevelManager.instance.SetLevelTransition(1.5f, 0.01f);
    }

    public void PauseGame(bool on)
    {
        paused = on;
        WaterLevelManager.instance.Pause(on);
    }

    void Update()
    {

    }
}
