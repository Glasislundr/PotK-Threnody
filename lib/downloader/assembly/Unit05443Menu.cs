// Decompiled with JetBrains decompiler
// Type: Unit05443Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit05443Menu : EquipmentDetailMenuBase
{
  [SerializeField]
  [Tooltip("ここから先(シーン)には行かせない")]
  private bool isTerminal;
  [SerializeField]
  public UI2DSprite DynWeaponIll;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected Transform DynWeaponModel;
  [SerializeField]
  protected GameObject charaThum;
  [SerializeField]
  protected GameObject DirAddStauts;
  public Transform TopStarPos;
  public NGHorizontalScrollParts indicator;
  public Unit00443indicator indicatorPage1;
  public Unit00443indicatorDirection indicatorPage2;
  public UIGrid grid;
  public GameCore.ItemInfo RetentionGear;
  public UIWidget ZoomBuguSprite;
  protected Unit004431Menu.Param sendParam = new Unit004431Menu.Param();
  [SerializeField]
  public UI2DSprite rarityStarsIcon;
  protected GameObject itemIconPrefab;
  private int m_windowHeight;
  private int m_windowWidth;

  public bool IsTerminal => this.isTerminal;

  protected void SetTitleText(string gearName)
  {
    ((Component) this.TxtTitle).gameObject.SetActive(true);
    this.TxtTitle.SetText(gearName);
  }

  public IEnumerator Init(GameCore.ItemInfo targetGear, bool limited = false, bool is_for_reisou = false)
  {
    Unit05443Menu unit05443Menu = this;
    unit05443Menu.RetentionGear = targetGear;
    PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
    PlayerUnit equiptargets = (PlayerUnit) null;
    foreach (PlayerUnit playerUnit in playerUnitArray)
    {
      if (((IEnumerable<int?>) playerUnit.equip_gear_ids).Contains<int?>(new int?(targetGear.itemID)))
      {
        equiptargets = playerUnit;
        break;
      }
    }
    foreach (Component component in unit05443Menu.charaThum.transform)
      Object.Destroy((Object) component.gameObject);
    IEnumerator e = unit05443Menu.SetIncrementalParameter(targetGear, unit05443Menu.DirAddStauts);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> iconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = iconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon uniticon = iconPrefabF.Result.Clone(unit05443Menu.charaThum.transform).GetComponent<UnitIcon>();
    if (equiptargets == (PlayerUnit) null)
    {
      uniticon.SetEmpty();
      if (!targetGear.broken)
      {
        uniticon.SelectUnit = true;
        uniticon.onClick = (Action<UnitIconBase>) (_ => this.choiceUnitChangeScene());
      }
      else
        uniticon.onClick = (Action<UnitIconBase>) (_ => { });
    }
    else
    {
      e = uniticon.SetUnit(((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (unit => unit.unit.ID == equiptargets.unit.ID)), equiptargets.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      uniticon.setBottom(equiptargets);
      uniticon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      uniticon.setLevelText(equiptargets);
      uniticon.princessType.SetPrincessType(equiptargets);
      uniticon.onClick = (Action<UnitIconBase>) (_ => this.choiceUnitChangeScene());
      uniticon.SelectUnit = false;
    }
    if (limited)
    {
      uniticon.SelectUnit = false;
      uniticon.onClick = (Action<UnitIconBase>) (_ => { });
    }
    e = unit05443Menu.indicatorPage1.LoadPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) unit05443Menu.itemIconPrefab, (Object) null))
    {
      Future<GameObject> ItemIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = ItemIconF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit05443Menu.itemIconPrefab = ItemIconF.Result;
      ItemIconF = (Future<GameObject>) null;
    }
    unit05443Menu.indicatorPage1.Init(targetGear, (GameCore.ItemInfo) null);
    e = unit05443Menu.indicatorPage2.Init(targetGear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit05443Menu.indicator.resetScrollView();
    unit05443Menu.indicator.setItemPositionQuick(0);
    unit05443Menu.sendParam.gearId = targetGear.itemID;
    unit05443Menu.sendParam.gearKindId = targetGear.gear.kind_GearKind;
    Future<Sprite> spriteF = targetGear.gear.LoadSpriteBasic();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit05443Menu.DynWeaponIll.sprite2D = spriteF.Result;
    UI2DSprite dynWeaponIll1 = unit05443Menu.DynWeaponIll;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) dynWeaponIll1).width = num1;
    UI2DSprite dynWeaponIll2 = unit05443Menu.DynWeaponIll;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) dynWeaponIll2).height = num2;
    ((Component) unit05443Menu.DynWeaponIll).transform.localScale = Vector2.op_Implicit(new Vector2(0.8f, 0.8f));
    unit05443Menu.SetTitleText(targetGear.name);
    unit05443Menu.StartCoroutine(unit05443Menu.WaitScrollSe());
    RarityIcon.SetRarity(targetGear.gear, unit05443Menu.rarityStarsIcon);
  }

  protected IEnumerator setTexture(Future<Sprite> src, UI2DSprite to)
  {
    return src.Then<Sprite>((Func<Sprite, Sprite>) (sprite => to.sprite2D = sprite)).Wait();
  }

  public void choiceUnitChangeScene() => Unit054431Scene.ChangeScene(true, this.sendParam);

  protected IEnumerator WaitScrollSe()
  {
    yield return (object) new WaitForSeconds(0.3f);
    this.indicator.SeEnable = true;
  }

  public virtual void EndScene()
  {
    foreach (Component component in this.charaThum.transform)
      Object.Destroy((Object) component.gameObject);
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (this.RetentionGear != null)
      Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = this.RetentionGear.itemID;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnFavoriteOff()
  {
  }

  public virtual void IbtnFavoriteOn()
  {
  }

  public virtual void IbtnZoom() => Unit00446Scene.changeScene(true, this.RetentionGear.gear);

  protected override void Update()
  {
    if (this.m_windowHeight == 0 || this.m_windowWidth == 0)
    {
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    else if (this.m_windowHeight != Screen.height || this.m_windowWidth != Screen.width)
    {
      Debug.Log((object) "Window size change detected.");
      this.StartCoroutine(this.indicatorPage2.Init(this.RetentionGear));
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    base.Update();
  }
}
