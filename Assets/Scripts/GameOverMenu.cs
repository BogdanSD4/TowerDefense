using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Animation _anim;
    [SerializeField] private Light _world;
    private Color _color;
    private void Start()
    {
        _color = Color.black;
        this.gameObject.SetActive(false);
    }
    public void ShopButton() => Debug.Log("Comming soon...");
    public void RestartButton() => _anim.Play(AdditionalMenuAnimation.GameOverMenuClose.ToString());
    private void Update()
    {
        _world.color = Color.Lerp(_world.color, _color, 1f * Time.deltaTime);
    }
}
