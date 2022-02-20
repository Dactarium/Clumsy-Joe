using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorPicker : MonoBehaviour
{
    public Color[] Colors;

    void Start(){
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        foreach(Material material in meshRenderer.materials){
            if(material.name.Equals("CustomColor (Instance)")){
                material.SetColor("_BaseColor", Colors[Random.Range(0, Colors.Length)]);
            }
        }
    }
}
