using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : XRGrabInteractable
{
    
    public float speed = 2000.0f;

    
    public Transform tip;
    public LayerMask layerMask = ~Physics.IgnoreRaycastLayer;

     new Collider collider ;
     new Rigidbody rigidbody;

     Vector3 lastPosition = Vector3.zero;
     bool launched = false;

    protected override void Awake()
    {
        base.Awake();
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        
        if (args.interactorObject is XRDirectInteractor)
            Clear();

      
        base.OnSelectEntering(args);
        
    }

     void Clear()
    {
        SetLaunch(false);
        TogglePhysics(true);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
       
        base.OnSelectExited(args);

        //launch the arrow
        if (args.interactorObject is Notch notch)
            Launch(notch);
    }

     void Launch(Notch notch)
    {
        //check if the bow is dropped with arrow
        if (notch.IsReady)
        {
            SetLaunch(true);
            UpdateLastPosition();
            ApplyForce(notch.PullMeasurer);
        }
    }

     void SetLaunch(bool value)
    {
        collider.isTrigger = value;
        launched = value;
    }

     void UpdateLastPosition()
    {
      
        lastPosition = tip.position;
    }

     void ApplyForce(PullMeasurer pullMeasurer)
    {
        // force to the arrow
        float power = pullMeasurer.PullAmount;
        Vector3 force = transform.forward * (power * speed);
        rigidbody.AddForce(force);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (launched)
        {
          
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (CheckForCollision())
                    launched = false;

                UpdateLastPosition();
            }

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed)
                SetDirection();
        }
    }

     void SetDirection()
    {
        
        if (rigidbody.velocity.z > 0.5f)
            transform.forward = rigidbody.velocity;
    }

     bool CheckForCollision()
    {
     
        if (Physics.Linecast(lastPosition, tip.position, out RaycastHit hit, layerMask))
        {
            TogglePhysics(false);
            ChildArrow(hit);
            CheckForHittable(hit);
        }

        return hit.collider != null;
    }

     void TogglePhysics(bool value)
    {
        
        rigidbody.isKinematic = !value;
        rigidbody.useGravity = value;
    }

     void ChildArrow(RaycastHit hit)
    {
       
        Transform newParent = hit.collider.transform;
        transform.SetParent(newParent);
    }

     void CheckForHittable(RaycastHit hit)
    {
        
        GameObject hitObject = hit.transform.gameObject;
        IArrowHittable hittable = hitObject ? hitObject.GetComponent<IArrowHittable>() : null;

        
        if (hittable != null)
        {
            hittable.Hit(this);
            UIManager.Instance.UpdateScore();
        }
    }
}
