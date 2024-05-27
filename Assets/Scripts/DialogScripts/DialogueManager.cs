using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    private Queue<string> sentences;
    public Animator animator;
    public playerInputs pushP;

    private BasicEnemyBehaviour[] allEnemies;
    [SerializeField]
    private GameObject mainMenuContainer;

    //public List<AudioClip> sentenceAudioClips;
    [SerializeField] private List<AudioClip> voiceLines;
    private AudioSource audioSource;
    [SerializeField] private Animator robotAnimationController;
    [SerializeField] private Animator EvilrobotAnimationController;
    [SerializeField] private GameObject endingScreen;
    [SerializeField] private GameObject vendingMachineForSwitch;
    [SerializeField] private GameObject catForSwitch;
    [SerializeField] private GameObject swordForSwitch;

    //private Vector2 startVector = new Vector2(-7.15f, -1.65f); // this is the coords for spawn
    private Vector2 startVector = new Vector2(18.86f, 33.97f);
    
    // Start is called before the first frame update
    void Start()
    {
        pushP = playerInputs.Instance;
        sentences = new Queue<string>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = voiceLines[0];
    }

    public void StartDialogue(Dialogue dialogue)
    {
        EvilrobotAnimationController.speed = 1;
        robotAnimationController.speed = 1;
        playerInputs.Instance.DisableMovement();
        mainMenuContainer.SetActive(false);
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        //audioSource.Play();
        int sentenceIndex = sentences.Count;
        if (sentenceIndex < voiceLines.Count && voiceLines[sentenceIndex] != null)
        {
            audioSource.clip = voiceLines[sentenceIndex];
            audioSource.Play();
        }
        dialogueText.text = "";
        foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return null;
        }

        while (audioSource.isPlaying)
        {
            yield return null;
        }
    }

    void EndDialogue()
    {
        allEnemies = FindObjectsOfType<BasicEnemyBehaviour>();
        playerInputs.Instance.DisableMovement();
        robotAnimationController.speed = 0;
        EvilrobotAnimationController.speed = 0;
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        animator.SetBool("IsOpen", false);
        EventManager.OnTimerStart();
        
        foreach (BasicEnemyBehaviour enemy in allEnemies)
        {
            enemy.EnableMovement();
        }
        if(playerInputs.Instance.isCatAtEnd==true && playerInputs.Instance.returnPlayerName()=="Cat") {
            
            catForSwitch.SetActive(false);
            vendingMachineForSwitch.SetActive(true);
            playerInputs.Instance.transform.position = startVector;
            //playerInputs.Instance.EnableMovement();
            //add life force 0
        }
        else if (playerInputs.Instance.isVendingAtEnd == true && playerInputs.Instance.returnPlayerName() == "VendingMachine")
        {
            vendingMachineForSwitch.SetActive(false);
            swordForSwitch.SetActive(true);
            playerInputs.Instance.transform.position = startVector;
            //add life force 0
        }
        else if(playerInputs.Instance.isSwordAtEnd == true && playerInputs.Instance.returnPlayerName() == "Sword")
        {
            EventManager.OnTimerStop();
            endingScreen.SetActive(true);
            // add button for restart scene :)
        }
        playerInputs.Instance.EnableMovement();
    }

}
