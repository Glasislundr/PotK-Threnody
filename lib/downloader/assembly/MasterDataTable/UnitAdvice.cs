// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitAdvice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitAdvice
  {
    public int ID;
    public int role_UnitRole;
    public int activity_UnitActivityScenes;
    public string advice;

    public static UnitAdvice Parse(MasterDataReader reader)
    {
      return new UnitAdvice()
      {
        ID = reader.ReadInt(),
        role_UnitRole = reader.ReadInt(),
        activity_UnitActivityScenes = reader.ReadInt(),
        advice = reader.ReadString(true)
      };
    }

    public UnitRole role
    {
      get
      {
        UnitRole role;
        if (!MasterData.UnitRole.TryGetValue(this.role_UnitRole, out role))
          Debug.LogError((object) ("Key not Found: MasterData.UnitRole[" + (object) this.role_UnitRole + "]"));
        return role;
      }
    }

    public UnitActivityScenes activity
    {
      get
      {
        UnitActivityScenes activity;
        if (!MasterData.UnitActivityScenes.TryGetValue(this.activity_UnitActivityScenes, out activity))
          Debug.LogError((object) ("Key not Found: MasterData.UnitActivityScenes[" + (object) this.activity_UnitActivityScenes + "]"));
        return activity;
      }
    }

    public int same_character_id => this.ID;
  }
}
