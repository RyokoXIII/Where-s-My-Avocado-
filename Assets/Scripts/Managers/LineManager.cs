using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    #region GlobalVarable

    public LayerMask cantDrawOverLayer;
    public GameObject linePrefab;
    public Rigidbody2D _playerRb;
    public Rigidbody2D bigWoodRb;
    public Rigidbody2D[] smallWoodRbs;
    int _cantDrawOverLayerIndex;

    [Space(30f)]
    public Gradient lineColor;
    public float linePointsMinDistance; // distance between each points in line
    public float lineWidth;

    [SerializeField]
    Camera cam = null;
    [Space(30f)]
    [SerializeField]
    GameObject gameOverMenu;
    [SerializeField]
    GameObject optionMenu, gameMenu, tutorial;

    Line _currentLine;

    // Check if has draw for first time when start level
    bool hasDraw;

    // Line tutorial Animation
    [Space(30f)]
    [SerializeField]
    Animator _handAnim;
    [SerializeField]
    Animator _lineAnim;

    #endregion


    private void Start()
    {
        if (bigWoodRb != null)
        {
            bigWoodRb.isKinematic = true;
        }
        if (smallWoodRbs != null)
        {
            foreach (var smallWoodRb in smallWoodRbs)
            {
                smallWoodRb.isKinematic = true;
            }
        }

        _playerRb.isKinematic = true;
        _cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOverLayer");

        // Animation
        if ((_handAnim != null) && (_lineAnim != null))
        {
            _handAnim.SetBool("IsDrawing", false);
            _lineAnim.SetBool("IsDrawing", false);
        }
    }

    void Update()
    {
        if (gameOverMenu.activeInHierarchy == true || optionMenu.activeInHierarchy == true || gameMenu.activeInHierarchy == true)
        {
            if (_currentLine != null)
            {
                Destroy(_currentLine.gameObject);
            }
        }
        else
        {
            Draw();
            hasDraw = true;
        }
    }

    // Touch input
    void Draw()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
       || Input.GetMouseButtonDown(0))
        {
            BeginDraw();

            // Animation
            if ((_handAnim != null) && (_lineAnim != null))
            {
                _handAnim.SetBool("IsDrawing", true);
                _lineAnim.SetBool("IsDrawing", true);
            }
            tutorial.SetActive(false);
        }

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            || _currentLine != null)
        {
            Drawing();
        }

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
           || Input.GetMouseButtonUp(0))
        {
            EndDraw();
        }
    }

    void BeginDraw()
    {
        _currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();

        _currentLine.UsePhysics(false);
        _currentLine.SetLineColor(lineColor);
        _currentLine.SetPointsMinDistance(linePointsMinDistance);
        _currentLine.SetLineWidth(lineWidth);
    }

    void Drawing()
    {
        Vector2 beginMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.CircleCast(beginMousePos, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);

        if (hit)
        {
            EndDraw();
        }
        else
        {
            _currentLine.AddPoint(beginMousePos);
        }
    }

    void EndDraw()
    {
        if (_currentLine != null)
        {
            if (_currentLine.pointsCount < 2)
            {
                if (hasDraw == false)
                {
                    if (bigWoodRb != null)
                    {
                        bigWoodRb.isKinematic = true;
                    }
                    if (smallWoodRbs != null)
                    {
                        foreach (var smallWoodRb in smallWoodRbs)
                        {
                            smallWoodRb.isKinematic = true;
                        }
                    }

                    _playerRb.isKinematic = true;
                }
            }
            else
            {
                if (hasDraw == true)
                {
                    if (bigWoodRb != null)
                    {
                        bigWoodRb.isKinematic = false;
                    }
                    if (smallWoodRbs != null)
                    {
                        foreach (var smallWoodRb in smallWoodRbs)
                        {
                            smallWoodRb.isKinematic = false;
                        }
                    }

                    _playerRb.isKinematic = false;
                }
                _currentLine.gameObject.layer = _cantDrawOverLayerIndex;
                _currentLine.UsePhysics(true);
                _currentLine = null;
            }
        }
    }
}
