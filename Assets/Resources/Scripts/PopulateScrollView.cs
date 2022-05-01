using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PopulateScrollView : MonoBehaviour
{
    void Start()
    {
        foreach (var building in WoodBuildings.Buildings)
            building.InstantiateGameObject(GameObject.Find("Content").transform);
    }
}