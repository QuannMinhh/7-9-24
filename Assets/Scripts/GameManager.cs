using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public SaveSystem saveSystem;

    private void Awake()
    {
        SceneManager.sceneLoaded += Initialize;
        DontDestroyOnLoad(gameObject);
    }
    private   
    void Update() // Update instead of Awake for continuous input checking
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                SceneManager.LoadScene(0); // Load main menu (scene index 0)
        }
    }
    
    private void Initialize(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("Loaded GM");
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
            player = playerInput.gameObject;
        saveSystem = FindObjectOfType<SaveSystem>();
        if (player != null && saveSystem.LoadedData != null)
        {
            var damagable = player.GetComponentInChildren<Damagable>();
            damagable.Health = saveSystem.LoadedData.playerHealth;
        }
    }

    public void LoadLeve()
{
    Debug.Log("LoadedData: " + (saveSystem.LoadedData == null ? "null" : "exists"));

    if (saveSystem.LoadedData != null)
    {
        SceneManager.LoadScene(saveSystem.LoadedData.sceneIndex);
        return;
    }

    SceneManager.LoadScene(1); // Bắt đầu lại từ màn đầu tiên nếu không có dữ liệu
}


    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SaveData()
    {
        if (player != null)
            saveSystem.SaveData(SceneManager.GetActiveScene().buildIndex + 1, player.GetComponentInChildren<Damagable>().Health);
    }
}