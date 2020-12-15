using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSwiper : MonoBehaviour
{
    [SerializeField] GameObject _page1;
    [SerializeField] GameObject[] pageList;
    [SerializeField] GameObject[] levelButtonList;
    int _maxPage = 16;
    int _prevPage, _currentPage;

    SoundManager _soundManager;


    private void Start()
    {
        _soundManager = SoundManager.Instance;

        if (PlayerPrefs.GetInt("level") > 0)
        {
            LoadCurrentPage();
        }
        else
        {
            _page1.SetActive(true);
        }
    }

    public void NextPage()
    {
        _soundManager.selectFX.Play();
        if (_currentPage < _maxPage)
        {
            _prevPage = _currentPage;
            pageList[_prevPage].SetActive(false);

            _currentPage++;
            pageList[_currentPage].SetActive(true);
        }
        else
        {
            _currentPage = _maxPage;
        }
    }

    public void PreviousPage()
    {
        _soundManager.selectFX.Play();
        if (_currentPage > 0)
        {
            _prevPage = _currentPage;
            pageList[_prevPage].SetActive(false);

            _currentPage--;
            pageList[_currentPage].SetActive(true);
        }
        else
        {
            _currentPage = 0;
        }
    }

    void LoadCurrentPage()
    {
        for (int i = 0; i < levelButtonList.Length; i++)
        {
            int levelButtonName = int.Parse(levelButtonList[i].name);

            if (PlayerPrefs.GetInt("level").Equals(levelButtonName))
            {
                for (int j = 0; j < pageList.Length; j++)
                {
                    if (levelButtonList[i].GetComponent<LevelSelector>().levelPageID == pageList[j].name)
                    {
                        pageList[j].SetActive(true);
                        _currentPage = j;
                    }
                }
            }
        }
    }
}
