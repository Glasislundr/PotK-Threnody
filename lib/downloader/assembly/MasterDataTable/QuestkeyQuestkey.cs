// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestkeyQuestkey
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
  public class QuestkeyQuestkey
  {
    public int ID;
    public string name;
    public string description;
    public int priority;

    public static QuestkeyQuestkey Parse(MasterDataReader reader)
    {
      return new QuestkeyQuestkey()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        priority = reader.ReadInt()
      };
    }

    public Future<Sprite> LoadSpriteThumbnail()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Questkey/{0}/key_thum", (object) this.ID));
    }

    public Future<Sprite> LoadSpriteBasic()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Questkey/{0}/key_basic", (object) this.ID));
    }
  }
}
