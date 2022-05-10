using UnityEngine;

public class PopulateScrollView : MonoBehaviour
{
    public GameObject space;

    public void Populate(User user)
    {
        var index = 0;
        foreach (var building in WoodBuildings.Buildings)
            user.ShopItemBuilding.Add(
                building.InstantiateGameObject(Instantiate(
                        Resources.Load<GameObject>("Prefabs/ShopItem"),
                        GameObject.Find("Content").transform, false), index++
                )
            );
        Instantiate(space, GameObject.Find("Content").transform, false);
        index = 0;
        foreach (var store in CoinBuildings.Stores)
            user.ShopItemStore.Add(
                store.InstantiateGameObject(Instantiate(
                        Resources.Load<GameObject>("Prefabs/ShopItem"),
                        GameObject.Find("ContentCoin").transform, false), index++
                )
            );
        Instantiate(space, GameObject.Find("ContentCoin").transform, false);
    }
}