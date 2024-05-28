using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrequelDialog : MonoBehaviour
{
    public static PrequelDialog Instance { get; private set; }
    public Dialogue dialogue;
    [SerializeField] private GameObject nextPic;
    [SerializeField] private GameObject StartGameBtn;
    [SerializeField] private GameObject PrequelContainer;

    //public GameObject dialoguePrefab;

    private void Start()
    {
        Instance = this;
        //DontDestroyOnLoad(PrequelContainer);
    }
    public void TriggerDialogue()
    {
        DialogueManager.IsPrequelDialog = true;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        gameObject.SetActive(false);
    }
    public void EndDialog()
    {
        gameObject.SetActive(true);
        PrequelContainer.SetActive(false);
        nextPic.SetActive(true);
        StartGameBtn.SetActive(true);
        DialogueManager.IsPrequelDialog = false;
    }
}
