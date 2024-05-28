// Decompiled with JetBrains decompiler
// Type: Guide01132Menu
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
public class Guide01132Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtIntroduction;
  [SerializeField]
  protected UILabel TxtJobname;
  [SerializeField]
  protected UILabel TxtNumber;
  [SerializeField]
  protected UILabel TxtSubjugate;
  [SerializeField]
  protected UILabel TxtSubjugateNum;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UI2DSprite DynCharacter;
  [SerializeField]
  public UI2DSprite rarityStarsIcon;
  [SerializeField]
  private UnitUnit unit_;
  public List<GuideEnemyGearChange> enemyGearChangeList = new List<GuideEnemyGearChange>();
  private int m_windowHeight;
  private int m_windowWidth;
  private RenderTextureRecoveryUtil util;

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public virtual void IbtnZoom()
  {
    if (this.IsPushAndSet())
      return;
    Unit0043Scene.changeScene(true, this.unit_, this.unit_.job_UnitJob, true, bLibrary: true);
  }

  public void IbtnGearChange(GuideEnemyGearChange GuideEnemyGearChange)
  {
    this.StartCoroutine(this.onStartSceneAsync(GuideEnemyGearChange.unitData, false));
  }

  public IEnumerator onStartSceneAsync(UnitUnit unit, bool voiceFlag)
  {
    this.unit_ = unit;
    List<PlayerEnemyHistory> zukanDataList = ((IEnumerable<PlayerEnemyHistory>) SMManager.Get<PlayerEnemyHistory[]>()).ToList<PlayerEnemyHistory>();
    List<UnitUnit> unitDataList = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.character.category == UnitCategory.enemy)).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.history_group_number == unit.history_group_number)).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.character.category == unit.character.category)).ToList<UnitUnit>();
    this.TxtIntroduction.SetTextLocalize(unit.description);
    this.TxtJobname.SetTextLocalize(unit.job.name);
    this.SetNumber(unit);
    this.SetDefeat(unit, unitDataList.ToArray(), zukanDataList.ToArray());
    this.TxtTitle.SetTextLocalize(unit.name);
    this.SetUnitRarity(unit);
    IEnumerator e = this.SetUnitImg(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetGearChange(unit, unitDataList.ToArray(), zukanDataList.ToArray());
    if (voiceFlag && Object.op_Implicit((Object) Singleton<NGSoundManager>.GetInstanceOrNull()))
    {
      Singleton<NGSoundManager>.GetInstance().stopVoice();
      Singleton<NGSoundManager>.GetInstance().playVoiceByID(unit.unitVoicePattern, 42);
    }
  }

  public void SetDefeat(UnitUnit unit, UnitUnit[] commonUnitList, PlayerEnemyHistory[] historyList)
  {
    int defeat = 0;
    ((IEnumerable<PlayerEnemyHistory>) historyList).ForEach<PlayerEnemyHistory>((Action<PlayerEnemyHistory>) (obj =>
    {
      if (obj.unit_id != unit.ID)
        return;
      defeat += obj.defeat;
    }));
    if (defeat > 99999)
      defeat = 99999;
    this.TxtSubjugateNum.SetTextLocalize(defeat);
  }

  public void SetNumber(UnitUnit unit)
  {
    this.TxtNumber.SetTextLocalize("NO." + (unit.history_group_number % 10000).ToString().PadLeft(4, '0'));
  }

  public void SetUnitRarity(UnitUnit unit)
  {
    RarityIcon.SetRarity(unit, this.rarityStarsIcon, true, true);
  }

  public IEnumerator SetUnitImg(UnitUnit unit)
  {
    Future<Sprite> fSprite = unit.LoadFullSprite();
    if (fSprite != null)
    {
      IEnumerator e = fSprite.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.DynCharacter.sprite2D = fSprite.Result;
    }
  }

  public void SetGearChange(UnitUnit unit, UnitUnit[] commonUnitList, PlayerEnemyHistory[] history)
  {
    int sabun = this.enemyGearChangeList.Count - ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.character.category == UnitCategory.enemy)).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.history_group_number == unit.history_group_number)).ToList<UnitUnit>().OrderBy<UnitUnit, int>((Func<UnitUnit, int>) (x => x.ID)).ToList<UnitUnit>().Count;
    for (int i = 0; i < this.enemyGearChangeList.Count; i++)
    {
      if (sabun > i)
        ((Component) this.enemyGearChangeList[i]).gameObject.SetActive(false);
      else if (!((IEnumerable<PlayerEnemyHistory>) history).FirstIndexOrNull<PlayerEnemyHistory>((Func<PlayerEnemyHistory, bool>) (x => x.unit_id == commonUnitList[i - sabun].ID)).HasValue)
        this.enemyGearChangeList[i].Set(commonUnitList[i - sabun], false, true);
      else if (unit.ID == commonUnitList[i - sabun].ID)
        this.enemyGearChangeList[i].Set(commonUnitList[i - sabun], true, false);
      else
        this.enemyGearChangeList[i].Set(commonUnitList[i - sabun], false, false);
    }
  }

  protected override void Update()
  {
    if (this.m_windowHeight == 0 || this.m_windowWidth == 0)
    {
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    else if (this.m_windowHeight != Screen.height || this.m_windowWidth != Screen.width)
    {
      this.StartCoroutine(this.onStartSceneAsync(this.unit_, false));
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    base.Update();
    if (!Object.op_Inequality((Object) this.util, (Object) null))
      return;
    this.util.FixRenderTexture();
  }
}
