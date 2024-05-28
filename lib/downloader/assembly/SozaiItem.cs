// Decompiled with JetBrains decompiler
// Type: SozaiItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SozaiItem : MonoBehaviour
{
  [SerializeField]
  private GameObject linkSozaiBase;
  [SerializeField]
  private GameObject dirSozaiBase;
  [SerializeField]
  private GameObject dirSozaiBase2;
  [SerializeField]
  private UILabel txtPossessionNum;
  [SerializeField]
  private GameObject wLine;
  [NonSerialized]
  public UnitIcon UnitIcon;

  public GameObject LinkSozaiBase => this.linkSozaiBase;

  public GameObject DirSozaiBase => this.dirSozaiBase;

  public GameObject DirSozaiBase2 => this.dirSozaiBase2;

  public UILabel TxtPossessionNum => this.txtPossessionNum;

  public GameObject WLine => this.wLine;

  public void SetOnlyWLine()
  {
    foreach (Transform transform in ((Component) this).transform)
    {
      if (Object.op_Equality((Object) ((Component) transform).gameObject, (Object) this.wLine))
        ((Component) transform).gameObject.SetActive(true);
      else
        ((Component) transform).gameObject.SetActive(false);
    }
  }
}
