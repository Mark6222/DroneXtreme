using UnityEngine;
using UnityEngine.EventSystems;
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject FirstSelectedButton;

    void Start()
    {
        EventSystem.current.firstSelectedGameObject = mainMenu;
        EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
