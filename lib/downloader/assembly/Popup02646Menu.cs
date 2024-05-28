// Decompiled with JetBrains decompiler
// Type: Popup02646Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup02646Menu : MonoBehaviour
{
  [SerializeField]
  private UILabel txtName;
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtDescription;
  [SerializeField]
  private UILabel txtLevel;
  [SerializeField]
  private UILabel txtTimeLimit;
  [SerializeField]
  private UI2DSprite linkTitle;
  [SerializeField]
  private GameObject slcFriend;
  [SerializeField]
  private GameObject slcNotFriend;
  [SerializeField]
  private GameObject slcFirstBattle;
  [SerializeField]
  private GameObject linkUnitThum;
  private Action actionOk;
  private Action actionNo;
  private Action actionMatchingCancel;
  private int timeSeconds = 15;
  private float nowTime;
  private bool isTimeOut;
  private bool isIbtnNo;

  public IEnumerator Init(
    Action actionOk,
    Action actionNo,
    Action actionMatchingCancel,
    WebAPI.Response.PvpFriend fData)
  {
    this.slcFriend.SetActive(fData.is_friend);
    this.slcNotFriend.SetActive(!fData.is_friend);
    this.slcFirstBattle.SetActive(fData.is_first_battle);
    this.txtName.SetText(fData.target_player_name);
    this.txtLevel.SetText(fData.level.ToLocalizeNumberText());
    Consts instance = Consts.GetInstance();
    this.txtTitle.SetText(instance.VERSUS_002646POPUP_TITLE);
    this.txtDescription.SetText(instance.VERSUS_002646POPUP_DESCRIPTION);
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(fData.current_emblem_id);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.linkTitle.sprite2D = sprF.Result;
    PlayerUnit leaderUnit = (PlayerUnit) null;
    leaderUnit = PlayerUnit.create_by_unitunit(MasterData.UnitUnit[fData.leader_unit_id]);
    leaderUnit.job_id = fData.leader_unit_job_id;
    Future<GameObject> PrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon unitIconScript = PrefabF.Result.Clone(this.linkUnitThum.transform).GetComponent<UnitIcon>();
    e = unitIconScript.setSimpleUnit(leaderUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitIconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    unitIconScript.setLevelText(fData.leader_unit_level.ToLocalizeNumberText());
    unitIconScript.Button.onLongPress.Clear();
    this.isTimeOut = false;
    this.isIbtnNo = false;
    this.nowTime = Time.time;
    this.actionOk = actionOk;
    this.actionNo = actionNo;
    this.actionMatchingCancel = actionMatchingCancel;
  }

  private void Update()
  {
    if (!this.isTimeOut && (double) this.nowTime + 1.0 <= (double) Time.time)
    {
      --this.timeSeconds;
      this.nowTime = Time.time;
      if (this.timeSeconds < 0)
      {
        this.isTimeOut = true;
        this.TimeOutProc();
      }
      else
        this.txtTimeLimit.SetTextLocalize(this.timeSeconds);
    }
    if (Singleton<CommonRoot>.GetInstance().isInputBlock || !Input.GetKeyUp((KeyCode) 27))
      return;
    this.IbtnNo();
  }

  private void TimeOutProc()
  {
    this.IbtnNo();
    Singleton<PopupManager>.GetInstance().onDismiss();
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.VERSUS_002645POPUP_TITLE, instance.VERSUS_002645POPUP_DESCRIPTION, (Action) (() => { }));
  }

  public void IbtnOk()
  {
    if (this.isTimeOut)
      return;
    this.StartCoroutine(this.CreatePopupWait());
    this.actionOk();
  }

  public void IbtnNo()
  {
    if (this.isIbtnNo)
      return;
    this.isIbtnNo = true;
    this.actionNo();
  }

  private IEnumerator CreatePopupWait()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_026_4_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().onDismiss(true);
    Popup02642SerchMatching component = Singleton<PopupManager>.GetInstance().open(prefabF.Result).GetComponent<Popup02642SerchMatching>();
    component.Init(this.actionMatchingCancel);
    component.DisableButton();
  }
}
