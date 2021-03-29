using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Give a value to idle/walk/running head bobbing to make the character movement more realistic**/
public class HeadBobber : MonoBehaviour
{
    [Header("Breath Parameters")]
    [SerializeField] float idleAmplitude = 0.015f;
    [SerializeField] float idlePeriod = 0.65f;
    [SerializeField] float walkAmplitude = 0.01f;
    [SerializeField] float walkPeriod = 0.06f;
    [SerializeField] float runAmplitude = 0.02f;
    [SerializeField] float runPeriod = 0.045f;

    private float actualAmplitude, actualPeriod;
    private Vector3 startPos;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();

        actualAmplitude = walkAmplitude;
        actualPeriod = walkPeriod;
        startPos = this.transform.position;
    }

    private void Update()
    {
        UpdateBreathValues();
        Breath();
    }

    private void Breath()
    {
        //This value will always increase over time by timeSinceLevelLoad --- Period it's to increase/decrease the change value
        //It's the time it takes to complete a cicle (from -1 to 1 or 1 to -1 wit Mathf.Sin)
        float theta = Time.timeSinceLevelLoad / actualPeriod;

        //Mathf.Sin will always return a value between -1 and 1 --- Amplitude it's to increase/decrease the change value
        //It's total value to change the Y position of the camera
        float distance = actualAmplitude * Mathf.Sin(theta);

        //Aply the transformation overtime (remembering it's from -1 to 1 + increases/decreases changers)
        transform.position = new Vector3(this.transform.position.x, startPos.y + 1 * distance, this.transform.position.z);
    }

    private void UpdateBreathValues()   //Get the actual idle/walk/run state of playerController to update values
    {
        if (playerController.GetIsRunning()) 
        {
            actualAmplitude = runAmplitude;
            actualPeriod = runPeriod;
        }
        else if(playerController.GetIsWalking())
        {
            actualAmplitude = walkAmplitude;
            actualPeriod = walkPeriod;
        }
        else
        {
            actualAmplitude = idleAmplitude;
            actualPeriod = idlePeriod;
        }
    }
}
