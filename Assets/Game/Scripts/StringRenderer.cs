using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class StringRenderer : MonoBehaviour
{
  
    public Gradient pullColor;

    
    public PullMeasurer pullMeaserer;

    
    public Transform start;
    public Transform middle;
    public Transform end;

     LineRenderer lineRenderer;

     void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
       
        if (Application.isEditor && !Application.isPlaying)
            UpdatePositions();
    }

     void OnEnable()
    {
       
        Application.onBeforeRender += UpdatePositions;

      
        pullMeaserer.Pulled.AddListener(UpdateColor);
    }

     void OnDisable()
    {
        Application.onBeforeRender -= UpdatePositions;  
        pullMeaserer.Pulled.RemoveListener(UpdateColor);
    }

     void UpdatePositions()
    {
        
        Vector3[] positions = new Vector3[] { start.position, middle.position, end.position };
        lineRenderer.SetPositions(positions);
    }

     void UpdateColor(Vector3 pullPosition, float pullAmount)
    {
      
        Color color = pullColor.Evaluate(pullAmount);
        lineRenderer.material.color = color;
    }
}
