using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Animation Anim;
    public RectTransform Menu;
    private GameObject _settings;
    [HideInInspector] public bool IsOpen;
    public AudioSource Music;

    private float _menuWidthMax;
    private float _menuWidthMin;
    private float _menuHeightMax;
    private float _menuHeightMin;
    private void Start()
    {
        _menuHeightMax = Camera.main.scaledPixelHeight / 2 + Menu.sizeDelta.y / 2;
        _menuHeightMin = Camera.main.scaledPixelHeight / 2 - Menu.sizeDelta.y / 2;
        _menuWidthMax = Camera.main.scaledPixelWidth / 2 + Menu.sizeDelta.x / 2;
        _menuWidthMin = Camera.main.scaledPixelWidth / 2 - Menu.sizeDelta.x / 2;
        _settings = Menu.transform.gameObject;
        _settings.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MenuClose();
        }
    }
    private void MenuClose()
    {
        if (IsOpen)
        {
            Vector3 input = Input.mousePosition;
            if (input.x > _menuWidthMax |
                input.x < _menuWidthMin |
                input.y > _menuHeightMax |
                input.y < _menuHeightMin)
                StartCoroutine(MenuOFF());
        }
    }
    public void MenuON()
    {
        if (!IsOpen)
        {
            _settings.SetActive(true);
            Anim.Play(AdditionalMenuAnimation.SettingsMenuOpen.ToString());
        }
    }
    private IEnumerator MenuOFF()
    {
        Anim.Play(AdditionalMenuAnimation.SettingsMenuClose.ToString());
        Time.timeScale = 1;
        IsOpen = false;
        yield return new WaitForSeconds(0.3f);
        _settings.SetActive(false);
    }
    public void MusicSlider(Slider slider) => Music.volume = slider.value;
}
