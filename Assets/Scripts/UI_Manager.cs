using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthIcon;
    [SerializeField] private TextMeshProUGUI _coinIcon;
    [SerializeField] private Animation _healthAnim;
    [SerializeField] private Animation _startAnim;
    [SerializeField] private int _health;
    [SerializeField] private int _coin;
    [Space]
    [SerializeField] private TextMeshProUGUI _startMenu;
    [SerializeField] private WavesControl _wavesControl;
    [Header("GameOverMenu")]
    [SerializeField] private GameOverMenu _gameOver;
    [Header("SettingsMenu")]
    [SerializeField] private SettingsMenu _settingsMenu;
    [Header("GameSpeed")]
    [SerializeField] private TextMeshProUGUI _gameSpeedText;

    private bool _isOpenGameOverMenu;
    private static int _dayCount;
    private void Awake()
    {
        int from = _dayCount;
        if(from != 0)
        {
            string num = "";
            if (from < 10) num = "0";
            _startMenu.text = $"<color=black>Day</color> <color=red>{num}{from}</color>";
        }
        Time.timeScale = 3;
        GameSpeed();
    }
    private void Start()
    {
        Manager._coin = _coin;
        Manager._health = _health;
        _healthIcon.text = Manager._health.ToString();
        _coinIcon.text = Manager._coin.ToString();
        if (_dayCount == 0)
        {
            StartCoroutine(Day(99, 0, -1));
        }
        else StartCoroutine(Day(_dayCount, _dayCount, -1));
        _wavesControl.enabled = false;
    }
    IEnumerator Day(int from, int to, int step)
    {
        yield return new WaitForSeconds(1);
        while (from != to)
        {
            float time = 0.01f;
            string num = "";
            if (from < 10) num = "0";
            if (from < 20)
            {
                time = 0.1f;
                if (from < 5) time = 0.2f;
            }
            _startMenu.text = $"<color=black>Day</color> <color=red>{num}{from}</color>";
            yield return new WaitForSeconds(time);
            from += step;
        }
        if (_dayCount == 0) _dayCount++;
        yield return new WaitForSeconds(1);
        _startAnim.Play(AdditionalMenuAnimation.StartMenuClose.ToString());
        WayStart();
    }
    public void PlayerWin()
    {
        WavesControl._unitsInSceneForCheckWin--;
        if (WavesControl._unitsInSceneForCheckWin == 0)
        {
            PlayGameMusic(MusicType.MusicBeforeWave);
            if(Manager._health > 0 && _wavesControl.CurrentWave())
            {
                _dayCount++;
                ActionState = Actions.PlayerWin;
                _startAnim.Play(AdditionalMenuAnimation.StartMenuOpen.ToString());
                _startMenu.text = $"<color=black>Day</color> <color=red>complete</color>";
                PlayShotSFX(SFXType.WinSound);
                Debug.Log("DayComplete");
            }
        }
    }
    private void WayStart() => _wavesControl.enabled = true;
    public void ObjectOff(GameObject _object) => _object.SetActive(false); 
    public int HealthCall
    {
        get { return Manager._health; }
        set
        {
            _health -= value;
            Checker();
            _healthAnim.Play();
            if (_health < 4) _healthIcon.color = Color.red;
            if (_health <= 0)
            {
                _health = 0;
                if (!_isOpenGameOverMenu)
                {
                    _gameOver.gameObject.SetActive(true);
                    GetComponent<Animation>().Play(AdditionalMenuAnimation.GameOverMenuOpen.ToString());
                    PlayShotSFX(SFXType.LoseSound);
                    _isOpenGameOverMenu = true;
                }
            }
            _healthIcon.text = _health.ToString();
        }
    }
    public int CoinCall
    {
        get { return Manager._coin; }
        set
        {
            _coin += value;
            Checker();
            _coinIcon.text = _coin.ToString();
        }
    }
    private void Checker()
    {
        Manager._coin = _coin;
        Manager._health = _health;
    }
    public void GameSpeed()
    {
        Time.timeScale++;
        string color;
        if (Time.timeScale > 3) Time.timeScale = 1;
        switch (Time.timeScale)
        {
            case 1: color = "0094FF";
                break;
            case 2:
                color = "FF4400";
                break;
            case 3:
                color = "FF0003";
                break;
            default: color = "000";
                break;
        }
        _gameSpeedText.text = $"<color=#{color}>Speed x{Time.timeScale}</color>";
    }
    public void RestartButton()
    {
        _startAnim.Play(AdditionalMenuAnimation.GameOverMenuClose.ToString());
        ActionState = Actions.Restart;
    }
    public Actions ActionState { get; private set; }
    public void ActionListForGameOverMenu()
    {
        switch (ActionState)
        {
            case Actions.Restart: _startAnim.Play(AdditionalMenuAnimation.StartMenuOpen.ToString());
                break;
            case Actions.MainMenu:
                break;
            case Actions.Shop:
                break;
            default:
                break;
        }
        _isOpenGameOverMenu = false;
    }
    public void ActionListForStartMenu()
    {
        switch (ActionState)
        {
            case Actions.Restart:
                Restart();
                break;
            case Actions.MainMenu:
                break;
            case Actions.Shop:
                break;
            case Actions.PlayerWin:
            default:
                break;
        }
    }
    private void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void AnnounsOpeningMenu(Menu menu)
    {
        switch (menu)
        {
            case Menu.Settings: _settingsMenu.IsOpen = true;
                break;
            case Menu.GameOver:
                break;
            default:
                break;
        }
        Time.timeScale = 0;
    }
    public void PlayShotSFX(SFXType _sfx)
    {
        AudioManager.PlaySFX(_sfx);
    }
    public void PlayGameMusic(MusicType _music)
    {
        AudioManager.PlayAudio(_music);
    }
}
public static class Manager
{
    static public int _health = 100;
    static public int _coin = 500;
}
public enum AdditionalMenuAnimation
{
    GameOverMenuOpen,
    GameOverMenuClose,
    SettingsMenuOpen,
    SettingsMenuClose,
    StartMenuOpen,
    StartMenuClose
}
public enum Menu
{
    Settings,
    GameOver
}
public enum Actions
{
    Restart,
    Shop,
    MainMenu,
    PlayerWin
}
