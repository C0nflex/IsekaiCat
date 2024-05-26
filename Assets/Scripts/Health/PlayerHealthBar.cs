using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : HealthBar
{
    // Start is called before the first frame update
    void Start()
    {
        characterHealth = playerInputs.Instance.GetComponent<Health>();
        currentHealthBar = GameObject.Find("PlayerHealthBar").gameObject.transform.GetChild(0).GetComponent<Image>();
    }
}
