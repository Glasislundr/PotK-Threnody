// Decompiled with JetBrains decompiler
// Type: Quest002171CollaboOpenPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest002171CollaboOpenPopup : Quest002171QuestOpenPopup
{
  [SerializeField]
  private UILabel m_TextCondition;

  public new IEnumerator Init(PlayerQuestGate[] gates, Quest002171Scroll scrollcomp)
  {
    Quest002171CollaboOpenPopup collaboOpenPopup = this;
    KeyValuePair<int, QuestkeyCondition> keyValuePair = MasterData.QuestkeyCondition.FirstOrDefault<KeyValuePair<int, QuestkeyCondition>>((Func<KeyValuePair<int, QuestkeyCondition>, bool>) (fd => fd.Value.gate_id == gates[0].quest_gate_id));
    collaboOpenPopup.m_TextCondition.SetTextLocalize(keyValuePair.Value.contents);
    IEnumerator e = ((Quest002171QuestOpenPopup) collaboOpenPopup).Init(gates, scrollcomp);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
