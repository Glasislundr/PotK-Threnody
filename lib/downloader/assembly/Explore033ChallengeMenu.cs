// Decompiled with JetBrains decompiler
// Type: Explore033ChallengeMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Explore033ChallengeMenu : Coloseum02342Menu
{
  [SerializeField]
  private Explore033ChallengeScene sceneChallange;

  public IEnumerator Initialize(GameCore.ColosseumResult result, Gladiator gladiator, int duelCount)
  {
    Explore033ChallengeMenu explore033ChallengeMenu = this;
    explore033ChallengeMenu.SetColosseumData(result, gladiator);
    explore033ChallengeMenu.battleResultAnimPath = "Prefabs/explore033_Challenge/explore_BattleResults";
    IEnumerator e = explore033ChallengeMenu.LoadResource(duelCount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    explore033ChallengeMenu.initialized = true;
  }

  public override void IbtnStartBattle()
  {
    this.roundButton.SetActive(false);
    this.SkipPermission = false;
    this.BtnSkip.SetActive(false);
    this.SM.playSE("SE_1030");
    if (this.player == (BL.Unit) null || this.opponent == (BL.Unit) null || this.isSkip)
      this.sceneChallange.Reinitialize();
    else
      this.StartCoroutine(this.ChangeDuelScene());
  }

  public override void GoToResult()
  {
    Explore033ChallengeResultScene.ChangeScene(this.result, this.gladiator);
  }

  public override void Replay()
  {
    this.isSkip = false;
    this.BtnSkip.SetActive(true);
    this.ResetUnitIcons(this.playerUnitIconC);
    this.ResetUnitIcons(this.enemyUnitIconC);
    ((Component) this.resultAnimations[0]).gameObject.SetActive(false);
    ((Component) this.resultAnimations[1]).gameObject.SetActive(false);
    ((Component) this.ibtnResult).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) this.battleResults, (Object) null))
    {
      this.battleResults.dispReplay(false);
      this.dirResult.gameObject.SetActive(false);
    }
    this.sceneChallange.ReplayScene();
  }
}
