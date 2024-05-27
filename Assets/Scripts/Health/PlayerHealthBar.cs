using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : HealthBar
{
    // Start is called before the first frame update
    void Start()
    {
        if (playerInputs.Instance != null)
        {
            characterHealth = playerInputs.Instance.GetComponent<Health>();
            currentHealthBar = GameObject.Find("PlayerHealthBar").gameObject.transform.GetChild(0).GetComponent<Image>();
        }
    }

    protected override void Update()
    {
        if ((characterHealth == null && playerInputs.Instance != null) || (playerInputs.Instance != null && characterHealth != playerInputs.Instance.GetComponent<Health>()))
        {
            characterHealth = playerInputs.Instance.GetComponent<Health>();
            currentHealthBar = GameObject.Find("PlayerHealthBar").gameObject.transform.GetChild(0).GetComponent<Image>();
        }
        base.Update();
    }
}
