using UnityEngine;

public class QuestTriggerComplete : MonoBehaviour
{
    public int questID;
    public GameObject questLog;
    
    // Start is called before the first frame update
    void Start()
    {
        QuestLogController questLogController = questLog.GetComponent<QuestLogController>();

        questLogController.MarkComplete(questID, true, true);
    }
}
