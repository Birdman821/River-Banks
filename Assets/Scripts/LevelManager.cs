using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private GameObject loadingScreen;
    [field: SerializeField] public List<SceneAsset> levelList { get; private set; }
    private List<string> levelNameList = new List<string>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("destroyed new levelmanager");
            Destroy(gameObject);
        }

        foreach(SceneAsset scene in levelList)
        {
            levelNameList.Add(scene.name);
        }

    }

    private IEnumerator LoadLevelCoroutine(SceneAsset level)
    {
        loadingScreen.SetActive(true);

        string levelName = level.name;
        AsyncOperation scene = SceneManager.LoadSceneAsync(levelName);
        scene.allowSceneActivation = false;

        while(scene.progress < 0.8)
        {
            yield return null;
        }
        
        scene.allowSceneActivation = true;
        loadingScreen.SetActive(false);

    } 
    public void LoadLevel(SceneAsset level)
    {
        StartCoroutine(LoadLevelCoroutine(level));
    }

    public void LoadNextLevel()
    {
        
        LoadLevel(levelList[levelNameList.FindIndex((name) => name == SceneManager.GetActiveScene().name) + 1]);
    }

}
