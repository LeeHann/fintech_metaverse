using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMat : MonoBehaviour
{
    public Material[] materials;
    public MeshRenderer[] renderers;

    public void OnClickMat(int idx)
    {
        for (int i =0; i < renderers.Length; i++)
        {
            renderers[i].material = materials[idx];
        }
    }
}
