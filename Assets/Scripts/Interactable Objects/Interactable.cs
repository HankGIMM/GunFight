using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private Material originalMaterial;
    private Material highlightMaterial;

    void Start()
    {
       originalMaterial = GetComponent<Renderer>().material;
    }
    public abstract void Interact();

    public void Highlight()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void Unhighlight()
    {
        GetComponent<Renderer>().material = originalMaterial;
    }
}