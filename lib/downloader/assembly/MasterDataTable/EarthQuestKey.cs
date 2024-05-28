// Decompiled with JetBrains decompiler
// Type: MasterDataTable.EarthQuestKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class EarthQuestKey
  {
    public int ID;
    public string name;
    public int max_stack;
    public int quest_id;

    public static EarthQuestKey Parse(MasterDataReader reader)
    {
      return new EarthQuestKey()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        max_stack = reader.ReadInt(),
        quest_id = reader.ReadInt()
      };
    }

    public Future<Sprite> LoadSpriteThumbnail()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/EarthQuestkey/{0}/key_thum", (object) this.ID));
    }

    public Future<Sprite> LoadSpriteBasic()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/EarthQuestkey/{0}/key_basic", (object) this.ID));
    }
  }
}
