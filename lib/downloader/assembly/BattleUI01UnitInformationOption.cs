// Decompiled with JetBrains decompiler
// Type: BattleUI01UnitInformationOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI01UnitInformationOption : BattleUI01UnitInformationTab
{
  [SerializeField]
  [Tooltip("武器表示")]
  private BattleUI01UnitInformation.WeaponRow[] weapons_;

  public override IEnumerator initialize()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleUI01UnitInformationOption informationOption = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (informationOption.weapons_ == null || informationOption.weapons_.Length == 0)
      return false;
    int index = 0;
    if (!informationOption.unit_.crippled)
    {
      foreach (IAttackMethod battleOptionAttack in informationOption.unit_.playerUnit.battleOptionAttacks)
      {
        if (informationOption.weapons_.Length > index)
          informationOption.main_.initializeOptionAttack(informationOption.weapons_[index++], battleOptionAttack);
        else
          break;
      }
    }
    for (; index < informationOption.weapons_.Length; ++index)
      informationOption.weapons_[index].top_.SetActive(false);
    return false;
  }

  public override BattleUI01UnitInformationTab.Type type
  {
    get => BattleUI01UnitInformationTab.Type.Option;
  }
}
