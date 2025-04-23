using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{
    public Animator options, settings;
    public Text SensitivityText, speedText, dragText, cameraTiltText;
    public Slider SensitivitySlider, speedSlider, dragSlider, cameraTiltSlider;
    // void Awake()
    // {
    //     PlayerPrefs.SetFloat("Sensitivity", 5f);
    //     PlayerPrefs.SetFloat("Speed", 2.5f);
    //     PlayerPrefs.SetFloat("Drag", 2.5f);
    //     PlayerPrefs.SetFloat("CameraTilt", -32f);
    // }
    void Start()
    {
        settings.SetTrigger("Hide");
        SensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        speedSlider.value = PlayerPrefs.GetFloat("Speed");
        dragSlider.value = PlayerPrefs.GetFloat("Drag");
        cameraTiltSlider.value = -PlayerPrefs.GetFloat("CameraTilt");
    }

    void Update()
    {
        SensitivityText.text = SensitivitySlider.value.ToString("0.00");
        speedText.text = speedSlider.value.ToString("0.00");
        dragText.text = dragSlider.value.ToString("0.00");
        cameraTiltText.text = cameraTiltSlider.value.ToString("0.00");
        PlayerPrefs.SetFloat("Sensitivity", SensitivitySlider.value);
        PlayerPrefs.SetFloat("Speed", speedSlider.value);
        PlayerPrefs.SetFloat("Drag", dragSlider.value);
        PlayerPrefs.SetFloat("CameraTilt", -cameraTiltSlider.value);
    }

    public void ShowOptions()
    {
        settings.SetTrigger("Hide");
        options.SetTrigger("Show");
    }
    public void ShowSettings()
    {
        settings.SetTrigger("Show");
        options.SetTrigger("Hide");
    }
}
