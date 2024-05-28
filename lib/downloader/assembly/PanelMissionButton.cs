// Decompiled with JetBrains decompiler
// Type: PanelMissionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using UnityEngine;

#nullable disable
public class PanelMissionButton : MypageEventButton
{
  [SerializeField]
  private GameObject mEffectObj;

  public override bool IsActive() => !SMManager.Get<Player>().is_bingo_end;

  public override bool IsBadge() => SMManager.Get<Player>().is_open_bingo;

  public override void SetActive(bool value)
  {
    base.SetActive(value);
    this.mEffectObj.SetActive(value);
  }
}
