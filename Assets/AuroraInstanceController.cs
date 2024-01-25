using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuroraInstanceController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Renderer auroraRenderer = GetComponent<Renderer>();
        auroraRenderer.material.SetMatrix("_WorldToLocalMatrix", transform.worldToLocalMatrix);
    }
}
