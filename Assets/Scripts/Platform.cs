using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform _star;
    [SerializeField] private Transform _camera;

    [SerializeField] private TowerManager _towerManager;

    [Header("PlatformSkin")]
    [SerializeField] private MeshRenderer _platformMat;
    [SerializeField] private Transform _towerRangeCircle;
    public Transform TowerMenu;
    public Model _model;

    private Vector3 _menuPosition = new Vector3(0.12f, 0, 0.18f);
    private UI_Manager _manager;
    private Transform _empty;
    private Transform _menu;
    private Transform tower;
    private float _camScale;
    private float _cam;

    private float _health;
    private float _damage;
    private float _rotationSpeed;
    private float _fireSpeed;
    private float _towerRange;
    void Start()
    {
        _empty = Instantiate(_star, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, transform);
        _camera = GameObject.FindWithTag("MainCamera").transform;
        _empty.GetComponent<ChooseTower>().Platform = GetComponent<Platform>();
        _empty.rotation = Quaternion.Euler(_empty.rotation.x, _empty.rotation.y, +_camera.eulerAngles.x);
        _cam = _camera.GetComponent<Camera>().orthographicSize;
        _towerManager = _camera.GetComponent<TowerManager>();
        _manager = _camera.GetComponent<UI_Manager>();

        Transform rg = Instantiate(_towerRangeCircle, transform.position, Quaternion.identity, transform);
        _towerRangeCircle = rg;
        rg.gameObject.SetActive(false);

    }
    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            _camScale = Input.GetAxis("Mouse ScrollWheel");
            _cam = _camera.GetComponent<Camera>().orthographicSize;
            _empty.rotation = Quaternion.Euler(_empty.rotation.x, _empty.rotation.y, +_camera.eulerAngles.x);
            if (_menu != null)
            {
                _menu.rotation = Quaternion.Euler(_menu.eulerAngles.x, _menu.eulerAngles.y, +_camera.eulerAngles.x);
                if (_cam > 15 && _cam < 20) _menu.localScale -= new Vector3(_camScale, _camScale, _camScale);
            }
        }
    }
    public void MenuTowerRangeOpen(float scale)
    {
        _towerRangeCircle.gameObject.SetActive(true);
        _towerRangeCircle.localScale = new Vector3(scale/100, _towerRangeCircle.localScale.y, scale/100);
    } 
    public void MenuTowerRangeClose() => _towerRangeCircle.gameObject.SetActive(false);
    public void MenuOpen(Transform menu)
    {
        _empty.gameObject.SetActive(false);
        float camSize = (_cam - 15)*0.2f;
        _menu = Instantiate(menu, transform.position, Quaternion.identity, transform);
        _menu.rotation = Quaternion.Euler(_menu.eulerAngles.x, _menu.eulerAngles.y, +_camera.eulerAngles.x);
        _menu.localScale += new Vector3(camSize, camSize, camSize);
        _menu.position += new Vector3(0, 4f, 0);
        if (menu.GetComponent<WeaponMenuControl>())
        {
            _menu.GetComponent<WeaponMenuControl>().Platform = GetComponent<Platform>();
            _menu.GetComponent<WeaponMenuControl>().Manager = _camera.GetComponent<UI_Manager>();
        }
        else if (menu.GetComponent<UpdateMenuControl>())
        {
            _menu.GetComponent<UpdateMenuControl>().Platform = GetComponent<Platform>();
            _menu.GetComponent<UpdateMenuControl>().Manager = _camera.GetComponent<UI_Manager>();
        }
        camSize = camSize / 100 * 3f;
        _menu.localPosition = new Vector3(_menuPosition.x - camSize, _menuPosition.y, _menuPosition.z + camSize);
    }
    public void MenuClose(GameObject menu)
    {
        Destroy(menu);
        if (tower == null)
        {
            _empty.gameObject.SetActive(true);
            GetTowerLevel = 0;
        }
    }
    public bool TowerChoose(Model model)
    {
        
        for (int i = 0; i < _towerManager._towers.Count; i++)
        {
            if (_towerManager._towers[i].Model == model)
            {
                if (_towerManager._towers[i].Level.Count < i + 1) return false;
                Preferance preferances = _towerManager._towers[i].Level[GetTowerLevel];
                if (_towerManager._towers[i].Level[GetTowerLevel] != null)
                {
                    TowerParameters(model, GetTowerLevel);
                    tower = Instantiate(preferances.Prefab, transform.position, Quaternion.identity, transform);

                    tower.GetComponent<TowerControl>().Health = _health;
                    tower.GetComponent<TowerControl>().Damage = _damage;
                    tower.GetComponent<TowerControl>().FireSpeed = _fireSpeed;
                    tower.GetComponent<TowerControl>().RotationSpeed = _rotationSpeed;
                    tower.GetComponent<TowerControl>().TowerRange = _towerRange;

                    tower.GetComponent<TowerControl>().Platform = GetComponent<Platform>();
                    tower.GetComponent<TowerControl>().Manager = _camera.GetComponent<UI_Manager>();
                    _platformMat.material = _towerManager._platform[GetTowerLevel];
                    GetTowerLevel++;
                    return true;
                }
            }
        }
        return false;
    }
    private void TowerParameters(Model model, int level)
    {
        for (int i = 0; i < _towerManager._towers.Count; i++)
        {
            if (_towerManager._towers[i].Model == model)
            {
                _health = _towerManager._towers[i].Level[level].Health;
                _damage = _towerManager._towers[i].Level[level].Damage;
                _fireSpeed = _towerManager._towers[i].Level[level].FireSpeed;
                _rotationSpeed = _towerManager._towers[i].Level[level].RotationSpeed;
                _towerRange = _towerManager._towers[i].Level[level].TowerRange;
            }
        }
    }
    public int CurrentTowerPrice{ get; set; }
    public int GetTowerLevel { get; set; }
    public Transform GetTower 
    {
        get { return tower; } 
        set { tower = value; } 
    }
    public void ChangeTowerAfterUpdate(Model model)
    {
        TowerParameters(model, GetTowerLevel);
        MenuClose(tower.gameObject);
        TowerChoose(model);
    }
    public int TowerPrice(Model model, int level)
    {
        for (int i = 0; i < _towerManager._towers.Count; i++) {
            if (_towerManager._towers[i].Model == model)
            {
                if (_towerManager._towers[i].Level.Count > i)
                {
                    return _towerManager._towers[i].Level[level].Price;
                }
                else return 0;
            }
        }
        return 0;
    }
}

