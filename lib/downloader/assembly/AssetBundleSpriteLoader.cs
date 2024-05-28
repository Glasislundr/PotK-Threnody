// Decompiled with JetBrains decompiler
// Type: AssetBundleSpriteLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class AssetBundleSpriteLoader : MonoBehaviour, ISimpleScrollContent
{
  [Tooltip("Resouces以下のパスを拡張子なしで入力. Material読込は末尾に.mat")]
  public string Path = string.Empty;
  public bool AutoLoad = true;

  public bool isDone { get; private set; }

  private IEnumerator Start()
  {
    if (this.AutoLoad)
      yield return (object) this.Load();
  }

  public IEnumerator Load()
  {
    this.isDone = false;
    string[] strArray = this.Path.Split('.');
    IEnumerator e;
    if (strArray[strArray.Length - 1].Equals("mat"))
    {
      e = this.LoadMaterial(strArray[0]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.LoadSprite(strArray[0]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.isDone = true;
  }

  private IEnumerator LoadSprite(string path)
  {
    AssetBundleSpriteLoader bundleSpriteLoader = this;
    Future<Sprite> loader = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = loader.Result;
    UI2DSprite component1 = ((Component) bundleSpriteLoader).GetComponent<UI2DSprite>();
    if (Object.op_Inequality((Object) component1, (Object) null))
    {
      component1.sprite2D = result;
    }
    else
    {
      SpriteRenderer component2 = ((Component) bundleSpriteLoader).GetComponent<SpriteRenderer>();
      if (Object.op_Inequality((Object) component2, (Object) null))
        component2.sprite = result;
    }
  }

  private IEnumerator LoadMaterial(string path)
  {
    AssetBundleSpriteLoader bundleSpriteLoader = this;
    Future<Material> loader = Singleton<ResourceManager>.GetInstance().Load<Material>(path);
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Material result = loader.Result;
    UI2DSprite component1 = ((Component) bundleSpriteLoader).GetComponent<UI2DSprite>();
    if (Object.op_Inequality((Object) component1, (Object) null))
    {
      ((UIWidget) component1).material = result;
    }
    else
    {
      SpriteRenderer component2 = ((Component) bundleSpriteLoader).GetComponent<SpriteRenderer>();
      if (Object.op_Inequality((Object) component2, (Object) null))
        ((Renderer) component2).material = result;
    }
  }
}
