using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private CameraMove _camera;
    [SerializeField] [Range(0, 1)] private float _num;
    private void Update()
    {
        _material.color = new Color(1, 1, 1, _num);
    }
    public void SetSensitivity(Slider slider) => _camera._sensivity = slider.value;
    private void OnDisable()
    {
        _material.color = new Color(1, 1, 1, 0);
    }
}
