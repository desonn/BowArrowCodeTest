using UnityEngine;

public class Target : MonoBehaviour, IArrowHittable
{
    
    public Material otherMaterial;

    public void Hit(Arrow arrow)
    {
        ApplyMaterial();
      
    }

     void ApplyMaterial()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = otherMaterial;
    }


}
