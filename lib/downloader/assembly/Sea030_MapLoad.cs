// Decompiled with JetBrains decompiler
// Type: Sea030_MapLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Sea030_MapLoad : MonoBehaviour
{
  [SerializeField]
  private string atlasName;
  [SerializeField]
  private string spritePrefix;
  [SerializeField]
  private string spriteExt;
  [SerializeField]
  private BoxCollider collider;
  [SerializeField]
  private int width;
  [SerializeField]
  private int height;
  [SerializeField]
  private List<UISprite> sprites;
  private bool isOverlap;
  private BoxCollider myCollider;
  private string spriteName = string.Empty;
  private UIAtlas atlas;

  private void Awake()
  {
    for (int index = 0; index < this.sprites.Count; ++index)
    {
      this.sprites[index].spriteName = string.Empty;
      this.sprites[index].atlas = (UIAtlas) null;
    }
  }

  private void Start()
  {
    this.myCollider = ((Component) this).GetComponent<BoxCollider>();
    if (!Object.op_Equality((Object) this.collider, (Object) null))
      return;
    this.StartCoroutine(this.setSprite());
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.collider, (Object) null))
      return;
    if (!this.isOverlap)
    {
      Bounds bounds = ((Collider) this.myCollider).bounds;
      if (!((Bounds) ref bounds).Intersects(((Collider) this.collider).bounds))
        return;
      this.StartCoroutine(this.setSprite());
      this.isOverlap = true;
    }
    else
    {
      Bounds bounds = ((Collider) this.myCollider).bounds;
      if (((Bounds) ref bounds).Intersects(((Collider) this.collider).bounds))
        return;
      for (int index = 0; index < this.sprites.Count; ++index)
        this.sprites[index].atlas = (UIAtlas) null;
      Resources.UnloadUnusedAssets();
      this.isOverlap = false;
    }
  }

  private IEnumerator setSprite()
  {
    Sea030_MapLoad sea030MapLoad = this;
    for (int index = 0; index < sea030MapLoad.sprites.Count; ++index)
    {
      sea030MapLoad.sprites[index].spriteName = string.Format(sea030MapLoad.spritePrefix + "{0}" + sea030MapLoad.spriteExt, (object) (index + 1));
      ((UIWidget) sea030MapLoad.sprites[index]).width = sea030MapLoad.width;
      ((UIWidget) sea030MapLoad.sprites[index]).height = sea030MapLoad.height;
    }
    IEnumerator e = Singleton<ResourceManager>.GetInstance().LoadResource(((Component) sea030MapLoad).gameObject);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
