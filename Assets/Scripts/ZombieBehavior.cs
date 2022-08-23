using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehavior : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private Transform _point;
    [SerializeField] private Collider _collider;

    public UI_Manager Manager;
    public Transform HealthBar;
    public float Speed;
    private float _health;
    public int Reward;
    public float MaxHP;
    public int BaseDamage;
    [SerializeField] private float _deathSpeed = 0.2f;
    private const int _LAYER_MASK = 0;
    private void Start()
    {
        _health = MaxHP;
        HealthBar = Instantiate(HealthBar, Vector3.zero, Quaternion.identity, transform);
        HealthBar.localPosition = new Vector3(-0.1f, 2.3f, 0.35f);
        HealthBar = HealthBar.GetChild(0);
        HealthBar.parent.Rotate(0,0,48);
        HealthBar.parent.localScale = new Vector3(HealthBar.parent.localScale.x/10, HealthBar.parent.localScale.y/10, HealthBar.parent.localScale.z/10);
        _point = GameObject.FindWithTag("Point").transform;
        _meshAgent.Warp(transform.position);
        _meshAgent.speed = Speed;
        StartCoroutine(GiveTarget());
    }
    private void Update()
    {
        if (_health <= 0) Death();
        Quaternion par = HealthBar.parent.localRotation;
        HealthBar.parent.localRotation = Quaternion.Euler(par.x, -transform.eulerAngles.y, par.z);
    }
    void Death()
    {
        if (_meshAgent.enabled)
        {
            transform.gameObject.layer = _LAYER_MASK;
            _meshAgent.enabled = false;
            HealthBar.parent.gameObject.SetActive(false);
            Manager.CoinCall = Reward;
        }
        float pos = transform.position.x;
        transform.Translate(0, -_deathSpeed, 0);
        if (transform.localPosition.y < -12) Destroy(this.gameObject);
    }
    IEnumerator GiveTarget()
    {
        yield return new WaitForSeconds(0.1f);
        _meshAgent.enabled = true;
        _meshAgent.SetDestination(_point.position);
    }
    public void Damage(float damage)
    {
        _health -= damage;
        Vector3 heal = new Vector3(HealthBar.localScale.x, HealthBar.localScale.y, _health / MaxHP);
        HealthBar.localScale = heal;
        heal = new Vector3(HealthBar.localPosition.x, HealthBar.localPosition.y, -5 + (heal.z * 5));
        HealthBar.localPosition = heal;
    }
    private void OnDestroy()
    {
        Manager.PlayerWin();
    }
}
