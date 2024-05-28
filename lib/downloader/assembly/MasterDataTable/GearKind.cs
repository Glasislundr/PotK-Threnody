// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearKind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearKind
  {
    public int ID;
    public string name;
    public int can_equip;
    public int same_element;
    public bool is_attack;
    public bool is_composite;
    public int colosseum_preempt_coefficient;

    public GearKindEnum Enum => (GearKindEnum) this.ID;

    public bool isEquip => this.can_equip != 0;

    public bool isNonWeapon => !this.isEquip || !this.is_attack;

    public bool isAssist
    {
      get
      {
        switch (this.ID)
        {
          case 7:
          case 10:
            return true;
          default:
            return false;
        }
      }
    }

    public static GearKind Parse(MasterDataReader reader)
    {
      return new GearKind()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        can_equip = reader.ReadInt(),
        same_element = reader.ReadInt(),
        is_attack = reader.ReadBool(),
        is_composite = reader.ReadBool(),
        colosseum_preempt_coefficient = reader.ReadInt()
      };
    }

    public bool isHideProficiency
    {
      get
      {
        switch (this.Enum)
        {
          case GearKindEnum.unique_wepon:
          case GearKindEnum.accessories:
            return true;
          default:
            return false;
        }
      }
    }

    public Future<GameObject> LoadFieldWeaponModel()
    {
      string path = string.Format("GearKinds/{0}/3D/field_weapon/prefab", (object) this.ID);
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(path);
    }
  }
}
