// Decompiled with JetBrains decompiler
// Type: BattleUI02SubMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BattleUI02SubMenu : BattleBackButtonMenuBase
{
  private BattleUI02SubMenu.Tabname currentTab;
  [SerializeField]
  private SelectParts force;
  [SerializeField]
  private Battle02StatusScrollParts[] statusPlayerList;
  [SerializeField]
  private Battle02StatusScrollParts[] statusEnemyList;
  [SerializeField]
  private GameObject[] tabButton;
  private bool mIsEnd;
  private GameObject[] prefab = new GameObject[4];
  private BL.BattleModified<BL.StructValue<int>> forceModified;

  protected override IEnumerator Start_Battle()
  {
    BattleUI02SubMenu battleUi02SubMenu = this;
    Future<GameObject> fs0 = Res.Prefabs.battle.battle02UI_Items.Load<GameObject>();
    IEnumerator e = fs0.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleUi02SubMenu.prefab[0] = fs0.Result;
    Future<GameObject> fs1 = Res.Prefabs.battle.battleUI_02_itemBattle.Load<GameObject>();
    e = fs1.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleUi02SubMenu.prefab[1] = fs1.Result;
    Future<GameObject> fs2 = Res.Prefabs.battle.battleUI_02_itemDetail_1.Load<GameObject>();
    e = fs2.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleUi02SubMenu.prefab[2] = fs2.Result;
    Future<GameObject> fs3 = Res.Prefabs.battle.battleUI_02_itemEtc.Load<GameObject>();
    e = fs3.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleUi02SubMenu.prefab[3] = fs3.Result;
    foreach (BL.Unit v in battleUi02SubMenu.env.core.playerUnits.value)
    {
      e = Battle02MenuBase.LoadIcon(v);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    foreach (BL.Unit v in battleUi02SubMenu.env.core.enemyUnits.value)
    {
      e = Battle02MenuBase.LoadIcon(v);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    battleUi02SubMenu.force.setValueNonTween(0);
    battleUi02SubMenu.PartsUpdate();
    yield return (object) new WaitForSeconds(0.01f);
    for (int index = 0; index < battleUi02SubMenu.statusPlayerList.Length; ++index)
    {
      Transform transform = ((Component) battleUi02SubMenu.statusPlayerList[index]).GetComponent<Battle02StatusScrollParts>().chilObject[0].transform;
      transform.localPosition = new Vector3(transform.localPosition.x + 100000f, transform.localPosition.y, transform.localPosition.z);
      ((Component) battleUi02SubMenu.statusPlayerList[index]).transform.localPosition = new Vector3(((Component) battleUi02SubMenu.statusPlayerList[index]).transform.localPosition.x - 100000f, ((Component) battleUi02SubMenu.statusPlayerList[index]).transform.localPosition.y, ((Component) battleUi02SubMenu.statusPlayerList[index]).transform.localPosition.z);
    }
    for (int index = 0; index < battleUi02SubMenu.statusEnemyList.Length; ++index)
    {
      Transform transform = ((Component) battleUi02SubMenu.statusEnemyList[index]).GetComponent<Battle02StatusScrollParts>().chilObject[0].transform;
      transform.localPosition = new Vector3(transform.localPosition.x + 100000f, transform.localPosition.y, transform.localPosition.z);
      ((Component) battleUi02SubMenu.statusEnemyList[index]).transform.localPosition = new Vector3(((Component) battleUi02SubMenu.statusEnemyList[index]).transform.localPosition.x - 100000f, ((Component) battleUi02SubMenu.statusEnemyList[index]).transform.localPosition.y, ((Component) battleUi02SubMenu.statusEnemyList[index]).transform.localPosition.z);
    }
    battleUi02SubMenu.SetActiceButton(BattleUI02SubMenu.Tabname.BaseTab, true);
    battleUi02SubMenu.mIsEnd = false;
  }

  protected override void Update_Battle()
  {
  }

  public void onButtonBack()
  {
    if (this.mIsEnd)
      return;
    this.mIsEnd = true;
    this.battleManager.popupDismiss();
  }

  public override void onBackButton() => this.onButtonBack();

  public void onButtonForce()
  {
    this.force.inclementLoopNonTween();
    this.SetActiceButton(this.currentTab, true);
    Debug.Log((object) " === Submenu Force ===");
  }

  public void onButtonBase()
  {
    Debug.Log((object) " === Submenu Base ===");
    this.SetActiceButton(BattleUI02SubMenu.Tabname.BaseTab);
  }

  public void onButtonBattle()
  {
    Debug.Log((object) " === Submenu Battle ===");
    this.SetActiceButton(BattleUI02SubMenu.Tabname.BattleTab);
  }

  public void onButtonDetail()
  {
    Debug.Log((object) " === Submenu Detail ===");
    this.SetActiceButton(BattleUI02SubMenu.Tabname.DetailTab);
  }

  public void onButtonEtc()
  {
    Debug.Log((object) " === Submenu Etc ===");
    this.SetActiceButton(BattleUI02SubMenu.Tabname.EtcTab);
  }

  private void SetActiceButton(BattleUI02SubMenu.Tabname tabName, bool forcingUpdate = false)
  {
    if (this.currentTab == tabName && !forcingUpdate)
      return;
    this.currentTab = tabName;
    for (int index = 0; index < this.tabButton.Length; index += 2)
    {
      bool flag = (BattleUI02SubMenu.Tabname) (index / 2) == tabName;
      this.tabButton[index].SetActive(!flag);
      this.tabButton[index + 1].SetActive(flag);
    }
    if (this.force.getValue() == 0)
    {
      ((IEnumerable<Battle02StatusScrollParts>) this.statusEnemyList).ForEach<Battle02StatusScrollParts>((Action<Battle02StatusScrollParts>) (v => ((Component) v).gameObject.SetActive(false)));
      for (int index = 0; index < this.statusPlayerList.Length; ++index)
      {
        ((Component) ((Component) this.statusPlayerList[index]).transform.parent).gameObject.SetActive((BattleUI02SubMenu.Tabname) index == tabName);
        ((Component) this.statusPlayerList[index]).gameObject.SetActive((BattleUI02SubMenu.Tabname) index == tabName);
      }
    }
    else
    {
      ((IEnumerable<Battle02StatusScrollParts>) this.statusPlayerList).ForEach<Battle02StatusScrollParts>((Action<Battle02StatusScrollParts>) (v => ((Component) v).gameObject.SetActive(false)));
      for (int index = 0; index < this.statusEnemyList.Length; ++index)
      {
        ((Component) ((Component) this.statusEnemyList[index]).transform.parent).gameObject.SetActive((BattleUI02SubMenu.Tabname) index == tabName);
        ((Component) this.statusEnemyList[index]).gameObject.SetActive((BattleUI02SubMenu.Tabname) index == tabName);
      }
    }
  }

  private void PartsUpdate()
  {
    for (int index = 0; index < this.prefab.Length; ++index)
    {
      this.statusPlayerList[index].initParts(this.prefab[index], 0);
      this.statusEnemyList[index].initParts(this.prefab[index], 1);
    }
  }

  private void OnDestroy() => Battle02MenuBase.ClearCache();

  private enum Tabname
  {
    BaseTab,
    BattleTab,
    DetailTab,
    EtcTab,
    Max,
  }
}
