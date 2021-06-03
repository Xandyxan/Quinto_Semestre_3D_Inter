using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    #region Singleton Stuff
    private static AudioSettings _instance;
    public static AudioSettings instance { get { return _instance; } }
    #endregion

    FMOD.Studio.Bus MasterBus, EnvironmentBus, DubbingBus, SfxBus, MusicBus;

    private float masterVolume, environmentVolume, dubbingVolume, sfxVolume, musicVolume;

    private void Awake()
    {
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/MASTER");
        EnvironmentBus = FMODUnity.RuntimeManager.GetBus("bus:/MASTER/AMBIENTE");
        DubbingBus = FMODUnity.RuntimeManager.GetBus("bus:/MASTER/DUBLAGENS");
        SfxBus = FMODUnity.RuntimeManager.GetBus("bus:/MASTER/SFX");
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/MUSICAS");

        CreatePlayerPrefs();

        MasterBus.setVolume(PlayerPrefs.GetFloat("masterVolume", 1));
        EnvironmentBus.setVolume(PlayerPrefs.GetFloat("environmentVolume", 1));
        DubbingBus.setVolume(PlayerPrefs.GetFloat("dubbingVolume", 1));
        SfxBus.setVolume(PlayerPrefs.GetFloat("sfxVolume", 1));
        MusicBus.setVolume(PlayerPrefs.GetFloat("musicVolume", 1));

        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    private void Start()
    {
        #region Set Delegates
        GameManager.instance.pauseGameTrue -= PauseGameTrue;
        GameManager.instance.pauseGameFalse -= PausedGameFalse;
        GameManager.instance.pauseGameTrue += PauseGameTrue;
        GameManager.instance.pauseGameFalse += PausedGameFalse;
        #endregion
    }

    #region Setters
    public void SetMasterVolume(float newVolume)
    {
        masterVolume = newVolume;
        MasterBus.setVolume(masterVolume);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
    }

    public void SetEnvironmentVolume(float newVolume)
    {
        environmentVolume = newVolume;
        EnvironmentBus.setVolume(environmentVolume);
        PlayerPrefs.SetFloat("environmentVolume", environmentVolume);
    }

    public void SetDubbingVolume(float newVolume)
    {
        dubbingVolume = newVolume;
        DubbingBus.setVolume(dubbingVolume);
        PlayerPrefs.SetFloat("dubbingVolume", dubbingVolume);
    }

    public void SetVfxVolume(float newVolume)
    {
        sfxVolume = newVolume;
        SfxBus.setVolume(sfxVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    public void SetMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        MusicBus.setVolume(musicVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }
    #endregion

    private void CreatePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("masterVolume")) PlayerPrefs.SetFloat("masterVolume", 1);
        if (!PlayerPrefs.HasKey("environmentVolume")) PlayerPrefs.SetFloat("environmentVolume", 1);
        if (!PlayerPrefs.HasKey("dubbingVolume")) PlayerPrefs.SetFloat("dubbingVolume", 1);
        if (!PlayerPrefs.HasKey("sfxVolume")) PlayerPrefs.SetFloat("sfxVolume", 1);
        if (!PlayerPrefs.HasKey("musicVolume")) PlayerPrefs.SetFloat("musicVolume", 1);

        //Using directly the .GetFloat and setting a default value in case of doesn't exist the key, don't actually
        //pass the value to the variable, it was returning 0 always
        //Example: masterVolume(float) = PlayerPrefs.GetFloat("masterVolume", 1); = masterVolume = 0;
    }

    private void PauseGameTrue()
    {
        MasterBus.setPaused(true);
        EnvironmentBus.setPaused(true);
        DubbingBus.setPaused(true);
        SfxBus.setPaused(true);

        float initialMusicVolumeLerp = PlayerPrefs.GetFloat("musicVolume", 1);
        float endMusicVolumeLerp = PlayerPrefs.GetFloat("musicVolume", 1) / 2;
        float musicVolumeLerp = Mathf.Lerp(endMusicVolumeLerp, initialMusicVolumeLerp, 10f * Time.deltaTime);

        MusicBus.setVolume(musicVolumeLerp);
    }

    private void PausedGameFalse()
    {
        MasterBus.setPaused(false);
        EnvironmentBus.setPaused(false);
        DubbingBus.setPaused(false);
        SfxBus.setPaused(false);

        float initialMusicVolumeLerp = PlayerPrefs.GetFloat("musicVolume", 1);
        float endMusicVolumeLerp = PlayerPrefs.GetFloat("musicVolume", 1) / 2;
        float musicVolumeLerp = Mathf.Lerp(initialMusicVolumeLerp, endMusicVolumeLerp, 10f * Time.deltaTime);

        MusicBus.setVolume(musicVolumeLerp);
    }

}
