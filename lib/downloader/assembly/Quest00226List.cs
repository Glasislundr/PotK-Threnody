// Decompiled with JetBrains decompiler
// Type: Quest00226List
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00226List : Quest00219List
{
  [SerializeField]
  private UILabel TxtTotalPoint;

  public IEnumerator Init(PlayerExtraQuestS extra, bool isClear, bool isNew, int totalPoint)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Quest00226List quest00226List = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    quest00226List.Name.SetTextLocalize(MasterData.QuestExtraM[extra.quest_extra_s.quest_m_QuestExtraM].name);
    quest00226List.TxtTotalPoint.SetTextLocalize(totalPoint);
    return false;
  }
}
