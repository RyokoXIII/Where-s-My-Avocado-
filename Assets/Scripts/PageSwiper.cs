using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSwiper : MonoBehaviour
{
    [SerializeField] GameObject[] pageList;
    int _maxPage = 12;
    int _prevPage, _currentPage;

    public void NextPage()
    {
        if(_currentPage < _maxPage)
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
}
