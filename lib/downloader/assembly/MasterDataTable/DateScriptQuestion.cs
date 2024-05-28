// Decompiled with JetBrains decompiler
// Type: MasterDataTable.DateScriptQuestion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class DateScriptQuestion
  {
    public int ID;
    public int unitID;
    public int questionID;
    public string script;

    public static DateScriptQuestion Parse(MasterDataReader reader)
    {
      return new DateScriptQuestion()
      {
        ID = reader.ReadInt(),
        unitID = reader.ReadInt(),
        questionID = reader.ReadInt(),
        script = reader.ReadString(true)
      };
    }
  }
}
