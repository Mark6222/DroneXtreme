using UnityEngine;
using UnityEngine.SceneManagement;
public class MultiplayerScreen : MonoBehaviour
{
    public Animator[] animators;
    int screenShowing = 0;

    void Start()
    {
        animators[4].SetTrigger("Hide");
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

    public void HostScreenButton()
    {
        animators[1].SetTrigger("Show");
        animators[screenShowing].SetTrigger("Hide");
        screenShowing = 1;
    }

    public void JoinScreenButton()
    {
        animators[2].SetTrigger("Show");
        animators[screenShowing].SetTrigger("Hide");
        screenShowing = 2;
    }
    public void SessionScreenButton()
    {
        animators[3].SetTrigger("Show");
        animators[screenShowing].SetTrigger("Hide");
        screenShowing = 3;
    }
    public void JoinedScreenButton()
    {
        animators[4].SetTrigger("Show");
        animators[screenShowing].SetTrigger("Hide");
        screenShowing = 4;
    }
}
