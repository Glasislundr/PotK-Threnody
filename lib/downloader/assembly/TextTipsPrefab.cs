// Decompiled with JetBrains decompiler
// Type: TextTipsPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class TextTipsPrefab : MonoBehaviour
{
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private UILabel subtitle;
  [SerializeField]
  private UILabel description;
  [SerializeField]
  private UILabel sourceName;

  public void Init(TipsTextTips tips)
  {
    this.title.text = tips.title;
    this.subtitle.text = tips.subtitle;
    this.description.text = tips.description;
    this.sourceName.text = tips.sourcename;
  }
}
