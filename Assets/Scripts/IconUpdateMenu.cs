using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IconUpdateMenu : MonoBehaviour
{
    [SerializeField] private UpdateMenuControl _updateMenu;
    [SerializeField] private Platform _platform;
    [SerializeField] private UI_Manager _manager;
    [SerializeField] private TextMeshPro _priceText;
    [SerializeField] private Icon _icon;
    private int _price;
    private void Start()
    {
        _platform = _updateMenu.Platform;
        _manager = _updateMenu.Manager;
        if (_platform != null)
        {
            switch (_icon)
            {
                case Icon.Update:
                    _price = _platform.TowerPrice(_platform._model, _platform.GetTowerLevel);
                    break;
                case Icon.Repair:
                    
                    break;
                case Icon.Sell:
                    _price = Mathf.RoundToInt(_platform.CurrentTowerPrice/2);
                    break;
                default:
                    break;
            }
            if(_price == 0) _priceText.text = "MAX";
            else _priceText.text = $"{_price}$";
        }
    }
    private void OnMouseDown()
    {
        switch (_icon)
        {
            case Icon.Update:
                if (_price != 0)
                {
                    if (_manager.CoinCall >= _price)
                    {
                        _manager.CoinCall = -_price;
                        _platform.CurrentTowerPrice = _price;
                        _platform.ChangeTowerAfterUpdate(_platform._model);
                    }
                    else print("Not Enought Money");
                }
                else print("MAX LEVEL");
                break;
            case Icon.Repair:

                break;
            case Icon.Sell:
                _manager.CoinCall = _price;
                _platform.MenuClose(_platform.GetTower.gameObject);
                _platform.GetTower = null;
                _price = 0;
                break;
            default:
                break;
        }
        _platform.MenuTowerRangeClose();
        _platform.MenuClose(_updateMenu.gameObject);
    }
    public enum Icon
    {
        Update = 1,
        Repair = 2,
        Sell = 4
    }
}
