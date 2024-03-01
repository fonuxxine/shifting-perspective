using UnityEngine;
using TMPro;

// adapted from https://www.youtube.com/watch?v=8oTYabhj248
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public GameObject activateAfter;

    private int index;
    private float timeElapsed;
    private bool typing;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopTyping();
                textComponent.text = lines[index];
            }
        } else
        {
            UpdateTyping();
        }
    }

    void StartDialogue()
    {
        index = 0;
        timeElapsed = 0f;
        typing = true;
    }

    void UpdateTyping()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= textSpeed)
        {
            timeElapsed = 0f;
            if (textComponent.text.Length < lines[index].Length)
            {
                textComponent.text += lines[index][textComponent.text.Length];
            }
            else
            {
                typing = false;
            }
        }
    }

    void StopTyping()
    {
        textComponent.text = lines[index];
        typing = false;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            timeElapsed = 0f;
            typing = true;
        }
        else
        {
            gameObject.SetActive(false);

            if (activateAfter != null)
            {
                activateAfter.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (typing)
        {
            UpdateTyping();
        }
    }
}