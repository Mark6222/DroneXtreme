using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject DroneCam;
    public GameObject BoardCam;
    void Start(){
        Board();
    }
    public void EcitDroneSwitch(){
        if(BoardCam.activeSelf){
            EditDrone();
        }else{
            Board();
        }
    }
    public void EditDrone(){
        DroneCam.SetActive(true);
        BoardCam.SetActive(false);
    }
    public void Board(){
        DroneCam.SetActive(false);
        BoardCam.SetActive(true);
    }

    public void ExitGame(){
        Application.Quit();
    }
}
