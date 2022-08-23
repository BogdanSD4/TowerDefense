using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseControl : MonoBehaviour
{
    [SerializeField] private UI_Manager _manager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            Destroy(other.gameObject);
            _manager.HealthCall = other.GetComponent<ZombieBehavior>().BaseDamage;
        }
    }
}
