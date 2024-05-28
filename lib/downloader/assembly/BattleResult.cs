// Decompiled with JetBrains decompiler
// Type: BattleResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleResult : MonoBehaviour
{
  [SerializeField]
  private GameObject[] countOwn;
  [SerializeField]
  private GameObject[] countEnemy;
  [SerializeField]
  private UILabel damageOwn;
  [SerializeField]
  private UILabel damageEnemy;
  [SerializeField]
  private UIButton btnReplay;

  public void Initialize(Action replayCallBack)
  {
    ((Component) this.damageOwn).gameObject.SetActive(true);
    ((Component) this.damageEnemy).gameObject.SetActive(true);
    ((IEnumerable<GameObject>) this.countOwn).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    ((IEnumerable<GameObject>) this.countEnemy).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    this.dispReplay(false);
    EventDelegate.Set(this.btnReplay.onClick, (EventDelegate.Callback) (() => replayCallBack()));
  }

  public void SetBattleResult(GameCore.ColosseumResult result)
  {
    this.DispPoint(result);
    this.SetTotalDamage(result);
  }

  private void DispPoint(GameCore.ColosseumResult result)
  {
    foreach (var data in ((IEnumerable<GameObject>) this.countOwn).Select((s, i) => new
    {
      s = s,
      i = i
    }))
      data.s.SetActive(data.i == result.win_count);
    foreach (var data in ((IEnumerable<GameObject>) this.countEnemy).Select((s, i) => new
    {
      s = s,
      i = i
    }))
      data.s.SetActive(data.i == result.lose_count);
  }

  private void SetTotalDamage(GameCore.ColosseumResult result)
  {
    this.damageOwn.SetTextLocalize(result.myTotalDamage);
    this.damageEnemy.SetTextLocalize(result.opponetTotalDamage);
  }

  public void dispReplay(bool canDisp)
  {
    ((Component) this.btnReplay).gameObject.SetActive(canDisp);
  }
}
