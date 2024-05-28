// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitTrustUpperLimitEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitTrustUpperLimitEffect
  {
    public int ID;
    public int same_character_id;
    public string face;
    public string serif;
    public string voice;
    public string background;

    public static UnitTrustUpperLimitEffect Parse(MasterDataReader reader)
    {
      return new UnitTrustUpperLimitEffect()
      {
        ID = reader.ReadInt(),
        same_character_id = reader.ReadInt(),
        face = reader.ReadString(true),
        serif = reader.ReadString(true),
        voice = reader.ReadString(true),
        background = reader.ReadString(true)
      };
    }

    public string[] GetSerif() => this.serif.Split(',');

    public string[] GetVoice() => this.voice.Split(',');
  }
}
