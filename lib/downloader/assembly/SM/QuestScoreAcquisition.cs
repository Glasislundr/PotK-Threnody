// Decompiled with JetBrains decompiler
// Type: SM.QuestScoreAcquisition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestScoreAcquisition : KeyCompare
  {
    public int score;
    public string description;
    public int id;

    public QuestScoreAcquisition()
    {
    }

    public QuestScoreAcquisition(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.score = (int) (long) json[nameof (score)];
      this.description = (string) json[nameof (description)];
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
