using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    // Ensure there's only one instance of CursorManager
    private static CursorManager instance;

    private void Awake()
    {
        // If an instance of CursorManager already exists, destroy this one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the instance and mark it to not be destroyed on scene load
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure the cursor is visible and unlocked initially
        SetCursorState(false);
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure the cursor is visible and unlocked when a new scene is loaded
        SetCursorState(false);
    }

    public void SetCursorState(bool isLocked)
    {
        // Set cursor visibility and lock state
        Cursor.visible = !isLocked;
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
