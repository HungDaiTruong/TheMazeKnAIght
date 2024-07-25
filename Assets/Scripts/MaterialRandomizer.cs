using System.Collections.Generic;
using UnityEngine;

public class MaterialRandomizer : MonoBehaviour
{
    [SerializeField] private List<Material> materials; // List of materials to choose from
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (materials.Count > 0)
        {
            RandomizeMaterial();
        }
        else
        {
            Debug.LogWarning("Material list is empty. Please add materials in the inspector.");
        }
    }

    void RandomizeMaterial()
    {
        int randomIndex = Random.Range(0, materials.Count);
        objectRenderer.material = materials[randomIndex];
    }
}
