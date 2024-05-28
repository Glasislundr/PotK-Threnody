// Decompiled with JetBrains decompiler
// Type: Unit0048Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit0048Menu : UnitMenuBase
{
  public const int SOZAI_MAX = 5;
  protected int unitCount;
  [SerializeField]
  private GameObject lumpToutaButtonView;

  public Unit00468Scene.Mode mode { get; set; }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public bool IsInit()
  {
    return ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Count<PlayerUnit>() != this.unitCount;
  }

  private void UpdateBottomInfo()
  {
    Player player = SMManager.Get<Player>();
    int length = SMManager.Get<PlayerUnit[]>().Length;
    this.unitCount = length;
    this.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) length, (object) player.max_units));
  }

  public virtual IEnumerator Init(Player player, PlayerUnit[] playerUnits, bool isEquip)
  {
    Unit0048Menu unit0048Menu = this;
    IEnumerator e = unit0048Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>();
    unit0048Menu.InitializeInfo((IEnumerable<PlayerUnit>) array, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit0048SortAndFilter, isEquip, false, true, true, false);
    e = unit0048Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0048Menu.lumpToutaButtonView.SetActive(true);
    unit0048Menu.UpdateBottomInfo();
    Singleton<PopupManager>.GetInstance().closeAll();
    unit0048Menu.lastReferenceUnitID = -1;
    unit0048Menu.InitializeEnd();
  }

  private void ChangeScene(UnitIconBase unitIcon)
  {
    if (unitIcon.PlayerUnit != (PlayerUnit) null)
    {
      this.lastReferenceUnitID = unitIcon.PlayerUnit.id;
      this.lastReferenceUnitIndex = this.GetUnitInfoDisplayIndex(unitIcon.PlayerUnit);
      if (this.mode == Unit00468Scene.Mode.Unit0048)
        Unit004TrainingScene.changeCombine(true, unitIcon.PlayerUnit, bDisabledTab: new bool?(true));
      else
        Unit004TrainingScene.changeReinforce(true, unitIcon.PlayerUnit, bDisabledTab: new bool?(true));
    }
    else
      Debug.LogWarning((object) "PlayerUnit Null : Unit0048Menu");
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    IEnumerator e = base.CreateUnitIcon(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
  }

  private void CreateUnitIconAction(int info_index, int unit_index)
  {
    this.allUnitIcons[unit_index].onClick = (Action<UnitIconBase>) (ui => this.ChangeScene(ui));
  }

  public override IEnumerator UpdateInfoAndScroll(
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits = null)
  {
    IEnumerator e = base.UpdateInfoAndScroll(playerUnits, playerMaterialUnits);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.UpdateBottomInfo();
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().setStartScene("mypage");
    this.backScene();
  }

  public void OnLumpToutaButton() => Unit00468Scene.changeScene00491Evolution(false);
}
