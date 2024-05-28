// Decompiled with JetBrains decompiler
// Type: Unit004ExtraskillInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004ExtraskillInfo : MonoBehaviour
{
  public static int None = -1;
  public static int StartTweenID = 42212;
  public static int EndTweenID = 42221;
  [SerializeField]
  private ExtraSkillInfo extraSkillInfo;
  [SerializeField]
  private GameObject dirUnitThumContainer;
  private Unit004SelectExtraSkillListMenuBase menuInstanc;
  private InventoryExtraSkill inventorySkill;
  [SerializeField]
  private UIButton btnDecision;
  [SerializeField]
  private UISprite slcBtnDecisionText;
  private GameObject skillDetailPrefab;

  public IEnumerator InitSkillInfo(
    Unit004SelectExtraSkillListMenuBase menu,
    InventoryExtraSkill invSkill,
    int tweenID,
    bool isSelectUnit = true)
  {
    Unit004ExtraskillInfo unit004ExtraskillInfo = this;
    unit004ExtraskillInfo.menuInstanc = menu;
    unit004ExtraskillInfo.inventorySkill = invSkill;
    IEnumerator e = unit004ExtraskillInfo.extraSkillInfo.Init(invSkill.skill, invSkill.favorite, menu.SpriteCache(invSkill.skill.masterData), menu.SkillGenrePrefab, new Action<bool>(unit004ExtraskillInfo.chageFavorite));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004ExtraskillInfo.dirUnitThumContainer.transform.Clear();
    UnitIcon unitIconScript = menu.UnitIconPrefab.CloneAndGetComponent<UnitIcon>(unit004ExtraskillInfo.dirUnitThumContainer.transform);
    if (invSkill.equipUnit != (PlayerUnit) null)
    {
      e = unitIconScript.SetUnit(invSkill.equipUnit, invSkill.equipUnit.unit.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitIconScript.setLevelText(invSkill.equipUnit);
      unitIconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    }
    else
    {
      unitIconScript.SetEmpty();
      unitIconScript.SelectUnit = isSelectUnit;
    }
    unitIconScript.onClick = (Action<UnitIconBase>) (ui => menu.changeSceneExtraSkillEquipment(invSkill.equipUnit, invSkill.skill));
    if (Object.op_Equality((Object) unit004ExtraskillInfo.skillDetailPrefab, (Object) null))
    {
      Future<GameObject> skillDetailLoader = PopupSkillDetails.createPrefabLoader(false);
      yield return (object) skillDetailLoader.Wait();
      unit004ExtraskillInfo.skillDetailPrefab = skillDetailLoader.Result;
      skillDetailLoader = (Future<GameObject>) null;
    }
    if (tweenID != Unit004ExtraskillInfo.None)
      unit004ExtraskillInfo.StartTween(tweenID);
  }

  public void StartTween(int groupID)
  {
    foreach (UITweener component in ((Component) this).GetComponents<UITweener>())
    {
      if (component.tweenGroup == groupID)
      {
        component.ResetToBeginning();
        component.PlayForward();
      }
    }
  }

  public void chageFavorite(bool favorite)
  {
    this.menuInstanc.changeExtraSkillFavorite(this.inventorySkill, favorite);
  }

  public void setEnableIbtnDecision(bool isEnable)
  {
    ((UIButtonColor) this.btnDecision).isEnabled = isEnable;
    ((UIWidget) this.slcBtnDecisionText).color = isEnable ? Color.white : Color.gray;
  }

  public void onClickDecision() => this.menuInstanc.onClickDecision();

  public void onClickSkillZoom()
  {
    if (this.inventorySkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null))
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, PopupSkillDetails.Param.createBySkillView(this.inventorySkill.skill));
  }

  public void setEnableFavoriteSwitch(bool bEnabled)
  {
    this.extraSkillInfo.setEnableFavoriteSwitch(bEnabled);
  }
}
