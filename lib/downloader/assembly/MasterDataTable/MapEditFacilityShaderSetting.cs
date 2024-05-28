// Decompiled with JetBrains decompiler
// Type: MasterDataTable.MapEditFacilityShaderSetting
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
  public class MapEditFacilityShaderSetting
  {
    public int ID;
    public int unit_UnitUnit;
    public string moving;
    public string installed;

    public static MapEditFacilityShaderSetting Parse(MasterDataReader reader)
    {
      return new MapEditFacilityShaderSetting()
      {
        ID = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        moving = reader.ReadStringOrNull(true),
        installed = reader.ReadStringOrNull(true)
      };
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }

    public bool hasMovingMaterial => !string.IsNullOrEmpty(this.moving);

    public Future<Material> LoadMovingMaterial()
    {
      return MapEditFacilityShaderSetting.LoadMaterial(this.moving);
    }

    public bool hasInstalledMaterial => !string.IsNullOrEmpty(this.installed);

    public Future<Material> LoadInstalledMaterial()
    {
      return MapEditFacilityShaderSetting.LoadMaterial(this.installed);
    }

    public static Future<Material> LoadMaterial(string materialName)
    {
      if (string.IsNullOrEmpty(materialName))
        return Future.Single<Material>((Material) null);
      string path = string.Format("Units/SharedFacility/{0}", (object) materialName);
      return Singleton<ResourceManager>.GetInstance().Load<Material>(path);
    }
  }
}
