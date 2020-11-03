using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToExit : MonoBehaviour
{
    [SerializeField]
    GameObject _exitMenu;


    private void Start()
    {
        UIManager.Instance.OnExitMenu += ExitMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UIManager.Instance.ExitMenu();
    }

    void ExitMenu()
    {
        SoundManager.Instance.selectFX.Play();
        _exitMenu.SetActive(true);
    }
}
