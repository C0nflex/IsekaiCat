using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    protected Health characterHealth;
    protected Image currentHealthBar;
   
    protected virtual void Update()
    {

        if (currentHealthBar == null || characterHealth == null)
            return;
        currentHealthBar.fillAmount = characterHealth._currentHealth / characterHealth._startingHealth;
    }
}
