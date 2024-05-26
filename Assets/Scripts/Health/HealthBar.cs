using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    protected Health characterHealth;
    protected Image currentHealthBar;
   
    protected virtual void Update()
    {
        currentHealthBar.fillAmount = characterHealth._currentHealth / characterHealth._startingHealth;
    }
}
