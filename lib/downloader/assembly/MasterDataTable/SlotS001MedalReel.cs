// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SlotS001MedalReel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SlotS001MedalReel
  {
    public int ID;
    public int reel_id;
    public int reel_detail_id;

    public static SlotS001MedalReel Parse(MasterDataReader reader)
    {
      return new SlotS001MedalReel()
      {
        ID = reader.ReadInt(),
        reel_id = reader.ReadInt(),
        reel_detail_id = reader.ReadInt()
      };
    }
  }
}
