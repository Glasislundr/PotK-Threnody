// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestkeyCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestkeyCondition
  {
    public int ID;
    public int gate_id;
    public string contents;

    public static QuestkeyCondition Parse(MasterDataReader reader)
    {
      return new QuestkeyCondition()
      {
        ID = reader.ReadInt(),
        gate_id = reader.ReadInt(),
        contents = reader.ReadString(true)
      };
    }
  }
}
