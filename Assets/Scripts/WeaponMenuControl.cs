using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMenuControl : MonoBehaviour
{
    public UI_Manager Manager;
    public Platform Platform;
    private Camera _camera;
    private bool _collision = true;
    private const int _LAYER_MASK = 5;
    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        if (hit.transform.gameObject.layer != _LAYER_MASK && !_collision)
        {
            Platform.MenuClose(transform.gameObject);
        }
    }
    private void OnMouseExit()
    {
        _collision = false;
    }
}
