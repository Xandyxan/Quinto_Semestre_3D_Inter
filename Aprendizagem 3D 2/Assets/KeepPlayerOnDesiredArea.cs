using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPlayerOnDesiredArea : MonoBehaviour
{
   // if the player moves too far away from the point of interest, the game will make him turn towards the objective and walk a bit in that direction
   // needed infos: PlayerController(movement), PlayerView(what direction is he facing), objectivePos, distance between player pos and objective pos.
    

    [SerializeField] private Vector3 objectivePos; //could've used a vector3 for the objective pos, but a null to set the position works fine
    private Transform playerTransform;
    private Camera playerCamera;    // used to force the player to turn around, maybe we could make use of the PlayerView script to handle that.
    private Transform playerCameraTransform;
    private void Awake()
    {
        playerTransform = FindObjectOfType<PlayerView>().GetComponent<Transform>();
        playerCamera = FindObjectOfType<Camera>();
        playerCameraTransform = playerCamera.GetComponent<Transform>();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 objectiveDirection = playerTransform.position - objectivePos;  // direction = destination - source
        float distanceFromObjective = Vector3.Distance(objectivePos, playerTransform.position);

        print(distanceFromObjective);
    }
}
