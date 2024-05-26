using UnityEngine;

public class FlagCheckPoint : MonoBehaviour
{
    // Reference to the flag GameObject
    public GameObject flag;

    // Reference to the prompt or indicator GameObject
    public GameObject interactionPrompt;

    // Distance at which the player can interact with the flag
    public float interactionDistance = 2.0f;

    public GameObject player2;



    void Update()
    {
        // Find the player GameObject by tag or layer
        GameObject player = GameObject.FindGameObjectWithTag("Player");

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
                    // Call a method to perform the action on the flag
                    InteractWithFlag();
                    
                }
            }
            else
            {
                // Hide interaction prompt if player is not in range
                interactionPrompt.SetActive(false);
            }
        }
    }

    void InteractWithFlag()
    {
        //player2.GetComponent<playerInputs>().setPlayerCheckPoint
        Debug.Log(transform.position);
        playerInputs.Instance.setPlayerCheckPoint(gameObject.transform.position);
    }
}
