using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestLogController : MonoBehaviour
{
    public TextMeshProUGUI questDisplay;
    public string[] quests;

    private bool[] _completionBitmap;

    // Start is called before the first frame update
    void Start()
    {
        _completionBitmap = new bool[quests.Length];
        Array.Clear(_completionBitmap, 0, _completionBitmap.Length);

        DisplayQuests();
    }

    void DisplayQuests()
    {
        string ongoing = "";
        string completed = "";
        
        for (var i = 0; i < quests.Length; i++)
        {
            if (_completionBitmap[i])
            {
                completed += "<color=#807046>- <s>" + quests[i] + "</s></color>\n";
            }
            else
            {
                ongoing += "- " + quests[i] + "\n";
                break;
            }
        }

        questDisplay.text = ongoing + completed;
    }

    public void MarkComplete(int questIndex, bool updateDisplay, bool previousComplete=false)
    {
        _completionBitmap[questIndex] = true;

        if (previousComplete)
        {
            for (int i = 0; i < questIndex; i++)
            {
                _completionBitmap[i] = true;
            }
        }

        if (updateDisplay)
        {
            DisplayQuests();
        }
    }


    void AddQuest(string quest)
    {
        _completionBitmap.Append(false);
        quests.Append(quest);
        
        DisplayQuests();
    }
}
