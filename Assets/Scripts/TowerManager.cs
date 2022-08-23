using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<Material> _platform;
    public List<Tower> _towers;
}
[System.Serializable]
public class Tower
{
    public Model Model;
    public List<Preferance> Level;
}
[System.Serializable]
public class Preferance
{
    public Transform Prefab;
    public float Health;
    public float Damage;
    public float RotationSpeed;
    public float FireSpeed;
    public float TowerRange;
    public int Price;
}
public enum Model
{
    MachineGun,
    Sniper,
    RailGun,
    RocketMan
}
