using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IconWeaponMenu : MonoBehaviour
{
    [SerializeField] private WeaponMenuControl _weaponMenu;
    [SerializeField] private Platform _platform;
    [SerializeField] private UI_Manager _manager;
    [SerializeField] private TextMeshPro _price;
    [SerializeField] private Model _weapon;
    [SerializeField] private float _baseRange;

    private int price;
    private bool isPressed;
    private void Start()
    {
        _platform = _weaponMenu.Platform;
        _manager = _weaponMenu.Manager;
        if(_platform != null) 
        { 
            price = _platform.TowerPrice(_weapon, 0);
            _price.text = $"{price}$";
        }
    }
    private void OnMouseEnter()
    {
        
    }
    private void OnMouseExit()
    {
        _platform.MenuTowerRangeClose();
        isPressed = false;
    }
    private void OnMouseDown()
    {
        
        if (_manager.CoinCall >= price)
        {
            if (isPressed)
            {
                if (_platform.TowerChoose(_weapon))
                {
                    _manager.CoinCall = -price;
                    _platform.CurrentTowerPrice = price;
                    _platform._model = _weapon;
                }
                else Debug.Log(_weapon.ToString());
            }
        }
        else Debug.Log("Not enought money");
        if (!isPressed)
        {
            _platform.MenuTowerRangeOpen(_baseRange);
            isPressed = true;
        }
        else
        {
            _platform.MenuTowerRangeClose();
            _platform.MenuClose(_weaponMenu.gameObject);
            isPressed = false;
        }
    }
    
}

