using UnityEngine;
using UnityEngine.EventSystems;
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(mainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
