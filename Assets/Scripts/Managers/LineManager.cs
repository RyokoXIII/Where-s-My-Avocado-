using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    #region GlobalVarable

    public LayerMask cantDrawOverLayer;
    public GameObject linePrefab;
    public GameObject player, bigWood;
    public GameObject[] smallWoods;

    int _cantDrawOverLayerIndex;

    [Space(30f)]
    public Gradient lineColor;
    public float linePointsMinDistance; // distance between each points in line
    public float lineWidth;

    [SerializeField]
    Camera cam = null;
    [Space(30f)] [SerializeField]
    GameObject gameOverMenu;
    [SerializeField]
    GameObject optionMenu, gameMenu, tutorial;

    Line _currentLine;

    // Check if has draw for first time when start level
    bool hasDraw;

    // Line tutorial Animation
    Animator _handAnim, _lineAnim;

    [Space(30f)][SerializeField]
    GameObject _handTut;
    [SerializeField]
    GameObject _lineTut;

    #endregion


    private void Start()
    {
        if (bigWood != null)
        {
            bigWood.GetComponent<Rigidbody2D>().isKinematic = true;
        }
        if (smallWoods != null)
        {
            foreach (var smallWood in smallWoods)
            {
                smallWood.GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }

        player.GetComponent<Rigidbody2D>().isKinematic = true;
        _cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOverLayer");

        // Animation
        if ((_handAnim != null) && (_lineAnim != null))
        {
            _handAnim = _handTut.GetComponent<Animator>();
            _lineAnim = _lineTut.GetComponent<Animator>();
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
                    if (bigWood != null)
                    {
                        bigWood.GetComponent<Rigidbody2D>().isKinematic = true;
                    }
                    if (smallWoods != null)
                    {
                        foreach (var smallWood in smallWoods)
                        {
                            smallWood.GetComponent<Rigidbody2D>().isKinematic = true;
                        }
                    }
                    player.GetComponent<Rigidbody2D>().isKinematic = true;
                }
            }
            else
            {
                if (hasDraw == true)
                {
                    if (bigWood != null)
                    {
                        bigWood.GetComponent<Rigidbody2D>().isKinematic = false;
                    }
                    if (smallWoods != null)
                    {
                        foreach (var smallWood in smallWoods)
                        {
                            smallWood.GetComponent<Rigidbody2D>().isKinematic = false;
                        }
                    }
                    player.GetComponent<Rigidbody2D>().isKinematic = false;
                }
                _currentLine.gameObject.layer = _cantDrawOverLayerIndex;
                _currentLine.UsePhysics(true);
                _currentLine = null;
            }
        }
    }
}
