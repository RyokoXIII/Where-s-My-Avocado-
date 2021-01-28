using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour, IAnimatable
{
    #region GlobalVarable

    public LayerMask cantDrawOverLayer;
    public GameObject linePrefab;
    public Rigidbody2D playerRb;
    [SerializeField] PlayerManager _playerManager;

    public List<Rigidbody2D> bigWoodRbs;
    public List<Rigidbody2D> smallWoodRbs;
    public List<Rigidbody2D> roundLogRbs;

    int _cantDrawOverLayerIndex;

    [Space(20f)]
    public Gradient lineColor;
    public float linePointsMinDistance; // distance between each points in line
    public float lineWidth;
    [SerializeField] GameObject _trailBlue, _trailRed;

    [SerializeField]
    Camera cam = null;
    [Space(20f)]
    [SerializeField]
    GameObject gameOverMenu;
    [SerializeField]
    GameObject gameMenu, _getChestMenu, tutorial;
    [SerializeField] LevelManager _levelManager;

    Line _currentLine;

    [Header("Animation")]
    [Space(20f)]
    public List<SkeletonAnimation> skeletonAnimationList;
    [SerializeField] AnimationReferenceAsset _dissapeared;

    string _currentAnimation;

    // Check if has draw for first time when start level
    bool hasDraw;
    bool cantDraw;
    bool stopTrail;

    // Line tutorial Animation
    [Space(20f)]
    [SerializeField]
    Animator _handAnim;
    [SerializeField]
    Animator _lineAnim;

    [Space(10f)]
    [SerializeField] Animator _handAnim1;
    [SerializeField] Animator _handAnim2, _handAnim3;
    [SerializeField] Animator _lineAnim1, _lineAnim2, _lineAnim3;

    RaycastHit2D hit;
    Touch touch;

    #endregion

    private void Start()
    {
        SetObjectIsKinematic();

        playerRb.isKinematic = true;
        //_cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOverLayer");

        // Animation
        if ((_handAnim != null) && (_lineAnim != null))
        {
            AssignTutorial();

            _handAnim.SetBool("IsDrawing", false);
            _lineAnim.SetBool("IsDrawing", false);
        }

        Input.multiTouchEnabled = false;
    }

    void Update()
    {
        if (gameOverMenu.activeInHierarchy == true || gameMenu.activeInHierarchy == true
            || _getChestMenu.activeInHierarchy == true)
        {
            if (_currentLine != null)
            {
                Destroy(_currentLine.gameObject);
            }
            stopTrail = true;
        }
        else
        {
            Draw();
            hasDraw = true;

            TrailFollowMouse();
            stopTrail = false;
        }
    }

    // Touch input
    void Draw()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            BeginDraw();
        }

        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Moved) || _currentLine != null)
        {
            Drawing();
        }

        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended)
           || Input.GetMouseButtonUp(0))
        {
            EndDraw();

            // Animation
            if ((_handAnim != null) && (_lineAnim != null))
            {
                _handAnim.SetBool("IsDrawing", true);
                _lineAnim.SetBool("IsDrawing", true);
            }
            tutorial.SetActive(false);
        }
    }

    void BeginDraw()
    {
        _currentLine = Instantiate(linePrefab).GetComponent<Line>();
        _currentLine.transform.parent = gameObject.transform;

        _currentLine.edgeCollide.enabled = false;
        _currentLine.UsePhysics(false);
        _currentLine.SetLineColor(lineColor);
        _currentLine.SetPointsMinDistance(linePointsMinDistance);
        _currentLine.SetLineWidth(lineWidth);
    }

    void Drawing()
    {
        Vector2 beginMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        hit = Physics2D.CircleCast(beginMousePos, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);

        if (hit)
        {
            EndDraw();
            cantDraw = true;
        }
        else
        {
            cantDraw = false;
            _currentLine.AddPoint(beginMousePos);
        }
    }

    void EndDraw()
    {
        if (_currentLine != null)
        {
            if (_currentLine.pointsCount < 2)
            {
                // Destroy line if points < 2 and not hit cant draw layer
                if (!hit)
                {
                    Destroy(_currentLine.gameObject);
                }

                if (hasDraw == false)
                {
                    SetObjectIsKinematic();
                    playerRb.isKinematic = true;
                }
            }
            else
            {
                if (hasDraw == true)
                {
                    SetObjectIsNotKinematic();
                    playerRb.isKinematic = false;

                    SetCharacterState("animation");

                    //_currentLine.gameObject.layer = _cantDrawOverLayerIndex;
                    _currentLine.edgeCollide.enabled = true;
                    _currentLine.UsePhysics(true);
                    _currentLine = null;
                }
            }
        }
    }

    void SetObjectIsKinematic()
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
        if (roundLogRbs != null)
        {
            foreach (var roundLogRb in roundLogRbs)
            {
                roundLogRb.isKinematic = true;
            }
        }
    }

    void SetObjectIsNotKinematic()
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
        if (roundLogRbs != null)
        {
            foreach (var roundLogRb in roundLogRbs)
            {
                roundLogRb.isKinematic = false;
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

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(_currentAnimation))
            return;

        for (int i = 0; i < skeletonAnimationList.Count; i++)
        {
            skeletonAnimationList[i].state.SetAnimation(0, animation, loop).TimeScale = timeScale;
            _currentAnimation = animation.name;
        }
    }

    public void SetCharacterState(string state)
    {
        if (state == "animation")
        {
            SetAnimation(_dissapeared, false, 1f);
        }
    }

    void TrailFollowMouse()
    {
        //Vector3 touchPos = Input.GetTouch(0).position;
        Vector2 _pos;

        if (stopTrail == true)
        {
            _trailBlue.SetActive(false);
            _trailRed.SetActive(false);
        }

        if (cantDraw == false)
        {
            if (_trailBlue.activeInHierarchy == false)
            {
                _trailBlue.SetActive(true);
            }

            _trailRed.SetActive(false);
            _pos = cam.ScreenToWorldPoint(Input.mousePosition);
            _trailBlue.transform.position = _pos;
        }

        if (cantDraw == true)
        {
            if (_trailRed.activeInHierarchy == false)
            {
                _trailRed.SetActive(true);
            }

            _trailBlue.SetActive(false);
            _pos = cam.ScreenToWorldPoint(Input.mousePosition);
            _trailRed.transform.position = _pos;
        }
    }
}
