// Decompiled with JetBrains decompiler
// Type: Battle01StatusScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (NGHorizontalScrollParts))]
public class Battle01StatusScrollParts : BattleMonoBehaviour
{
  private GameObject unitScrollContainer;
  [HideInInspector]
  public List<Battle01UIPlayerStatus> allPlayerStatus = new List<Battle01UIPlayerStatus>();
  private NGHorizontalScrollParts scrollParts;
  private float scrollViewX;
  private BL.BattleModified<BL.CurrentUnit> modified;
  private BattleInputObserver inputObserver;
  private BL.ForceID forceId;
  private BattleAIController _aiController;
  private int firstSelected = -1;
  private bool isRunResetScrollDragging;

  private BattleAIController aiController
  {
    get
    {
      if (Object.op_Equality((Object) this._aiController, (Object) null))
        this._aiController = this.battleManager.getController<BattleAIController>();
      return this._aiController;
    }
  }

  public override IEnumerator onInitAsync()
  {
    Battle01StatusScrollParts statusScrollParts = this;
    statusScrollParts.scrollParts = ((Component) statusScrollParts).GetComponent<NGHorizontalScrollParts>();
    statusScrollParts.scrollViewX = statusScrollParts.scrollParts.scrollView.transform.localPosition.x;
    statusScrollParts.modified = BL.Observe<BL.CurrentUnit>(statusScrollParts.env.core.unitCurrent);
    statusScrollParts.inputObserver = statusScrollParts.battleManager.getController<BattleInputObserver>();
    Future<GameObject> f = (Future<GameObject>) null;
    f = !statusScrollParts.battleManager.isSea ? Res.Prefabs.battle.Battle01_Player_Status.Load<GameObject>() : Res.Prefabs.battle.Battle01_Player_Status_sea.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    statusScrollParts.unitScrollContainer = f.Result;
    statusScrollParts.scrollParts.initParts(statusScrollParts.unitScrollContainer, statusScrollParts.env.core.playerUnits.value.Count);
  }

  protected override IEnumerator Start_Battle()
  {
    BL.ForceID fid = BL.ForceID.none;
    int idx = this.unitsIndexOf(this.modified.value.unit, ref fid);
    if (fid != BL.ForceID.player)
    {
      idx = 0;
      fid = BL.ForceID.player;
    }
    this.firstSelected = idx;
    yield return (object) new WaitForEndOfFrame();
    this.initCurrent(idx, fid);
  }

  private IEnumerator doSetItemPosition(int idx)
  {
    this.scrollParts.setItemPositionQuick(idx);
    this.firstSelected = idx;
    yield return (object) null;
  }

  public void initParts(BL.ForceID forceId, int idx = 0)
  {
    this.forceId = forceId;
    List<BL.Unit> unitList;
    switch (forceId)
    {
      case BL.ForceID.player:
        unitList = this.env.core.playerUnits.value;
        break;
      case BL.ForceID.neutral:
        unitList = this.env.core.neutralUnits.value;
        break;
      case BL.ForceID.enemy:
        unitList = this.env.core.enemyUnits.value;
        break;
      default:
        return;
    }
    if (unitList.Count == 0 || idx >= unitList.Count)
      return;
    this.scrollParts.destroyParts();
    foreach (BL.Unit unit in unitList)
    {
      if (unit.isEnable)
      {
        Battle01UIPlayerStatus component = this.scrollParts.instantiateParts(this.unitScrollContainer).GetComponent<Battle01UIPlayerStatus>();
        component.setUnit(unit);
        if (forceId == BL.ForceID.player)
        {
          component.battleStatusScrollParts = this;
          this.allPlayerStatus.Add(component);
        }
      }
    }
    this.scrollParts.resetScrollView();
    this.StartCoroutine(this.doSetItemPosition(idx));
  }

  private int unitsIndexOf(BL.Unit unit, ref BL.ForceID forceId)
  {
    if (forceId == BL.ForceID.none)
      forceId = this.env.core.getForceID(unit);
    BL.ClassValue<List<BL.Unit>> classValue = this.env.core.forceUnits(forceId);
    if (classValue == null)
      return -1;
    int num = 0;
    for (int index = 0; index < classValue.value.Count; ++index)
    {
      if (classValue.value[index].isEnable)
      {
        if (classValue.value[index] == unit)
          return num;
        ++num;
      }
    }
    return -1;
  }

  public void initCurrent(int idx, BL.ForceID fid)
  {
    if (idx == -1)
      this.forceId = fid;
    else
      this.initParts(fid, idx);
  }

  protected void Update()
  {
    if (Object.op_Equality((Object) this.scrollParts, (Object) null) || Object.op_Equality((Object) this.inputObserver, (Object) null))
      return;
    if (this.scrollParts.uiScrollView.isDragging || (double) this.scrollViewX != (double) this.scrollParts.scrollView.transform.localPosition.x)
    {
      if (this.isRunResetScrollDragging)
      {
        this.StopCoroutine("resetScrollDragging");
        this.isRunResetScrollDragging = false;
      }
      this.inputObserver.isUnitScrollDragging = true;
    }
    else if (this.inputObserver.isUnitScrollDragging && !this.isRunResetScrollDragging)
    {
      this.isRunResetScrollDragging = true;
      this.StartCoroutine("resetScrollDragging");
    }
    this.scrollViewX = this.scrollParts.scrollView.transform.localPosition.x;
  }

  private IEnumerator resetScrollDragging()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.inputObserver.isUnitScrollDragging = false;
    this.isRunResetScrollDragging = false;
  }

  private void OnDisable()
  {
    if (!this.inputObserver.isUnitScrollDragging)
      return;
    this.inputObserver.isUnitScrollDragging = false;
  }

  public void onItemChanged(int select)
  {
    if (Object.op_Equality((Object) this.battleManager, (Object) null) || Object.op_Inequality((Object) this.aiController, (Object) null) && this.aiController.isAction)
      return;
    GameObject gameObject = this.scrollParts.getItem(select);
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    BL.Unit unit = gameObject.GetComponent<Battle01UIPlayerStatus>().getUnit();
    if (!(this.env.core.unitCurrent.unit != unit))
      return;
    this.battleManager.getManager<BattleTimeManager>().setCurrentUnit(unit);
    if (this.env.core.phaseState.turnCount != 1)
      return;
    this.env.unitResource[unit].PlayVoiceDuelStart(unit);
  }

  protected override void LateUpdate_Battle()
  {
    if (!this.modified.isChangedOnce())
      return;
    BL.ForceID forceId = BL.ForceID.none;
    int idx = this.unitsIndexOf(this.modified.value.unit, ref forceId);
    if (forceId != this.forceId || idx == this.scrollParts.selected)
      return;
    this.scrollParts.setItemPosition(idx);
  }

  public void resetScrollPosition(BL.Unit unit)
  {
    BL.ForceID forceId = BL.ForceID.none;
    int idx = this.unitsIndexOf(unit, ref forceId);
    if (this.forceId == forceId && Object.op_Inequality((Object) this.scrollParts, (Object) null))
    {
      this.scrollParts.setItemPositionQuick(idx);
      if (!this.env.core.currentUnitPosition.cantChangeCurrent)
        this.scrollParts.setActiveArrow(idx);
    }
    foreach (Battle01UIPlayerStatus allPlayerStatu in this.allPlayerStatus)
      allPlayerStatu.resetDirSkillListPos();
  }
}
