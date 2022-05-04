using UnityEngine;

public class PopulateScrollView : MonoBehaviour
{
    private void Start()
    {
        var index = 0;
        foreach (var building in WoodBuildings.Buildings)
            building.InstantiateGameObject(Instantiate(
                    Resources.Load<GameObject>("Prefabs/ShopItem"),
                    GameObject.Find("Content").transform, false), index++
            );
    }
}