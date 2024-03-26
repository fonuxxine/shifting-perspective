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

    private ControlImageSwapper.ControllerType _controllerType;

    // Start is called before the first frame update
    void Start()
    {
        _controllerType = ControlImageSwapper.GetControllerType();
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.KeypadEnter))
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
                        var substr = lines[index].Substring(textComponent.text.Length, endIndex - textComponent.text.Length + 1);
                        if (_controllerType == ControlImageSwapper.ControllerType.Keyboard)
                        {
                            textComponent.text += substr;
                        }
                        else // convert to Controller icons if required
                        {
                            var substr_modified = substr.Replace("KBM", "Con");
                            textComponent.text += substr_modified;
                            // update lines[index] with new substring
                            lines[index] = lines[index].Remove(textComponent.text.Length - substr.Length, substr.Length).Insert(textComponent.text.Length - substr.Length, substr_modified);
                        }
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
        if (_controllerType != ControlImageSwapper.ControllerType.Keyboard)
        {
            lines[index] = lines[index].Replace("KBM\">", "Con\">");
        }
        
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