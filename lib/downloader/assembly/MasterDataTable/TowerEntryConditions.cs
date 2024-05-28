// Decompiled with JetBrains decompiler
// Type: MasterDataTable.TowerEntryConditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class TowerEntryConditions
  {
    public int ID;
    public int tower_id_TowerTower;
    public int value;
    public string text;

    public static TowerEntryConditions Parse(MasterDataReader reader)
    {
      return new TowerEntryConditions()
      {
        ID = reader.ReadInt(),
        tower_id_TowerTower = reader.ReadInt(),
        value = reader.ReadInt(),
        text = reader.ReadString(true)
      };
    }

    public TowerTower tower_id
    {
      get
      {
        TowerTower towerId;
        if (!MasterData.TowerTower.TryGetValue(this.tower_id_TowerTower, out towerId))
          Debug.LogError((object) ("Key not Found: MasterData.TowerTower[" + (object) this.tower_id_TowerTower + "]"));
        return towerId;
      }
    }
  }
}
