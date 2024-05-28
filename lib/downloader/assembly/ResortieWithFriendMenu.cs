// Decompiled with JetBrains decompiler
// Type: ResortieWithFriendMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ResortieWithFriendMenu : ResultMenuBase
{
  [SerializeField]
  private GameObject mainPanel_;
  [SerializeField]
  private float fadeTime_ = 1f;
  private int friendPoint_;
  private PlayerHelper helper_;
  private bool isFriend_;
  private bool isRunning_;
  private object quest_;

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    this.isRunning_ = false;
    switch (info.quest_type)
    {
      case CommonQuestType.Story:
        this.quest_ = (object) info.storyQuest;
        break;
      case CommonQuestType.Character:
        this.quest_ = (object) info.charaQuest;
        break;
      case CommonQuestType.Extra:
        this.quest_ = (object) info.extraQuest;
        break;
      case CommonQuestType.Harmony:
        this.quest_ = (object) info.harmonyQuest;
        break;
      case CommonQuestType.Sea:
        this.quest_ = (object) info.seaQuest;
        break;
      default:
        this.quest_ = (object) null;
        break;
    }
    this.helper_ = result.battle_helpers.Length != 0 ? ((IEnumerable<PlayerHelper>) result.battle_helpers).First<PlayerHelper>() : (PlayerHelper) null;
    if (this.helper_ != null)
    {
      this.isFriend_ = this.helper_.is_friend;
      this.friendPoint_ = result.incr_friend_point;
      yield break;
    }
  }

  public override IEnumerator Run()
  {
    EventDelegate.Add(((UITweener) TweenAlpha.Begin(this.mainPanel_, this.fadeTime_, 0.0f)).onFinished, (EventDelegate.Callback) (() => this.mainPanel_.SetActive(false)), true);
    bool toNext = false;
    this.isRunning_ = true;
    IEnumerator e = BattleUI05PopupResortie.show(this.quest_, this.helper_, this.friendPoint_, this.isFriend_, (Action) (() => toNext = true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.isRunning_ = false;
    while (!toNext)
      yield return (object) null;
  }

  public override IEnumerator OnFinish()
  {
    yield break;
  }

  public override void OnRemove()
  {
    if (!this.isRunning_)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
  }
}
