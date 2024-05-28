// Decompiled with JetBrains decompiler
// Type: MasterDataTable.OverkillersMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class OverkillersMaterial
  {
    public int ID;
    public int materials_no;
    public int? materials_set;
    public int? material_UnitUnit;
    public int quantity;

    public static OverkillersMaterial Parse(MasterDataReader reader)
    {
      return new OverkillersMaterial()
      {
        ID = reader.ReadInt(),
        materials_no = reader.ReadInt(),
        materials_set = reader.ReadIntOrNull(),
        material_UnitUnit = reader.ReadIntOrNull(),
        quantity = reader.ReadInt()
      };
    }

    public UnitUnit material
    {
      get
      {
        if (!this.material_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit material;
        if (!MasterData.UnitUnit.TryGetValue(this.material_UnitUnit.Value, out material))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.material_UnitUnit.Value + "]"));
        return material;
      }
    }

    public static OverkillersMaterial[] getMaterials(int materialsNo)
    {
      if (materialsNo == 0)
        return new OverkillersMaterial[0];
      HashSet<OverkillersMaterial> source = new HashSet<OverkillersMaterial>();
      foreach (OverkillersMaterial overkillersMaterial1 in MasterData.OverkillersMaterialList)
      {
        if (overkillersMaterial1.materials_no == materialsNo)
        {
          if (overkillersMaterial1.material != null)
            source.Add(overkillersMaterial1);
          else if (overkillersMaterial1.materials_set.HasValue)
          {
            int num = overkillersMaterial1.materials_set.Value;
            foreach (OverkillersMaterial overkillersMaterial2 in MasterData.OverkillersMaterialList)
            {
              if (overkillersMaterial2.materials_no == num)
              {
                if (overkillersMaterial2.material != null)
                  source.Add(overkillersMaterial2);
                else
                  OverkillersMaterial.errorLog(overkillersMaterial2);
              }
            }
          }
          else
            OverkillersMaterial.errorLog(overkillersMaterial1);
        }
      }
      return source.ToArray<OverkillersMaterial>();
    }

    private static void errorLog(OverkillersMaterial dat)
    {
      Debug.LogError((object) ("Bad Data \"OverkillersMaterial{ID=" + dat.ID.ToString() + ", materials_no=" + dat.materials_no.ToString() + ", materials_set=" + (dat.materials_set.HasValue ? dat.materials_set.Value.ToString() : "null") + ", material=" + (dat.material_UnitUnit.HasValue ? dat.material_UnitUnit.Value.ToString() : "null") + "}\""));
    }
  }
}
