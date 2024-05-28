// Decompiled with JetBrains decompiler
// Type: MasterDataTable.JobMaterialGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class JobMaterialGroup
  {
    public int ID;
    public int? check_item_id_JobCheckItem;

    public static JobMaterialGroup Parse(MasterDataReader reader)
    {
      return new JobMaterialGroup()
      {
        ID = reader.ReadInt(),
        check_item_id_JobCheckItem = reader.ReadIntOrNull()
      };
    }

    public JobCheckItem? check_item_id
    {
      get
      {
        int? itemIdJobCheckItem = this.check_item_id_JobCheckItem;
        return !itemIdJobCheckItem.HasValue ? new JobCheckItem?() : new JobCheckItem?((JobCheckItem) itemIdJobCheckItem.GetValueOrDefault());
      }
    }
  }
}
