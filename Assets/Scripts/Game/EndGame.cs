using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Animator animator;
    public int levelToLoad;

    public void FadeToLevel(int levelIndex)
    {
        animator.SetTrigger("FadeOut");
        levelToLoad = levelIndex;
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    // New method to reset the level completely
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}