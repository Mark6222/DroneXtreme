using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public Animator[] animators;
    int screenShowing = 0;
    void Start()
    {
        animators[3].SetTrigger("Hide");
        animators[2].SetTrigger("Hide");
        animators[1].SetTrigger("Hide");
        animators[screenShowing].SetTrigger("show");
    }

    public void HomeScreenButton()
    {
        animators[0].SetTrigger("Show");
        animators[screenShowing].SetTrigger("Hide");
        screenShowing = 0;
    }

    public void ModesScreenButton()
    {
        animators[1].SetTrigger("Show");
        animators[screenShowing].SetTrigger("Hide");
        screenShowing = 1;
    }

    public void MultiplayerScreenButton()
    {
        animators[2].SetTrigger("Show");
        animators[screenShowing].SetTrigger("Hide");
        screenShowing = 2;
    }

    public void SettingsScreenButton()
    {
        animators[3].SetTrigger("Show");
        animators[screenShowing].SetTrigger("Hide");
        screenShowing = 3;
    }

    // HomeScreen 
    public void LoadPracticeLevel()
    {
        SceneManager.LoadScene("FreeRoam");
    }
    // ModesScreen
    public void LoadRaceLevel()
    {
        SceneManager.LoadScene("Race");
    }
    // MultiplayerScreen
    public void LoadMultiplayerLevel()
    {
        SceneManager.LoadScene("NetworkExample");
    }
    // SettingsScreen
}
