// Decompiled with JetBrains decompiler
// Type: SM.QuestScoreCampaignDescriptionBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestScoreCampaignDescriptionBlock : KeyCompare
  {
    public QuestScoreCampaignDescriptionBlockBodies[] bodies;
    public string title;

    public QuestScoreCampaignDescriptionBlock()
    {
    }

    public QuestScoreCampaignDescriptionBlock(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<QuestScoreCampaignDescriptionBlockBodies> descriptionBlockBodiesList = new List<QuestScoreCampaignDescriptionBlockBodies>();
      foreach (object json1 in (List<object>) json[nameof (bodies)])
        descriptionBlockBodiesList.Add(json1 == null ? (QuestScoreCampaignDescriptionBlockBodies) null : new QuestScoreCampaignDescriptionBlockBodies((Dictionary<string, object>) json1));
      this.bodies = descriptionBlockBodiesList.ToArray();
      this.title = json[nameof (title)] == null ? (string) null : (string) json[nameof (title)];
    }
  }
}
