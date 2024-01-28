using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public float interactionDistance = 2f; // distance within which interaction can occur
    
    private Transform _player; // player character's transform
    private Transform _npc; // NPC's transform
    private GameObject _dialoguePrompt; // the dialogue prompt UI element
    private bool _isInRange = false; // whether player is in range of this NPC
    private bool _hasDialogue = false; // whether NPC has dialogue prepared
    
    private void Start()
    {
        // get the player character's transform
        _player = GameObject.FindWithTag("Player").transform;
        Debug.Log(_player);
        
        // get the NPC's transform
        _npc = transform;
        Debug.Log(_npc);
        
        // find the dialogue prompt for the NPC
        Transform dialoguePromptTransform = _npc.Find("DialoguePrompt");
        
        // check if dialogue prompt exists & hide it if so
        _hasDialogue = dialoguePromptTransform != null;

        if (_hasDialogue)
        {
            _dialoguePrompt = dialoguePromptTransform.gameObject;
            _dialoguePrompt.SetActive(false);
        }
    }

    private void Update()
    {
        // check if the player is in range of the NPC
        _isInRange = Vector3.Distance(_player.position, _npc.position) <= interactionDistance;
    
        // activate dialogue prompt when player presses ENTER and is in range
        if (_isInRange && Input.GetKeyDown(KeyCode.Return))
        {
            ActivateDialogue();
        }
    }
    
    void ActivateDialogue()
    {
        // enable the dialogue prompt if it exists
        if (_hasDialogue)
        {
            _dialoguePrompt.SetActive(true);
        }
    }
}