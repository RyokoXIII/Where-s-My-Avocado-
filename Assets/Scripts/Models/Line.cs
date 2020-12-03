using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    #region Global Variables

    public LineRenderer lineRenderer;
    public Rigidbody2D rb;
    public EdgeCollider2D edgeCollide;
    [SerializeField] PhysicsMaterial2D _circleBoost;

    [HideInInspector]
    public List<Vector2> points = new List<Vector2>();
    [HideInInspector]
    public int pointsCount = 0;

    float _pointsMinDistance = 0.1f;
    float _circleColliderRadius;

    #endregion


    // Add point collider to line
    public void AddPoint(Vector2 newPoint)
    {
        if (pointsCount >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < _pointsMinDistance)
            return;

        points.Add(newPoint);
        pointsCount++;

        // Add Circle Collider to each Points
        CircleCollider2D _circleCollider = this.gameObject.AddComponent<CircleCollider2D>();
        _circleCollider.offset = newPoint;
        _circleCollider.radius = _circleColliderRadius;
        _circleCollider.sharedMaterial = _circleBoost;

        // Line Renderer
        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(pointsCount - 1, newPoint);

        // Edge Collider
        if (pointsCount > 1)
            edgeCollide.points = points.ToArray();
    }

    public Vector2 GetLastPoint()
    {
        return (Vector2) lineRenderer.GetPosition(pointsCount - 1);
    }

    public void UsePhysics(bool usePhysic)
    {
        rb.isKinematic = !usePhysic;
    }

    public void SetLineColor(Gradient gradient)
    {
        lineRenderer.colorGradient = gradient;
    }

    public void SetPointsMinDistance(float distance)
    {
        _pointsMinDistance = distance;
    }

    public void SetLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        _circleColliderRadius = width / 2f;
        edgeCollide.edgeRadius = _circleColliderRadius;
    }
}
