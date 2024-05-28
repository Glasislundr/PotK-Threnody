// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CallMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CallMessage
  {
    public int ID;
    public int message_set_id;
    public int message_type_id;
    public int view_order;
    public int branch_group_id;
    public int view_wait_time;
    public string text;

    public static CallMessage Parse(MasterDataReader reader)
    {
      return new CallMessage()
      {
        ID = reader.ReadInt(),
        message_set_id = reader.ReadInt(),
        message_type_id = reader.ReadInt(),
        view_order = reader.ReadInt(),
        branch_group_id = reader.ReadInt(),
        view_wait_time = reader.ReadInt(),
        text = reader.ReadString(true)
      };
    }
  }
}
