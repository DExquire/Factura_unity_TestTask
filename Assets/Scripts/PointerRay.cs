using UnityEngine;

public class VisibleLineFromObject : MonoBehaviour
{
    [Header("Line Settings")]
    public Vector3 direction = Vector3.up;
    public float distance = 5f;                 
    public Color lineColor = Color.red;         
    public float lineWidth = 0.1f;              

    public Transform originObject;
    private LineRenderer lineRenderer;

    void Start()
    {
        if (!TryGetComponent(out lineRenderer))
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = lineColor };
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        Vector3 startPoint = originObject.position;
        Vector3 endPoint = startPoint + transform.TransformDirection(direction) * distance;

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}