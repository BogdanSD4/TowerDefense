using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlow : MonoBehaviour
{
    [SerializeField] private Material _water;
    private float _currentFlow = 0;
    [SerializeField][Range(1,100)]private float _speedFlow;
    private float _time = 0;
    private float _timer = 0;
    private int _valueChange = 0;
    private void Start()
    {
        _water.SetTextureOffset("_MainTex", new Vector2(0,0));
    }
    void Update()
    {
        if(_time + Time.deltaTime < _timer + 1)
        {
            _timer += _time;
            _currentFlow += _speedFlow/50000;
            if (_currentFlow > 1) _currentFlow = 0;
            if(_valueChange == 1000)
            {
                _speedFlow = Random.Range(20, 100);
                _time = 0;
                _timer = 0;
                _valueChange = 0;
            }
            _valueChange++;
        }
        _water.SetTextureOffset("_MainTex", new Vector2(0, -_currentFlow));
    }
}
