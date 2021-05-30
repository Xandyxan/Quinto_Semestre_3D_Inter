using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour, ISound
{
    [Header("Sound settings")]
    [Tooltip("The name of the path sound from FMOD project")]
    [SerializeField] private string _soundPath;

    [Header("3D settings")]
    [Tooltip("It is a 3D sound?")]
    [SerializeField] private bool _is3dSound;
    [SerializeField] private Transform _soundSource;

    [Header("Extra sounds?")]
    [SerializeField] private string extraSoundPath;

    private FMOD.Studio.EventInstance sound;

    public string soundPath { get => _soundPath; set => _soundPath = value; }
    public bool is3Dsound { get => _is3dSound; set => _is3dSound = value; }

    private void Start()
    {
        if (_soundSource == null) _soundSource = this.gameObject.transform;
        sound = FMODUnity.RuntimeManager.CreateInstance(_soundPath);
        if (_is3dSound) sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(_soundSource));
    }
    private void OnEnable()
    {
        if (_is3dSound) sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(_soundSource));
    }

    private void OnDisable()
    {
        StopSound();
    }

    private void Update()
    {
        if (_is3dSound) sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(_soundSource));
    }

    public void StartSound()
    {
        sound.start();
    }

    public void StopSound()
    {
        sound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        sound.release();
    }

    public void PlayOneShoot()
    {
        if(extraSoundPath != "") FMODUnity.RuntimeManager.PlayOneShot(extraSoundPath);
    }
}
