// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollViewInfoWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class DetailMenuScrollViewInfoWeapon : DetailMenuScrollViewBase
{
  [SerializeField]
  private DetailMenuScrollViewInfo menu_;
  [SerializeField]
  [Tooltip("武器表示")]
  private DetailMenuScrollViewInfo.WeaponRow[] weapons_;
  private PlayerUnit playerUnit;

  public override IEnumerator initAsync(
    PlayerUnit playerUnit,
    bool limitMode,
    bool isMaterial,
    GameObject[] prefabs)
  {
    if (this.weapons_ != null && this.weapons_.Length != 0)
    {
      int index1 = 0;
      GearGear equippedGearOrInitial = playerUnit.equippedGearOrInitial;
      GearGear ag = playerUnit.equippedAssistGear;
      UnitUnit u = playerUnit.unit;
      IEnumerator e;
      if (equippedGearOrInitial != null)
      {
        e = this.menu_.initializeWeapon(this.weapons_[index1++], equippedGearOrInitial, ag, u.awake_unit_flag);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      PlayerItem equippedGear2 = playerUnit.equippedGear2;
      GearGear gear;
      if (equippedGear2 != (PlayerItem) null && (gear = equippedGear2.gear) != null && gear.kind.is_attack && this.weapons_.Length > index1)
      {
        e = this.menu_.initializeWeapon(this.weapons_[index1++], gear, ag, u.awake_unit_flag);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      if (playerUnit.skills != null && playerUnit.magicSkills.Length != 0)
      {
        PlayerUnitSkills[] playerUnitSkillsArray = playerUnit.magicSkills;
        for (int index2 = 0; index2 < playerUnitSkillsArray.Length; ++index2)
        {
          PlayerUnitSkills magic = playerUnitSkillsArray[index2];
          if (this.weapons_.Length > index1)
          {
            e = this.menu_.initializeMagic(this.weapons_[index1++], magic);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          else
            break;
        }
        playerUnitSkillsArray = (PlayerUnitSkills[]) null;
      }
      for (; index1 < this.weapons_.Length; ++index1)
        this.weapons_[index1].top_.SetActive(false);
    }
  }
}
