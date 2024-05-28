// Decompiled with JetBrains decompiler
// Type: Battle01TipEventSkillexp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle01TipEventSkillexp : Battle01TipEventBase
{
  private UnitIcon unitIcon;

  public override IEnumerator onInitAsync()
  {
    Battle01TipEventSkillexp tipEventSkillexp = this;
    Future<GameObject> f = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tipEventSkillexp.unitIcon = tipEventSkillexp.cloneIcon<UnitIcon>(f.Result);
    tipEventSkillexp.selectIcon(0);
  }

  private IEnumerator doSetIcon(UnitUnit unit)
  {
    IEnumerator e = this.unitIcon.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIcon.BottomModeValue = UnitIconBase.BottomMode.Nothing;
  }

  public override void setData(BL.DropData e, BL.Unit unit)
  {
    Debug.LogWarning((object) (" ==== setData:" + (object) e.reward.Type));
    if (e.reward.Type != MasterDataTable.CommonRewardType.gear_experience_point)
      return;
    this.setText(Consts.Format(Consts.GetInstance().TipEvent_text_skillexp, (IDictionary) new Dictionary<string, string>()
    {
      ["skill"] = unit.playerUnit.equippedGearName
    }));
    Singleton<NGBattleManager>.GetInstance().StartCoroutine(this.doSetIcon(unit.unit));
  }
}
