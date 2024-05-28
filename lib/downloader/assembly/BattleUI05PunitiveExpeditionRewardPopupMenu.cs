// Decompiled with JetBrains decompiler
// Type: BattleUI05PunitiveExpeditionRewardPopupMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI05PunitiveExpeditionRewardPopupMenu : ResultMenuBase
{
  private GameObject HunitingRewardGetPrefab;

  public override IEnumerator Init(BattleInfo info, BattleEnd result, int index)
  {
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.HunitingRewardGetPrefab, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.battle.Huniting_RewardGet.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.HunitingRewardGetPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  public override IEnumerator Run()
  {
    GameObject popup = this.HunitingRewardGetPrefab.Clone();
    BattleUI05PunitiveExpeditionRewardPopup script = popup.GetComponent<BattleUI05PunitiveExpeditionRewardPopup>();
    IEnumerator e = script.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    bool toNext = false;
    script.SetTapCallBack((Action) (() =>
    {
      if (this.IsPush)
        return;
      toNext = true;
    }));
    while (!toNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        script.TapFinish();
      }
      yield return (object) null;
    }
    Singleton<PopupManager>.GetInstance().onDismiss();
    yield return (object) new WaitForSeconds(0.5f);
  }

  public override IEnumerator OnFinish()
  {
    yield break;
  }
}
