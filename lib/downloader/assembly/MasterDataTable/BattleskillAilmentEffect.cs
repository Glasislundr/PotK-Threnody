// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleskillAilmentEffect
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
  public class BattleskillAilmentEffect
  {
    public int ID;
    public string field_effect_name;
    public string duel_effect_name;

    public static BattleskillAilmentEffect Parse(MasterDataReader reader)
    {
      return new BattleskillAilmentEffect()
      {
        ID = reader.ReadInt(),
        field_effect_name = reader.ReadString(true),
        duel_effect_name = reader.ReadString(true)
      };
    }

    public Future<GameObject> LoadFieldAilmentEffectPrefab()
    {
      string path = string.Format("BattleEffects/field/{0}", (object) this.field_effect_name);
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(path);
    }
  }
}
