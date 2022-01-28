using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityAssetFactory
{
    public static UnityAsset CreateUnityAsset(string assetType)
    {
        switch (assetType)
        {
            case "GameObject":
                return new PrefabAsset();

            case "Sprite":
                return new SpriteAsset();

            default:
                return null;
        }
    }
}

public abstract class UnityAsset
{
    public string resPath;
    protected Object obj;

    public void Load(string resPath)
    {
        this.resPath = resPath;
        obj = Resources.Load(resPath);
    }

    public abstract void Instantiate();

    public abstract void Show();
}

public class PrefabAsset : UnityAsset
{
    public override void Instantiate()
    {
        GameObject.Instantiate(obj as GameObject);
    }

    public override void Show()
    {
        throw new System.NotImplementedException();
    }
}

public class SpriteAsset : UnityAsset
{
    public override void Instantiate()
    {
        GameObject go = new GameObject("Sprite1");
        SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
        Texture2D t2 = obj as Texture2D;
        Sprite sprite = Sprite.Create(t2, new Rect(0, 0, t2.width, t2.height), Vector2.zero);
        spriteRenderer.sprite = sprite;
    }

    public override void Show()
    {
        throw new System.NotImplementedException();
    }
}