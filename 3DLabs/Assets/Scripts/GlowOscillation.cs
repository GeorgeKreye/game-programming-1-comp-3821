using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowOscillation : MonoBehaviour
{
    [Tooltip("The Material used by this GameObject")]
    [SerializeField] private Material material;

    [Header("Parameters")]
    [Tooltip("The default emission value")]
    [SerializeField] private float normal;

    [Tooltip("The max/min emission value")]
    [SerializeField] private float amplitude;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float newEmission = (Mathf.Sin(Time.time) * amplitude) + normal;
        material.SetVector("Emission", new Vector4(0, newEmission, newEmission,
            newEmission));
    }
}
