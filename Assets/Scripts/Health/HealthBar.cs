using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Image _currentHealthBar;


    private void Start()
    {
        
    }

   
    private void Update()
    {
        _currentHealthBar.fillAmount = _playerHealth._currentHealth / _playerHealth._startingHealth;
    }
}
