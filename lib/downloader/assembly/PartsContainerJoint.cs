// Decompiled with JetBrains decompiler
// Type: PartsContainerJoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PartsContainerJoint : MonoBehaviour
{
  [SerializeField]
  private string container_;
  [SerializeField]
  private bool autoInitialize_ = true;
  [SerializeField]
  private PartsContainerJoint.UI2DSpriteJoint[] ui2DSpriteJoints_;
  private GameObject prefab_;

  private IEnumerator Start()
  {
    if (this.autoInitialize_)
      yield return (object) this.initializeAsync();
  }

  public IEnumerator initializeAsync()
  {
    PartsContainerJoint partsContainerJoint = this;
    if (Object.op_Equality((Object) partsContainerJoint.prefab_, (Object) null) && !string.IsNullOrEmpty(partsContainerJoint.container_))
    {
      Future<GameObject> loader = new ResourceObject(partsContainerJoint.container_).Load<GameObject>();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      partsContainerJoint.prefab_ = loader.Result;
      loader = (Future<GameObject>) null;
    }
    if (Object.op_Inequality((Object) partsContainerJoint.prefab_, (Object) null))
    {
      PartsContainer component = partsContainerJoint.prefab_.GetComponent<PartsContainer>();
      if (Object.op_Inequality((Object) component, (Object) null) && partsContainerJoint.ui2DSpriteJoints_ != null && partsContainerJoint.ui2DSpriteJoints_.Length != 0)
      {
        Dictionary<string, Sprite> partsSprite = component.partsSprite;
        Dictionary<string, Material> partsMaterial = component.partsMaterial;
        foreach (PartsContainerJoint.UI2DSpriteJoint ui2DspriteJoint in partsContainerJoint.ui2DSpriteJoints_)
        {
          if (!string.IsNullOrEmpty(ui2DspriteJoint.key_) && !Object.op_Equality((Object) ui2DspriteJoint.to_, (Object) null))
          {
            Sprite sprite;
            if (ui2DspriteJoint.setSprite_ && partsSprite.TryGetValue(ui2DspriteJoint.key_, out sprite))
              ui2DspriteJoint.to_.sprite2D = sprite;
            Material material;
            if (ui2DspriteJoint.setMaterial_ && partsMaterial.TryGetValue(ui2DspriteJoint.key_, out material))
              ((UIWidget) ui2DspriteJoint.to_).material = new Material(material);
          }
        }
      }
    }
    ((Behaviour) partsContainerJoint).enabled = false;
  }

  [Serializable]
  public class UI2DSpriteJoint
  {
    [SerializeField]
    [Tooltip("\"PartsContainer\"の\"name\"と一致したものを\"to_\"へセット")]
    public string key_ = string.Empty;
    [SerializeField]
    [Tooltip("\"PartsContainer\"の素材セット先")]
    public UI2DSprite to_;
    public bool setSprite_ = true;
    public bool setMaterial_ = true;
  }
}
