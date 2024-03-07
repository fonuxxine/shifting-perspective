using UnityEngine;

public class OldDialogueTrigger : MonoBehaviour
{
    public float interactionDistance = 0.5f; // distance within which interaction can occur
    
    private Transform _player; // player character's transform
    private Transform _npc; // NPC's transform
    private GameObject _dialoguePrompt; // the dialogue prompt UI element
    private GameObject _enterKeyImage; // the ENTER KEY image GameObject
    private bool _isInRange = false; // whether player is in range of this NPC
    private bool _hasDialogue = false; // whether NPC has dialogue prepared
    private bool _hasEnterPrompt = false; // whether NPC has dialogue prepared

    
    private void Start()
    {
        // get the player character's transform
        _player = GameObject.FindWithTag("Player").transform;
        
        // get the NPC's transform
        _npc = transform;
        
        // check if dialogue prompt exists & hide it if so
        Transform dialoguePromptTransform = _npc.Find("DialoguePrompt");
        _hasDialogue = dialoguePromptTransform != null;

        if (_hasDialogue)
        {
            _dialoguePrompt = dialoguePromptTransform.gameObject;
            _dialoguePrompt.SetActive(false);
        }
        
        // retrieve the ENTER KEY image if exists & hide it if so
        Transform enterKeyImageTransform = _npc.Find("EnterKeyCanvas");
        _hasEnterPrompt = enterKeyImageTransform != null;

        if (_hasEnterPrompt)
        {
            _enterKeyImage = enterKeyImageTransform.gameObject;
            _enterKeyImage.SetActive(false);
        }
    }

    private void Update()
    {
        // check if the player is in range of the NPC
        _isInRange = Vector3.Distance(_player.position, _npc.position) <= interactionDistance;

        // activate dialogue prompt when player presses ENTER and is in range
        if (_isInRange && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            ActivateDialogue();
            _hasEnterPrompt = false;
        }
        else if (_hasEnterPrompt)
        {
            // show ENTER KEY image when player is in range and not pressing ENTER
            SetEnterKeyImageActive(_isInRange);

            // make the Enter Key image face the camera
            if (_hasEnterPrompt && _isInRange)
            {
                _enterKeyImage.transform.LookAt(_enterKeyImage.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            }
        }
    }
    
    void ActivateDialogue()
    {
        // enable the dialogue prompt if it exists
        if (_hasDialogue)
        {
            _dialoguePrompt.SetActive(true);
            SetEnterKeyImageActive(false);
        }
    }

    void SetEnterKeyImageActive(bool isActive)
    {
        // show or hide ENTER KEY image based on input
        if (_hasEnterPrompt)
        {
            _enterKeyImage.SetActive(isActive);
        }
    }
}
