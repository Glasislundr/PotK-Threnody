// Decompiled with JetBrains decompiler
// Type: Popup05020Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup05020Menu : BackButtonMenuBase
{
  public override void onBackButton() => this.IbtnNo();

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowResetPopup());
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (Persist.earthBattleEnvironment.Data.core.battleInfo.quest_type == CommonQuestType.EarthExtra)
    {
      EarthExtraQuest quest = ((IEnumerable<EarthExtraQuest>) MasterData.EarthExtraQuestList).FirstOrDefault<EarthExtraQuest>((Func<EarthExtraQuest, bool>) (x => x.ID == Persist.earthBattleEnvironment.Data.core.battleInfo.quest_s_id));
      this.StartCoroutine(Singleton<EarthDataManager>.GetInstance().BattleInitExtra(quest));
    }
    else
      this.StartCoroutine(Singleton<EarthDataManager>.GetInstance().BattleInitStory());
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  private IEnumerator ShowResetPopup()
  {
    Popup05020Menu popup05020Menu = this;
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_050_21__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result.Clone();
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true).GetComponent<Popup05021Menu>().Init(new Action(popup05020Menu.cbPushOff));
  }

  public void cbPushOff() => this.StartCoroutine(this.IsPushOff());
}
