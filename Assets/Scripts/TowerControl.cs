using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField] private Transform _tower;
    [SerializeField] private Transform _updateMenu;
    [SerializeField] private Animation _anim;

    public UI_Manager Manager;
    public Platform Platform;
    public float Health;
    public float Damage;
    public float RotationSpeed;
    public float FireSpeed;
    public float TowerRange;

    private Transform _enemy;
    private delegate void EnemyDmg(float a);
    private float _timer;

    private ZombieBehavior _check;
    private const int _LAYER_MASK = 1 << 8;
    private void Update()
    {
        if (IsTarget())
        {
            Vector2 vectorPos = new Vector2(_enemy.position.x - transform.position.x, _enemy.position.z - transform.position.z);
            Quaternion main = Quaternion.Euler(_tower.rotation.x, EnemyInRoundSearch(vectorPos), _tower.rotation.z);
            _tower.rotation = Quaternion.Lerp(Quaternion.identity, main, RotationSpeed);
            _check = _enemy.GetComponent<ZombieBehavior>();
            EnemyDmg dmg = _check.Damage;
            Timer(dmg, FireSpeed / 100);
            _anim.Play();
        }
        else
        {
            _anim.Stop();
        }
    }
    void Timer(EnemyDmg enemyDmg, float time)
    {
        _timer += Time.deltaTime;
        if (_timer > time)
        {
            enemyDmg(Damage);
            _timer = 0;
        }
    }
    float EnemyInRoundSearch(Vector2 a)
    {
        float rotY = Vector2.Angle(new Vector2(0, 1), a);
        if(a.x > 0)
        {
            return rotY;
        }
        return -rotY;
    }
    private bool IsTarget()
    {
        Collider[] enem = Physics.OverlapSphere(transform.position, TowerRange/2, _LAYER_MASK);
        if (enem.Length > 0)
        {
            _enemy = enem[0].transform;
            return true;
        }
        return false;
    }
    private void OnMouseDown()
    {
        Platform.MenuOpen(_updateMenu);
        Platform.MenuTowerRangeOpen(TowerRange);
    }
}
