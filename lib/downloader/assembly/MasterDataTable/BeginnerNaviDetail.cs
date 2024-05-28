// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BeginnerNaviDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BeginnerNaviDetail
  {
    public int ID;
    public int title_BeginnerNaviTitle;
    public string questionText;
    public string answerText;
    public string descriptionImage;
    public int movePage_BeginnerNaviMovePage;
    public int frameNumber;

    public static BeginnerNaviDetail Parse(MasterDataReader reader)
    {
      return new BeginnerNaviDetail()
      {
        ID = reader.ReadInt(),
        title_BeginnerNaviTitle = reader.ReadInt(),
        questionText = reader.ReadString(true),
        answerText = reader.ReadString(true),
        descriptionImage = reader.ReadString(true),
        movePage_BeginnerNaviMovePage = reader.ReadInt(),
        frameNumber = reader.ReadInt()
      };
    }

    public BeginnerNaviTitle title
    {
      get
      {
        BeginnerNaviTitle title;
        if (!MasterData.BeginnerNaviTitle.TryGetValue(this.title_BeginnerNaviTitle, out title))
          Debug.LogError((object) ("Key not Found: MasterData.BeginnerNaviTitle[" + (object) this.title_BeginnerNaviTitle + "]"));
        return title;
      }
    }

    public BeginnerNaviMovePage movePage
    {
      get
      {
        BeginnerNaviMovePage movePage;
        if (!MasterData.BeginnerNaviMovePage.TryGetValue(this.movePage_BeginnerNaviMovePage, out movePage))
          Debug.LogError((object) ("Key not Found: MasterData.BeginnerNaviMovePage[" + (object) this.movePage_BeginnerNaviMovePage + "]"));
        return movePage;
      }
    }
  }
}
