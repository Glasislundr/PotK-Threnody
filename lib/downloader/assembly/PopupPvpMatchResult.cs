// Decompiled with JetBrains decompiler
// Type: PopupPvpMatchResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PopupPvpMatchResult : NGBattleMenuBase
{
  [SerializeField]
  private GameObject[] effects;
  [SerializeField]
  private GameObject[] effects_win;
  [SerializeField]
  private UI2DSprite linkObj;
  [SerializeField]
  private GameObject effectParent;
  private Sprite sprite;
  private AppPeer.FinishBattle result;
  private int order;

  protected override IEnumerator Start_Battle()
  {
    PopupPvpMatchResult popupPvpMatchResult = this;
    popupPvpMatchResult.effectParent.SetActive(false);
    Future<Sprite> spriteF = MasterData.UnitUnit[popupPvpMatchResult.env.core.playerUnits.value[0].unitId].LoadSpriteLarge(popupPvpMatchResult.env.core.playerUnits.value[0].playerUnit.job_id, 1f);
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popupPvpMatchResult.sprite = spriteF.Result;
    if (popupPvpMatchResult.result != null)
      popupPvpMatchResult.setResultBase();
  }

  public IEnumerator Start_Battle_Debug()
  {
    this.effectParent.SetActive(true);
    Future<Sprite> spriteF = MasterData.UnitUnit[100113].LoadSpriteLarge();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.sprite = spriteF.Result;
  }

  public void setResult(AppPeer.FinishBattle f, int p)
  {
    this.result = f;
    this.order = p;
    if (!Object.op_Inequality((Object) this.sprite, (Object) null))
      return;
    this.setResultBase();
  }

  private void setResultBase()
  {
    PvpVictoryEffectEnum victoryEffect = this.result.victoryEffects[this.order];
    int num;
    switch (victoryEffect)
    {
      case PvpVictoryEffectEnum.lose_effect:
        num = 1;
        break;
      case PvpVictoryEffectEnum.draw_effect:
        num = 2;
        break;
      default:
        num = 0;
        break;
    }
    int n = num;
    if (n < this.effects.Length)
      ((IEnumerable<GameObject>) this.effects).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(Object.op_Equality((Object) x, (Object) this.effects[n]))));
    if (victoryEffect == PvpVictoryEffectEnum.great_effect || victoryEffect == PvpVictoryEffectEnum.excellent_effect)
    {
      int m = (int) (victoryEffect - 1);
      ((IEnumerable<GameObject>) this.effects_win).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(Object.op_Equality((Object) x, (Object) this.effects_win[m]))));
    }
    else
      ((IEnumerable<GameObject>) this.effects_win).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    this.linkObj.sprite2D = this.sprite;
    ((UIWidget) this.linkObj).color = Color.Lerp(Color.white, Color.gray, n == 0 ? 0.0f : 1f);
    ((Component) this.linkObj).GetComponent<NGxMaskSpriteWithScale>().FitMask();
    this.effectParent.SetActive(true);
    switch (victoryEffect)
    {
      case PvpVictoryEffectEnum.excellent_effect:
      case PvpVictoryEffectEnum.great_effect:
      case PvpVictoryEffectEnum.win_effect:
        Singleton<NGSoundManager>.GetInstance().playVoiceByID(this.env.core.playerUnits.value[0].unit.unitVoicePattern, 71, 0);
        break;
      case PvpVictoryEffectEnum.lose_effect:
      case PvpVictoryEffectEnum.draw_effect:
        Singleton<NGSoundManager>.GetInstance().playVoiceByID(this.env.core.playerUnits.value[0].unit.unitVoicePattern, 72, 0);
        break;
    }
    Singleton<NGSoundManager>.GetInstance().playSE(this.getSEname(victoryEffect));
  }

  private string getSEname(PvpVictoryEffectEnum e)
  {
    switch (e)
    {
      case PvpVictoryEffectEnum.excellent_effect:
        return "SE_0543";
      case PvpVictoryEffectEnum.great_effect:
        return "SE_0542";
      case PvpVictoryEffectEnum.win_effect:
        return "SE_0541";
      case PvpVictoryEffectEnum.lose_effect:
        return "SE_0540";
      case PvpVictoryEffectEnum.draw_effect:
        return "SE_0539";
      default:
        return "SE_0539";
    }
  }
}
