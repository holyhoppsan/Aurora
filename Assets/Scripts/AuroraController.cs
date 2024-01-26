using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuroraController : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    public float _AuroraFade = 0.0f;
    private List<Material> _cachedMaterials;

    void Awake()
    {
        _cachedMaterials = new List<Material>();

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            _cachedMaterials.AddRange(renderer.sharedMaterials);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var component in GetComponentsInChildren<AuroraInstanceController>())
        {
            component.fadeIntensity = _AuroraFade;
        }
    }
}
