// Decompiled with JetBrains decompiler
// Type: CorpsQuestUnitListMenu
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
public class CorpsQuestUnitListMenu : UnitMenuBase
{
  private HashSet<int> UsedUnitIds;
  private GameObject goHpGauge;

  public IEnumerator LoadResources()
  {
    Future<GameObject> f = Res.Prefabs.tower.dir_hp_gauge.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.goHpGauge = f.Result;
  }

  private void SetUnitIconGray(int unit_index)
  {
    if (unit_index >= this.allUnitIcons.Count || Object.op_Equality((Object) this.allUnitIcons[unit_index], (Object) null))
      return;
    UnitIcon allUnitIcon = (UnitIcon) this.allUnitIcons[unit_index];
    bool flag = this.UsedUnitIds.Contains(allUnitIcon.PlayerUnit.id);
    allUnitIcon.Gray = flag;
    if (Object.op_Equality((Object) allUnitIcon.HpGauge, (Object) null))
      return;
    int totalHp = allUnitIcon.PlayerUnit.total_hp;
    allUnitIcon.HpGauge.SetGauge(flag ? 0 : totalHp, totalHp, false);
  }

  public IEnumerator InitializeAsync(int[] entryUnitIds, int[] usedUnitIds)
  {
    CorpsQuestUnitListMenu questUnitListMenu = this;
    PlayerUnit[] playerUnits = ((IEnumerable<int>) entryUnitIds).Select<int, PlayerUnit>((Func<int, PlayerUnit>) (x => ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (y => y.id == x)).FirstOrDefault<PlayerUnit>())).ToArray<PlayerUnit>();
    questUnitListMenu.UsedUnitIds = new HashSet<int>((IEnumerable<int>) usedUnitIds);
    IEnumerator e = questUnitListMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    questUnitListMenu.SetIconType(UnitMenuBase.IconType.NormalWithHpGauge);
    questUnitListMenu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.corpsUnitListSortAndFilter, false, false, false, true, false);
    e = questUnitListMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    questUnitListMenu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) (playerUnits.Length - usedUnitIds.Length), (object) playerUnits.Length));
    questUnitListMenu.InitializeEnd();
  }

  protected override void CreateAllUnitInfo(
    IEnumerable<PlayerUnit> playerUnits,
    IEnumerable<PlayerMaterialUnit> playerMaterialUnits,
    bool isEquip,
    bool removeButton,
    bool for_battle_check,
    bool princessType,
    bool isSpecialIcon,
    int maxDispMaterialUnit)
  {
    base.CreateAllUnitInfo(playerUnits, playerMaterialUnits, isEquip, removeButton, for_battle_check, princessType, isSpecialIcon, maxDispMaterialUnit);
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      allUnitInfo.is_used = this.UsedUnitIds.Contains(allUnitInfo.playerUnit.id);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onClickUnitlIcon(PlayerUnit unit)
  {
    Singleton<NGSceneManager>.GetInstance().LastHeaderType = new CommonRoot.HeaderType?(Singleton<CommonRoot>.GetInstance().headerType);
    Unit0042Scene.changeScene(true, unit, this.getUnits());
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
    this.SetUnitIconGray(unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
    this.SetUnitIconGray(unit_index);
  }

  protected void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    unitIcon.onClick = (Action<UnitIconBase>) (ui => this.onClickUnitlIcon(unitIcon.PlayerUnit));
  }

  protected override IEnumerator CreateUnitIconBase(GameObject prefab)
  {
    CorpsQuestUnitListMenu questUnitListMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = questUnitListMenu.\u003C\u003En__1(prefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) questUnitListMenu.goHpGauge, (Object) null))
    {
      for (int index = 0; index < questUnitListMenu.allUnitIcons.Count; ++index)
      {
        UnitIcon allUnitIcon = (UnitIcon) questUnitListMenu.allUnitIcons[index];
        questUnitListMenu.goHpGauge.Clone(allUnitIcon.hp_gauge.transform);
        if (allUnitIcon.PlayerUnit != (PlayerUnit) null)
        {
          bool flag = questUnitListMenu.UsedUnitIds.Contains(allUnitIcon.PlayerUnit.id);
          int totalHp = allUnitIcon.PlayerUnit.total_hp;
          allUnitIcon.HpGauge.SetGauge(flag ? 0 : totalHp, totalHp, false);
        }
      }
    }
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }
}
