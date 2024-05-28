// Decompiled with JetBrains decompiler
// Type: Popup004UnitTransformResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup004UnitTransformResult : BackButtonMenuBase
{
  [SerializeField]
  private UILabel TxtMoveUnit;
  [SerializeField]
  private UILabel TxtUnitPossession;
  [SerializeField]
  private UILabel TxtStoragePosession;

  public void Init(int moveUnit, int unitPossession, int storagePossession, Player player)
  {
    this.TxtMoveUnit.SetTextLocalize(Consts.Format(Consts.GetInstance().popup_004_Unit_Transform_Resulut, (IDictionary) new Hashtable()
    {
      {
        (object) "count",
        (object) moveUnit
      }
    }));
    this.TxtUnitPossession.SetTextLocalize(string.Format("{0}/{1}", (object) unitPossession, (object) player.max_units));
    this.TxtStoragePosession.SetTextLocalize(string.Format("{0}/{1}", (object) storagePossession, (object) player.max_unit_reserves));
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();
}
