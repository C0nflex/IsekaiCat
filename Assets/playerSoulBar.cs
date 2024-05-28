using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSoulBar : MonoBehaviour
{
    [SerializeField] public Image SoulBarFill;

    private void Start()
    {
        
    }
    // Start is called before the first frame update
    void Update()
    {
        //SoulBarFill.fillAmount = 1;
        SoulBarFill.fillAmount = (float)playerInputs.Instance.SoulLevel / 10f;
    }



}
