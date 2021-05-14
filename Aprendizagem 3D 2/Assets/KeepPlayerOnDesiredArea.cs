using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPlayerOnDesiredArea : MonoBehaviour
{
    // if the player moves too far away from the point of interest, the game will make him turn towards the objective and walk a bit in that direction
    // needed infos: PlayerController(movement), PlayerView(what direction is he facing), objectivePos, distance between player pos and objective pos.
    

    [SerializeField] private Transform playerTransform, objectiveTransform; // could've used a vector3 for the objective pos, but a null to set the position works fine


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 objectiveDirection = playerTransform.position - objectiveTransform.position;  // direction = destination - source
        float distanceFromObjective = Vector3.Distance(objectiveTransform.position, playerTransform.position);

        print(distanceFromObjective);
    }
}
