using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPlayerOnDesiredArea : MonoBehaviour
{
    // if the player moves too far away from the point of interest, the game will make him turn towards the objective and walk a bit in that direction
    // needed infos: PlayerController(movement), PlayerView(what direction is he facing), objectivePos, distance between player pos and objective pos.
    private float distanceFromObjective;

    [SerializeField] private Vector3 objectivePos; //could've used a vector3 for the objective pos, but a null to set the position works fine
    private Transform playerViewTransform;
    [SerializeField] private float rotationMultiplier = 3f;

    [Header("Dialogues")]
    [SerializeField] DialogueManager2 objectiveManager;

    [SerializeField] List<int> dialogueIndexes;
    private PlayerController playerMovement;

    private float delay;
    private float dragSpeed = 10f;

    private bool isInside = false;

    private void Awake()
    {
        playerViewTransform = FindObjectOfType<PlayerView>().GetComponent<Transform>();
        playerMovement = FindObjectOfType<PlayerController>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromObjective = Vector3.Distance(objectivePos, playerViewTransform.position);
        if (!isInside)
        {
            Vector3 objectiveDirection = objectivePos - playerViewTransform.position;  // direction = destination - source (tem que ser do pov do jogador)

          /// print("distance is: " + distanceFromObjective);
           // Debug.DrawRay(playerViewTransform.position, objectiveDirection, Color.green);
            Quaternion targetRotation = Quaternion.LookRotation(objectiveDirection);

            if (distanceFromObjective > 11.5f)
            {
                if (delay < Time.time)
                {
                    delay = Time.time + 5f;

                    int i = 0;
                    if (dialogueIndexes.Count > 1) { i = Random.Range(0, dialogueIndexes.Count); }
                    objectiveManager.ExecuteDialogue(dialogueIndexes[i]);
                }

                playerViewTransform.rotation = Quaternion.Slerp(playerViewTransform.rotation, targetRotation, Time.deltaTime * rotationMultiplier);
            }
            if (distanceFromObjective > 15f)
            {
                //paimon puxando jogador no abismo fds
                print("PARADO AI");
                playerMovement.transform.position = Vector3.MoveTowards(playerMovement.transform.position, objectivePos, dragSpeed * Time.deltaTime);
                //playerMovement.transform.position = objectivePos;
            }
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (distanceFromObjective > 9f)
            {
                isInside = true;
            }
            else
            {
                isInside = false;
            }
        }
    }
}
