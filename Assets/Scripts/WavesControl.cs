using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WavesControl : MonoBehaviour
{
    [SerializeField] private Transform healthBar;
    [SerializeField] private UI_Manager manager;
    [SerializeField] private TextMeshProUGUI _waveUI;
    [SerializeField] private Transform[] _spawner;
    [SerializeField] private int _waveNumber = 0;
    private int _waveNumberUI = 0;
    private bool _waveComtinue;
    public static int _unitsInSceneForCheckWin;
    private static int _unitsInSceneForCheckWave;

    [Header("Enemy")]
    [SerializeField] private List<EnemySettings> _waves;

    [Header("WaveTimer")]
    [SerializeField] private Image _timer;

    [Header("UI_Indicator")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("WayArrow")]
    [SerializeField] private ArrowControl _arrow;

    private float _startTime;
    private void Start()
    {
        _waveNumberUI = _waveNumber;
        _waveUI.text = $"{_waveNumberUI + 1}/{_waves.Count} Wave";
    }
    void Update()
    {
        if (WaveTimer(_waveNumber))
        {
            if (_waveNumber != _waves.Count)
            {
                for(int i = 0; i < _waves[_waveNumber]._parametersEnemy.Count; i++)
                {
                    ParametersEnemy enemy = _waves[_waveNumber]._parametersEnemy[i];
                    enemy._waveTimer += Time.deltaTime;
                    if (enemy._waveTimer > enemy._timeToSpawn)
                    {
                        if (enemy._timeToSpawn >= 0)
                        {
                            enemy._timeToSpawn = -1;
                            Spawn(enemy._prefabEnemy, CheckSpawner(_waves[_waveNumber].spawner, i));
                            enemy._unitInScene++;
                            enemy._waveTimer = 0;
                            _unitsInSceneForCheckWin++;
                            _unitsInSceneForCheckWave--;
                        }
                        if (enemy._waveTimer > enemy._spawnStep && enemy._unitInScene != enemy._maxUnits)
                        {
                            enemy._waveTimer = 0;
                            Spawn(enemy._prefabEnemy, CheckSpawner(_waves[_waveNumber].spawner, i));
                            enemy._unitInScene++;
                            _unitsInSceneForCheckWin++;
                            _unitsInSceneForCheckWave--;
                        }
                        if (_unitsInSceneForCheckWave == 0)
                        {
                            WaveEnd();
                            break;
                        }
                    }
                }
            }
        }
    }
    private bool WaveTimer(int wave)
    {
        if (_timer.fillAmount == 0)
        {
            _timer.transform.parent.gameObject.SetActive(false);
            if (!_waveComtinue)
            {
                StartWave();
                _waveComtinue = true;
            }
            _arrow.CloseArrow();
            return true;
        }
        if (_startTime == 0)
        {
            _waveComtinue = false;
            _arrow.ChoseArrow(_waves[wave].spawner);
            _startTime = _waves[wave]._timeToWave;
            _timer.transform.parent.gameObject.SetActive(true);
        }
        _waves[wave]._timeToWave -= Time.deltaTime;
        _timer.fillAmount = _waves[wave]._timeToWave / _startTime;
        return false;
    }
    private void CountUnitsInScene()
    {
        _unitsInSceneForCheckWave = 0;
        List<ParametersEnemy> parameters = _waves[_waveNumber]._parametersEnemy;
        for (int i = 0; i < parameters.Count; i++)
        {
            _unitsInSceneForCheckWave += (int)parameters[i]._maxUnits;
        }
    }
    public void StartWave()
    {
        _waveNumberUI++;
        _waveComtinue = true;
        _waveUI.text = $"{_waveNumberUI}/{_waves.Count} Wave";
        _timer.fillAmount = 0;
        manager.PlayShotSFX(SFXType.StartWave);
        manager.PlayGameMusic(MusicType.WaveMusic);
        CountUnitsInScene();
    }
    private bool WaveEnd()
    {
        if (_waveNumber == _waves.Count - 1)
        {
            _waveNumber++;
            return true;
        }
        else
        {
            _waveComtinue = false;
            _startTime = 0;
            _timer.fillAmount = 1;
            _waveNumber++;
        }
        return false;
    }
    public bool CurrentWave()
    {
        if (_waveNumber == _waves.Count) return true;
        return false;
    }
    private void Spawn(Transform enemy, Transform spawner)
    {
        Transform go = Instantiate(enemy, spawner.position, Quaternion.identity, spawner);
        go.GetComponent<ZombieBehavior>().HealthBar = healthBar;
        go.GetComponent<ZombieBehavior>().Manager = manager;
    }
    private Transform CheckSpawner(Spawner spawner, int enemyNumber)
    {
        switch (spawner)
        {
            case Spawner.one: return _spawner[0];
            case Spawner.two: return _spawner[1];
            case Spawner.one | Spawner.two: return CheckSpawner(_waves[_waveNumber]._parametersEnemy[enemyNumber]._spawnerCurrent, 0);
            default:
                break;
        }
        return null;
    }
}
[System.Serializable]
public class EnemySettings
{
    public Spawner spawner;
    [Tooltip("Time to the next wave / Standart 20 seconds")]
    public float _timeToWave = 20;
    public List<ParametersEnemy> _parametersEnemy = new List<ParametersEnemy>();
}
[System.Serializable]
public class ParametersEnemy
{
    public Spawner _spawnerCurrent;
    public Transform _prefabEnemy;
    [Header("SpawnSettings")]
    public float _spawnStep;
    public float _timeToSpawn;
    public float _maxUnits;
    [HideInInspector]public float _waveTimer;
    [HideInInspector]public float _unitInScene;
}
public enum Enemy
{
    StandartZombie = 1,
    Runer = 2,
    StrongZombie = 4
}
public enum Spawner
{
    one = 1,
    two = 2,
    both = one | two
}
