using LootLocker.Requests;
using UnityEngine;

public class AssencionFountain : MonoBehaviour
{
    // Reference to the flag GameObject
    public GameObject flag;

    // Reference to the prompt or indicator GameObject
    public GameObject interactionPrompt;

    // Distance at which the player can interact with the flag
    public float interactionDistance = 2.0f;

    public Dialogue notEnoughSoul;
    public Dialogue dialogueCat;
    public Dialogue dialogueVending;
    public Dialogue dialogueSword;
    [SerializeField] private AudioClip GamemusicClip;
    private AudioSource audioSource;


    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GamemusicClip;
        //EventManager.OnTimerStart();
        //audioSource.Play();

    }


    void Update()
    {
        // Find the player GameObject by tag or layer
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        string playerName = player.name;
        

        if (player != null)
        {
            // Calculate distance between player and flag
            float distance = Vector2.Distance(player.transform.position, flag.transform.position);

            // Check if the player is within interaction distance
            if (distance <= interactionDistance)
            {
                // Show interaction prompt
                interactionPrompt.SetActive(true);

                // Check if the 'E' key is pressed
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //if (playerInputs.Instance.isEnoughSoul()==false)
                    if (playerInputs.Instance.returnPlayerSoul()<10)
                    {
                        Debug.Log(playerInputs.Instance.SoulLevel);
                        notEnoughSoulLevelDiag();
                    }
                    else if (playerName == "Cat")
                    {
                        InteractWithGuyAsCat();
                    }
                    else if(playerName == "VendingMachine")
                    {
                        InteractWithGuyAsVendingMachine();
                    }
                    else if (playerName == "Sword")
                    {
                        InteractWithGuyAsSwordMachine();
                    }

                    // Call a method to perform the action on the flag
                    //InteractWithFlag();
                }
            }
            else
            {
                // Hide interaction prompt if player is not in range
                interactionPrompt.SetActive(false);
            }
        }
    }

    void notEnoughSoulLevelDiag()
    {
        playerInputs.Instance.swordAtEnd();
        FindObjectOfType<DialogueManager>().StartDialogue(notEnoughSoul);
    }

    void InteractWithGuyAsCat()
    {
        audioSource.Play();
        playerInputs.Instance.catAtEnd();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueCat);
    }

    void InteractWithGuyAsVendingMachine()
    {
        audioSource.Play();
        playerInputs.Instance.VedingAtEnd();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueVending);
    }

    void InteractWithGuyAsSwordMachine()
    {
        GameManager.Instance.PauseMusic();
        
        audioSource.Play();
        playerInputs.Instance.swordAtEnd();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueSword);
        
    }
}
