// Decompiled with JetBrains decompiler
// Type: BattleUI01UnitInformationWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI01UnitInformationWeapon : BattleUI01UnitInformationTab
{
  [SerializeField]
  [Tooltip("武器表示")]
  private BattleUI01UnitInformation.WeaponRow[] weapons_;

  public override IEnumerator initialize()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleUI01UnitInformationWeapon informationWeapon = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (informationWeapon.weapons_ == null || informationWeapon.weapons_.Length == 0)
      return false;
    int index = 0;
    BL.Unit unit = informationWeapon.unit_;
    GearGear gear1 = unit.weapon.gear;
    if (gear1 != null)
      informationWeapon.main_.initializeWeapon(informationWeapon.weapons_[index++], gear1);
    PlayerItem equippedGear2 = unit.playerUnit.equippedGear2;
    GearGear gear2;
    if (equippedGear2 != (PlayerItem) null && (gear2 = equippedGear2.gear) != null && gear2.kind.is_attack && informationWeapon.weapons_.Length > index)
      informationWeapon.main_.initializeWeapon(informationWeapon.weapons_[index++], gear2);
    if (unit.playerUnit.skills != null && unit.playerUnit.magicSkills.Length != 0)
    {
      foreach (PlayerUnitSkills magicSkill in unit.playerUnit.magicSkills)
      {
        if (informationWeapon.weapons_.Length > index)
          informationWeapon.main_.initializeMagic(informationWeapon.weapons_[index++], magicSkill);
        else
          break;
      }
    }
    for (; index < informationWeapon.weapons_.Length; ++index)
      informationWeapon.weapons_[index].top_.SetActive(false);
    return false;
  }

  public override BattleUI01UnitInformationTab.Type type
  {
    get => BattleUI01UnitInformationTab.Type.Weapon;
  }
}
