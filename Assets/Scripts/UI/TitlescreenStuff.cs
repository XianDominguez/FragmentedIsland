using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlescreenStuff : MonoBehaviour
{
    public Animator animator;
    public string sceneToLoad;

    private bool hasTransitioned = false;

    void Update()
    {
        if (!hasTransitioned && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !animator.IsInTransition(0))
        {
            hasTransitioned = true;
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
