// Decompiled with JetBrains decompiler
// Type: Shop00723AnimeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Shop00723AnimeManager : MonoBehaviour
{
  [SerializeField]
  private UILabel txtInfo;

  public void SetInfo(string info) => this.txtInfo.SetTextLocalize(info);
}
