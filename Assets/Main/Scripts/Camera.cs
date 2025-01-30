using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject DroneCam;
    public GameObject BoardCam;
    void Start(){
        Board();
    }

    public void EditDrone(){
        DroneCam.SetActive(true);
        BoardCam.SetActive(false);
    }
    public void Board(){
        DroneCam.SetActive(false);
        BoardCam.SetActive(true);
    }
}
