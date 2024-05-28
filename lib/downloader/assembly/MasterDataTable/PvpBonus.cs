// Decompiled with JetBrains decompiler
// Type: MasterDataTable.PvpBonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class PvpBonus
  {
    public int ID;
    public string disp_text;

    public static PvpBonus Parse(MasterDataReader reader)
    {
      return new PvpBonus()
      {
        ID = reader.ReadInt(),
        disp_text = reader.ReadString(true)
      };
    }
  }
}
