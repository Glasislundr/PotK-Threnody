// Decompiled with JetBrains decompiler
// Type: Quest0028PopupBattleSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest0028PopupBattleSetting : BackButtonMenuBase
{
  [SerializeField]
  private ToggleTweenPositionControl toggleAuto_;
  [SerializeField]
  private ToggleTweenPositionControl toggleItemMove_;
  [SerializeField]
  private ToggleTweenPositionControl toggleCallSkill_;
  private Persist.AutoBattleSetting saveData_;
  private bool isModified_;

  public static IEnumerator show()
  {
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    string path;
    if (!instance.IsSea || instance.QuestType.HasValue)
    {
      CommonQuestType? questType = instance.QuestType;
      CommonQuestType commonQuestType = CommonQuestType.Sea;
      if (!(questType.GetValueOrDefault() == commonQuestType & questType.HasValue))
      {
        path = "Prefabs/popup/popup_002_8_auto_setting__anim_popup01";
        goto label_5;
      }
    }
    path = "Prefabs/popup/popup_002_8_auto_setting_sea__anim_popup01";
label_5:
    Future<GameObject> ldPrefab = new ResourceObject(path).Load<GameObject>();
    IEnumerator e = ldPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = ldPrefab.Result;
    ldPrefab = (Future<GameObject>) null;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      GameObject go = Singleton<PopupManager>.GetInstance().open(result);
      while (go.activeInHierarchy)
        yield return (object) null;
    }
  }

  private void Start()
  {
    try
    {
      this.saveData_ = Persist.autoBattleSetting.Data;
    }
    catch
    {
      Persist.autoBattleSetting.Delete();
      this.saveData_ = Persist.autoBattleSetting.Data = new Persist.AutoBattleSetting();
      this.isModified_ = true;
    }
    if (Object.op_Inequality((Object) this.toggleAuto_, (Object) null))
    {
      this.toggleAuto_.resetSwitch(this.saveData_.isAutoBattle);
      this.toggleAuto_.setCheckCancel(new Func<bool>(this.checkToggleCancel));
    }
    this.toggleItemMove_.resetSwitch(this.saveData_.isItemMove);
    this.toggleItemMove_.setCheckCancel(new Func<bool>(this.checkToggleCancel));
    this.toggleCallSkill_.resetSwitch(this.saveData_.isCallSkill);
    this.toggleCallSkill_.setCheckCancel(new Func<bool>(this.checkToggleCancel));
  }

  private bool checkToggleCancel() => this.IsPush;

  private void saveData()
  {
    if (Object.op_Inequality((Object) this.toggleAuto_, (Object) null) && this.toggleAuto_.isSwitch != this.saveData_.isAutoBattle)
    {
      this.isModified_ = true;
      this.saveData_.isAutoBattle = this.toggleAuto_.isSwitch;
    }
    if (this.toggleItemMove_.isSwitch != this.saveData_.isItemMove)
    {
      this.isModified_ = true;
      this.saveData_.isItemMove = this.toggleItemMove_.isSwitch;
    }
    if (this.toggleCallSkill_.isSwitch != this.saveData_.isCallSkill)
    {
      this.isModified_ = true;
      this.saveData_.isCallSkill = this.toggleCallSkill_.isSwitch;
    }
    if (!this.isModified_)
      return;
    this.isModified_ = false;
    Persist.autoBattleSetting.Flush();
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    this.saveData();
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
