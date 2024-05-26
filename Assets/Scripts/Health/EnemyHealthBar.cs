using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : HealthBar
{
    // Start is called before the first frame update
    void Start()
    {
        characterHealth = transform.parent.transform.parent.transform.parent.GetComponent <Health>();
        currentHealthBar = GetComponent<Image>();
    }


}
