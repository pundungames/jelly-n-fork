using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    public void Load()
    {
        int level = PlayerPrefs.GetInt("_level", 0);

        //loop
        if (SceneManager.GetActiveScene().buildIndex != level)
        {
            if (level > 6)
            {
                level = Random.Range(0, 7);
            }
        }
       
        SceneManager.LoadScene(level);
    }
    private void OnEnable()
    {
        EventManager.levelComplete += LevelComplete;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        EventManager.levelComplete -= LevelComplete;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void LevelComplete()
    {
        PlayerPrefs.SetInt("_level", PlayerPrefs.GetInt("_level", 0) + 1);
        Invoke(nameof(Load), 0.6f);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventManager.OnLevelStart();
    }
}