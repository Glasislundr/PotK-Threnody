// Decompiled with JetBrains decompiler
// Type: MasterDataTable.Music
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class Music
  {
    public int ID;
    public string music_name;
    public string composer;
    public string arranger;
    public string bgm_file;
    public string bgm_name;
    public bool is_arrange;
    public int music_tab_id;
    public string purchase_site_link;

    public static Music Parse(MasterDataReader reader)
    {
      return new Music()
      {
        ID = reader.ReadInt(),
        music_name = reader.ReadString(true),
        composer = reader.ReadString(true),
        arranger = reader.ReadString(true),
        bgm_file = reader.ReadString(true),
        bgm_name = reader.ReadString(true),
        is_arrange = reader.ReadBool(),
        music_tab_id = reader.ReadInt(),
        purchase_site_link = reader.ReadString(true)
      };
    }
  }
}
