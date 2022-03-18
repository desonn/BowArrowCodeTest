using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionManagerCustom : XRInteractionManager
{
    public void ForceDeselect(IXRSelectInteractor interactor)
    {
        if (interactor.hasSelection)
            SelectExit(interactor, interactor.firstInteractableSelected);
    }
}
