using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    #region GlobalVarable

    public LayerMask cantDrawOverLayer;
    public GameObject linePrefab;
    public Rigidbody2D playerRb;
    [SerializeField] PlayerManager _playerManager;

    public List<Rigidbody2D> bigWoodRbs;
    public List<Rigidbody2D> smallWoodRbs;

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
    GameObject gameMenu, tutorial;
    [SerializeField] LevelManager _levelManager;

    Line _currentLine;

    // Check if has draw for first time when start level
    bool hasDraw;

    // Line tutorial Animation
    [Space(30f)]
    [SerializeField]
    Animator _handAnim;
    [SerializeField]
    Animator _lineAnim;

    [Space(10f)]
    [SerializeField] Animator _handAnim1;
    [SerializeField] Animator _handAnim2, _handAnim3;
    [SerializeField] Animator _lineAnim1, _lineAnim2, _lineAnim3;

    #endregion


    private void Start()
    {
        if (bigWoodRbs != null)
        {
            foreach(var bigWoodRb in bigWoodRbs)
            {
                bigWoodRb.isKinematic = true;
            }
        }
        if (smallWoodRbs != null)
        {
            foreach (var smallWoodRb in smallWoodRbs)
            {
                smallWoodRb.isKinematic = true;
            }
        }

        playerRb.isKinematic = true;
        _cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOverLayer");

        // Animation
        if ((_handAnim != null) && (_lineAnim != null))
        {
            AssignTutorial();

            _handAnim.SetBool("IsDrawing", false);
            _lineAnim.SetBool("IsDrawing", false);
        }
    }

    void Update()
    {
        if (gameOverMenu.activeInHierarchy == true || gameMenu.activeInHierarchy == true)
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
                    if (bigWoodRbs != null)
                    {
                        foreach (var bigWoodRb in bigWoodRbs)
                        {
                            bigWoodRb.isKinematic = true;
                        }
                    }
                    if (smallWoodRbs != null)
                    {
                        foreach (var smallWoodRb in smallWoodRbs)
                        {
                            smallWoodRb.isKinematic = true;
                        }
                    }
                    playerRb.isKinematic = true;
                }
            }
            else
            {
                if (hasDraw == true)
                {
                    if (bigWoodRbs != null)
                    {
                        foreach (var bigWoodRb in bigWoodRbs)
                        {
                            bigWoodRb.isKinematic = false;
                        }
                    }
                    if (smallWoodRbs != null)
                    {
                        foreach (var smallWoodRb in smallWoodRbs)
                        {
                            smallWoodRb.isKinematic = false;
                        }
                    }
                    playerRb.isKinematic = false;
                    _playerManager.SetCharacterState("circle");
                }
                _currentLine.gameObject.layer = _cantDrawOverLayerIndex;
                _currentLine.UsePhysics(true);
                _currentLine = null;
            }
        }
    }

    void AssignTutorial()
    {
        if ((_levelManager._tutorialHand1.activeInHierarchy == true) &&
                (_levelManager._lineTut1.activeInHierarchy == true))
        {
            _handAnim = _handAnim1;
            _lineAnim = _lineAnim1;
        }
        else if ((_levelManager._tutorialHand2.activeInHierarchy == true) &&
            (_levelManager._lineTut2.activeInHierarchy == true))
        {
            _handAnim = _handAnim2;
            _lineAnim = _lineAnim2;
        }
        else
        {
            _handAnim = _handAnim3;
            _lineAnim = _lineAnim3;
        }
    }
}
