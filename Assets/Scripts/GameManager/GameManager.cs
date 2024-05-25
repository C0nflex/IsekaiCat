using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public cameraFollowObject _cameraFollowObject;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //EventManager.OnTimerStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
