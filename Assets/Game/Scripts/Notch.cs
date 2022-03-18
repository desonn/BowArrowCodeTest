using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

[RequireComponent(typeof(PullMeasurer))]
public class Notch : XRSocketInteractor
{
    
    [Range(0, 1)] public float releaseThreshold = 0.25f;

   
    public PullMeasurer PullMeasurer { get;  set; }
    public bool IsReady { get;  set; } = false;

   
     InteractionManagerCustom CustomManager => interactionManager as InteractionManagerCustom;

    protected override void Awake()
    {
        base.Awake();
        PullMeasurer = GetComponent<PullMeasurer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

     
        PullMeasurer.selectExited.AddListener(ReleaseArrow);

       
        PullMeasurer.Pulled.AddListener(MoveAttach);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PullMeasurer.selectExited.RemoveListener(ReleaseArrow);
        PullMeasurer.Pulled.RemoveListener(MoveAttach);
    }

    public void ReleaseArrow(SelectExitEventArgs args)
    {
      
        if (firstInteractableSelected is Arrow && PullMeasurer.PullAmount > releaseThreshold)
            CustomManager.ForceDeselect(this);
    }

    public void MoveAttach(Vector3 pullPosition, float pullAmount)
    {
       
        attachTransform.position = pullPosition;
    }

    public void SetReady(BaseInteractionEventArgs args)
    {
       
        IsReady = args.interactableObject.transform;
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
      
        return base.CanSelect(interactable) && CanHover(interactable as IXRHoverInteractable) && IsArrow(interactable as XRBaseInteractable) && IsReady;
    }

     bool IsArrow(XRBaseInteractable interactable)
    {
       
        return interactable is Arrow;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
       
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }

    
#pragma warning disable CS0672
    public override bool requireSelectExclusive => false;

}
