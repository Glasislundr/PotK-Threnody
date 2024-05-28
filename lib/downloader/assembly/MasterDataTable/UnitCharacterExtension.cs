// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitCharacterExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitCharacterExtension
  {
    public int ID;
    public int unit_id_UnitUnit;
    public int job_metamor_id;
    public string name;
    public string formal_name;

    public static UnitCharacterExtension Parse(MasterDataReader reader)
    {
      return new UnitCharacterExtension()
      {
        ID = reader.ReadInt(),
        unit_id_UnitUnit = reader.ReadInt(),
        job_metamor_id = reader.ReadInt(),
        name = reader.ReadString(true),
        formal_name = reader.ReadStringOrNull(true)
      };
    }

    public UnitUnit unit_id
    {
      get
      {
        UnitUnit unitId;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_id_UnitUnit, out unitId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_id_UnitUnit + "]"));
        return unitId;
      }
    }
  }
}
