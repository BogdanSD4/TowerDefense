using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowControl : MonoBehaviour
{
    [SerializeField] private GameObject[] _arrows;
    [SerializeField] private Material _targetMaterial;
    public void ChoseArrow(Spawner spawner)
    {
        switch (spawner)
        {
            case Spawner.one: 
                _arrows[0].SetActive(true);
                break;
            case Spawner.two:
                _arrows[1].SetActive(true);
                break;
            case Spawner.both:
                _arrows[2].SetActive(true);
                break;
        }
        StartCoroutine(Blinking(_targetMaterial));
    }
    public void CloseArrow()
    {
        for(int i = 0; i < _arrows.Length; i++)
        {
            _arrows[i].SetActive(false);
        }
    }
    IEnumerator Blinking(Material image)
    {
        Color c = image.color;

        float alpha = 0.2f;

        while (true)
        {
            c.a = Mathf.MoveTowards(c.a, alpha, Time.deltaTime/10);

            image.color = c;

            if (c.a == alpha)
            {
                if (alpha == 0.2f)
                {
                    alpha = 0.1f;
                }
                else
                    alpha = 0.2f;
            }

            yield return null;
        }
    }
}
