using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTower : MonoBehaviour
{
    public Platform Platform;
    private void OnMouseDown()
    {
        Platform.MenuOpen(Platform.TowerMenu);
    }
}
