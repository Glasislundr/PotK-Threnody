// Decompiled with JetBrains decompiler
// Type: BattleUI55Continue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI55Continue : ResultMenuBase
{
  public bool isReset;
  private bool isShowResetPopup;
  private GameObject popup1;
  private GameObject popup2;

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    Future<GameObject> popup1F = Res.Prefabs.popup.popup_070_1__anim_popup01.Load<GameObject>();
    IEnumerator e = popup1F.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.popup1 = popup1F.Result;
    Future<GameObject> popup2F = Res.Prefabs.popup.popup_070_2__anim_popup01.Load<GameObject>();
    e = popup2F.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.popup2 = popup2F.Result;
    this.isShowResetPopup = info.quest_type == CommonQuestType.Earth;
  }

  public override IEnumerator Run()
  {
    BattleUI55Continue battleUi55Continue = this;
    bool isFinish = false;
    battleUi55Continue.isReset = false;
    if (battleUi55Continue.isShowResetPopup)
    {
      Singleton<PopupManager>.GetInstance().open(battleUi55Continue.popup1).GetComponent<Popup0701Menu>().Init((Action<bool>) (reset =>
      {
        isFinish = true;
        this.isReset = reset;
      }), battleUi55Continue.popup2);
      while (!isFinish)
        yield return (object) null;
    }
    if (!battleUi55Continue.isReset)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      IEnumerator e = Singleton<EarthDataManager>.GetInstance().SaveAndSendServer();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      ((Component) battleUi55Continue).GetComponent<BattleUI55Scene>().IbtnTouchToNext();
    }
  }
}
