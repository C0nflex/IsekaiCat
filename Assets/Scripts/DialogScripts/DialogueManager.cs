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


    // Start is called before the first frame update
    void Start()
    {
        allEnemies = FindObjectsOfType<BasicEnemyBehaviour>();
        sentences = new Queue<string>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = voiceLines[0];
    }

    public void StartDialogue(Dialogue dialogue)
    {
        mainMenuContainer.SetActive(false);
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        if(sentences.Count == 0) {
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
        foreach(char c in sentence.ToCharArray())
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
        robotAnimationController.speed = 0;
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        animator.SetBool("IsOpen", false);
        EventManager.OnTimerStart();
        pushP.EnableMovement();
        foreach(BasicEnemyBehaviour enemy in allEnemies)
        {
            enemy.EnableMovement();
        }
    }

}
