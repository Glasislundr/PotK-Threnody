// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitReferenceImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitReferenceImage
  {
    public int ID;
    public string form;

    public static UnitReferenceImage Parse(MasterDataReader reader)
    {
      return new UnitReferenceImage()
      {
        ID = reader.ReadInt(),
        form = reader.ReadString(true)
      };
    }

    public string filePath(UnitUnit unit)
    {
      return Consts.Format(this.form, (IDictionary) new Hashtable()
      {
        {
          (object) nameof (unit),
          (object) unit.ID
        },
        {
          (object) "ref",
          (object) unit.resource_reference_unit_id_UnitUnit
        }
      });
    }
  }
}
