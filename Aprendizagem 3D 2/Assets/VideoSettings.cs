using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class VideoSettings : MonoBehaviour
{
    #region Singleton Stuff
    private static VideoSettings _instance;
    public static VideoSettings instance { get { return _instance; } }
    #endregion

    Resolution[] resolutions;
    public Dropdown dropDownResolutions;
    public Toggle fullScreenToggle;

    public delegate void ChangeMouseSensibility();
    public ChangeMouseSensibility changeMouseSensibility;

    [Header("It's main screen?")]
    [SerializeField] private bool mainMenuScreen;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("qualityIndex")) CreatePlayerPrefs();

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityIndex"));

        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        dropDownResolutions.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + " hZ";
            
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;

            dropDownResolutions.AddOptions(options);
            dropDownResolutions.value = currentResolutionIndex;
            dropDownResolutions.RefreshShownValue();
        }

        //Set the inital value with the actual PlayerPrefs
        if (PlayerPrefs.GetInt("isFullScreen") == 0)
        {
            Screen.fullScreen = false;
            fullScreenToggle.isOn = Screen.fullScreen;
        }
        else
        {
            Screen.fullScreen = true;
            fullScreenToggle.isOn = Screen.fullScreen;
        }

    }

    private void Update()
    {

    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("qualityIndex", qualityIndex);
    }
    
    public void SetResolution(int resolutionsIndex)
    {
        Resolution resolution = resolutions[resolutionsIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        if (Screen.fullScreen) PlayerPrefs.SetInt("isFullScreen", 1);
        else PlayerPrefs.SetInt("isFullScreen", 0);
    }

    public void SetMouseSensibility(float mouseSensibility)
    {
        PlayerPrefs.SetFloat("mouseSensibility", mouseSensibility);
        if(!mainMenuScreen) instance.changeMouseSensibility();
    }

    private void CreatePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("qualityIndex")) PlayerPrefs.SetInt("qualityIndex", 5);
        if (!PlayerPrefs.HasKey("isFullScreen")) PlayerPrefs.SetInt("isFullScreen", 1);
        if (!PlayerPrefs.HasKey("mouseSensibility")) PlayerPrefs.SetFloat("mouseSensibility", 5f);

        //if (!PlayerPrefs.HasKey("resolutionIndex")) PlayerPrefs.SetFloat("resolutionIndex", 0);

        //Using directly the .GetFloat and setting a default value in case of doesn't exist the key, don't actually
        //pass the value to the variable, it was returning 0 always
        //Example: masterVolume(float) = PlayerPrefs.GetFloat("masterVolume", 1); = masterVolume = 0;
    }


}
