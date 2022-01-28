using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //UnityAsset asset = UnityAssetFactory.CreateUnityAsset("Sprite");
        //asset.Load("plane");
        //asset.Instantiate();

        GameObject gameObject = UnitySingletonFactory.instance.GetAsset<GameObject>("GO");
        Instantiate(gameObject);
        Debug.Log(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}