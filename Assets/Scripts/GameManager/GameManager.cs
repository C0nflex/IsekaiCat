using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Tilemap groundTilemap;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        EventManager.OnTimerStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
