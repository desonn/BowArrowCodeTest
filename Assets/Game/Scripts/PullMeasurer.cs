using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PullMeasurer : XRBaseInteractable
{
    
    public class PullEvent : UnityEvent<Vector3, float> { }
    public PullEvent Pulled = new PullEvent();

    public Transform start;
    public Transform end;

    float pullAmount = 0.0f;
    public float PullAmount => pullAmount;

     XRBaseInteractor pullingInteractor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Set interactor
        pullingInteractor = args.interactorObject as XRBaseInteractor;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Clear 
        pullingInteractor = null;

        // Reset 
        SetPullValues(start.position, 0.0f);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected)
        {
           
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
                CheckForPull();
        }
    }

     void CheckForPull()
    {
        
        Vector3 interactorPosition = pullingInteractor.transform.position;

      
        float newPullAmount = CalculatePull(interactorPosition);
        Vector3 newPullPosition = CalculatePosition(newPullAmount);

       
        SetPullValues(newPullPosition, newPullAmount);
    }

     float CalculatePull(Vector3 pullPosition)
    {
       
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;

        // pull 
        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();

        //  distance
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        pullValue = Mathf.Clamp(pullValue, 0.0f, 1.0f);

        return pullValue;
    }

     Vector3 CalculatePosition(float amount)
    {
       
        return Vector3.Lerp(start.position, end.position, amount);
    }

     void SetPullValues(Vector3 newPullPosition, float newPullAmount)
    {
        
        if (newPullAmount != pullAmount)
        {
            pullAmount = newPullAmount;
            Pulled?.Invoke(newPullPosition, newPullAmount);
        }
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        
        return base.IsSelectableBy(interactor) && IsDirectInteractor(interactor as XRBaseInteractor);
    }

     bool IsDirectInteractor(XRBaseInteractor interactor)
    {
        return interactor is XRDirectInteractor;
    }

     void OnDrawGizmos()
    {
       
        if (start && end)
            Gizmos.DrawLine(start.position, end.position);
    }
}
