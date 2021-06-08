using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour, IInteractable, ISelectable, IObjectiveObj
{
    [Header("Use only one movement type & axis")]
    public bool understandable; // variavel que não usa pra nada, understandable

    [Header("Rotation Movement?")]
    [Tooltip("Use only one value")]
    public Vector3 desireRotationValue;
    public float degreesPerSecond = 45f;

    [Header("Position Movement?")]
    [Tooltip("Use only one value")]
    public Vector3 desirePositionValue;
    public float positionVelocity = 2f;
    public bool positionOnlyZ;

    protected Vector3 idleRotation, idlePosition;

    protected bool isOpened, openingIsHappening;

    protected bool rotateRigth, rotateLeft, rotateUp, rotateDown;
    private bool positionZ;

    [Header("Extra situations")]
    public Dialogue dialogue;
    public GameObject areaTrigger;

    [Header("Selected")]

    [SerializeField] string _objectDescription;

    [SerializeField] bool _triggerDialogue;
    [SerializeField] int _dialogueIndex;

    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }
    public bool triggerDialogue { get => _triggerDialogue; set => _triggerDialogue = value; }
    public int dialogueIndex { get => _dialogueIndex; set => _dialogueIndex = value; }

    protected virtual void Awake()
    {
        openingIsHappening = rotateRigth = rotateLeft =  rotateUp = rotateDown = positionZ = false;

        //set the idleRotation and openedRotation values
        idleRotation = this.transform.localEulerAngles;
        idlePosition = this.transform.localPosition;

        //check the direction of rotation Y rotation
        if (desireRotationValue.y > 1f) rotateLeft = true;
        else if(desireRotationValue.y < -1f) rotateRigth = true;

        //check the direction of rotation X rotation
        if (desireRotationValue.x > 1f) rotateDown = true;
        else if (desireRotationValue.x < -1f) rotateUp = true;

        if (desirePositionValue.z > Mathf.Abs(0.001f)) positionZ = true;

        if (positionOnlyZ) desirePositionValue = new Vector3(idlePosition.x, idlePosition.y, desirePositionValue.z);
    }

    protected void Update()
    {
        if (openingIsHappening) RotateDoor();
        
    }

    public virtual void Interact()
    {
        openingIsHappening = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_GENERAL/SFX_Porta_Med_Armario_AbreFecha", transform.position);
        if (dialogue != null)
        {
            dialogue.RunCoroutine();
            areaTrigger.SetActive(true);
        }
    }

  //  public virtual void OpenCloseDoors()
   // { 

   // } //É chamado no 'Interactive' script // chama que a porta vai abrir

    protected void RotateDoor()
    {
        //It takes rotation value of eulerAngles and not localEulerAngles because the received values are differents
        //To rotate we are using a Quarternion Method where it is necessary atributte to the transform.rotation instead of localEulerAngles
        //Summing up: eulerAngles to transform.rotation changes - localEulerAngles to transform.localEulerAngles
        Vector3 currentAngle = transform.rotation.eulerAngles;
        Vector3 currentPosition = this.transform.localPosition;

        // Y rotate
        if (rotateLeft)
        {
            if (!isOpened) transform.rotation = Quaternion.AngleAxis(currentAngle.y + (Time.deltaTime * degreesPerSecond), Vector3.up);
            else if(isOpened) transform.rotation = Quaternion.AngleAxis(currentAngle.y - (Time.deltaTime * degreesPerSecond), Vector3.up);
        }
        else if(rotateRigth)
        {
            if (!isOpened) transform.rotation = Quaternion.AngleAxis(currentAngle.y - (Time.deltaTime * degreesPerSecond), Vector3.up);
            else if(isOpened) transform.rotation = Quaternion.AngleAxis(currentAngle.y + (Time.deltaTime * degreesPerSecond), Vector3.up);
        }
        

        // X rotate > Different way to rotate due to Quaternion.AgleAxis sligth changes the other rotation axis while rotating X axis for some reason
        if(rotateUp)
        {
            if (!isOpened) transform.rotation = Quaternion.Euler(currentAngle.x - (Time.deltaTime * degreesPerSecond), currentAngle.y, currentAngle.z);
            else if (isOpened) transform.rotation = Quaternion.Euler(currentAngle.x + (Time.deltaTime * degreesPerSecond), currentAngle.y, currentAngle.z);
        }
        else if(rotateDown)
        {
            if (!isOpened) transform.rotation = Quaternion.Euler(currentAngle.x + (Time.deltaTime * degreesPerSecond), currentAngle.y, currentAngle.z);
            else if (isOpened) transform.rotation = Quaternion.Euler(currentAngle.x - (Time.deltaTime * degreesPerSecond), currentAngle.y, currentAngle.z);
        }

        // Z position move
        if (positionZ)
        {
            if (!isOpened) this.transform.localPosition = Vector3.Lerp(currentPosition, desirePositionValue, Time.deltaTime * positionVelocity);
            else if (isOpened) this.transform.localPosition = Vector3.Lerp(currentPosition, idlePosition, Time.deltaTime * positionVelocity);
        }

        CheckDoorState();
    }

    protected void CheckDoorState()
    {
        // Y rotate check
        if (rotateLeft)
        {
            if (InspectorRotation(this.transform.localEulerAngles.y) >= InspectorRotation(desireRotationValue.y)) SetIsOpened(true);
            else if (InspectorRotation(this.transform.localEulerAngles.y) <= InspectorRotation(idleRotation.y)) SetIsOpened(false);
        }
        else if(rotateRigth)
        {
            if (InspectorRotation(this.transform.localEulerAngles.y) <= InspectorRotation(desireRotationValue.y)) SetIsOpened(true);
            else if (InspectorRotation(this.transform.localEulerAngles.y) >= InspectorRotation(idleRotation.y)) SetIsOpened(false);
        }

        // X rotate check
        if (rotateUp)
        {
            if (InspectorRotation(this.transform.localEulerAngles.x) <= InspectorRotation(desireRotationValue.x)) SetIsOpened(true);
            else if (InspectorRotation(this.transform.localEulerAngles.x) >= InspectorRotation(idleRotation.x)) SetIsOpened(false);
        }
        else if(rotateDown)
        {
            if (InspectorRotation(this.transform.localEulerAngles.x) >= InspectorRotation(desireRotationValue.x)) SetIsOpened(true);
            else if (InspectorRotation(this.transform.localEulerAngles.x) <= InspectorRotation(idleRotation.x)) SetIsOpened(false);
        }

        // Z position check
        if (positionZ)
        {
            if (this.transform.localPosition.z <= idlePosition.z + 0.0005f)
            {
                this.transform.localPosition = idlePosition;
                SetIsOpened(false);
            }
            else if (this.transform.localPosition.z >= desirePositionValue.z - 0.0005f)
            {
                this.transform.localPosition = desirePositionValue;
                SetIsOpened(true);
            }
        }
    }

    protected void SetIsOpened(bool isOpened)     //stop rotating when reach the desire or idle value,
    {
        this.isOpened = isOpened;
        openingIsHappening = false;
        ResetDefaultRotation();
    }

    protected void ResetDefaultRotation()     //this.transform.localEulerAngles altera o valor diretamente da rotação do objeto igual a que mostra no inspector
    {
        if (!isOpened) this.transform.localEulerAngles = idleRotation;
        else this.transform.localEulerAngles = desireRotationValue;
    }

    float InspectorRotation(float angle)  //apply to this.transform.localEulerAngles value
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }

    // Setters

    public void SetRandomRotationSpeed(float min, float max)
    {
        degreesPerSecond = Random.Range(min, max);
    }

    public void SetRotationSpeed(float desiredDPS)
    {
        degreesPerSecond = desiredDPS;
    }

    //NOTAS PARA FUTURA ATUALIZAÇÃO
    //InspectorRotation method está sendo apenas diferença quando assunto é comparar os valores de rotação como segue no método CheckDoorState
    //Nos outros pontos onde é atribuir o valor esse método não está fazendo diferença, com ou sem o valor é o mesmo, necessita investigação
}
