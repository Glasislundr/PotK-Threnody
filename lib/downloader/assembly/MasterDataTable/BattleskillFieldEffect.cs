// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleskillFieldEffect
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
  public class BattleskillFieldEffect
  {
    public int ID;
    public string user_name;
    public bool user_move_camera;
    public bool user_effect_mask;
    public float user_wait_seconds;
    public string target_name;
    public bool target_move_camera;
    public bool target_effect_mask;
    public float target_wait_seconds;
    public bool targets_multiple_effect;
    public string invoked_effect_name;
    public bool invoked_move_camera;
    public bool invoked_effect_mask;
    public float invoked_wait_seconds;
    public bool invoked_effect_target;

    public static BattleskillFieldEffect Parse(MasterDataReader reader)
    {
      return new BattleskillFieldEffect()
      {
        ID = reader.ReadInt(),
        user_name = reader.ReadString(true),
        user_move_camera = reader.ReadBool(),
        user_effect_mask = reader.ReadBool(),
        user_wait_seconds = reader.ReadFloat(),
        target_name = reader.ReadString(true),
        target_move_camera = reader.ReadBool(),
        target_effect_mask = reader.ReadBool(),
        target_wait_seconds = reader.ReadFloat(),
        targets_multiple_effect = reader.ReadBool(),
        invoked_effect_name = reader.ReadStringOrNull(true),
        invoked_move_camera = reader.ReadBool(),
        invoked_effect_mask = reader.ReadBool(),
        invoked_wait_seconds = reader.ReadFloat(),
        invoked_effect_target = reader.ReadBool()
      };
    }

    public Future<GameObject> LoadFieldEffectPrefab()
    {
      string path = string.Format("BattleEffects/field/{0}", (object) this.user_name);
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(path);
    }

    public Future<GameObject> LoadFieldTargetEffectPrefab()
    {
      string path = string.Format("BattleEffects/field/{0}", (object) this.target_name);
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(path);
    }

    public Future<GameObject> LoadFieldInvokedEffectPrefab()
    {
      if (string.IsNullOrEmpty(this.invoked_effect_name))
        return Future.Single<GameObject>((GameObject) null);
      string path = string.Format("BattleEffects/field/{0}", (object) this.invoked_effect_name);
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(path);
    }
  }
}
