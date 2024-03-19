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
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
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
                char nextChar = lines[index][textComponent.text.Length];

                if (nextChar == '<') // do not split up a style tag i.e. <sprite index=1>
                {
                    int endIndex = lines[index].IndexOf('>', textComponent.text.Length+1);
                    if (endIndex != -1)
                    {
                        textComponent.text += lines[index].Substring(textComponent.text.Length, endIndex - textComponent.text.Length + 1);
                    } else
                    {
                        textComponent.text += nextChar;
                    }
                }
                else
                {
                    textComponent.text += nextChar;
                }
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