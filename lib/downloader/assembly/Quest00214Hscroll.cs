// Decompiled with JetBrains decompiler
// Type: Quest00214Hscroll
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
public class Quest00214Hscroll : MonoBehaviour
{
  [SerializeField]
  private UILabel apCost;
  public UISprite newSprite;
  public UISprite clearSprite;
  [SerializeField]
  private Transform StageNumbersParent_UnLock;
  [SerializeField]
  private Transform StageNumbersParent_Lock;
  public LongPressButton StageButton_UnLock;
  [SerializeField]
  private List<UISprite> StageNumbers;
  [SerializeField]
  private GameObject StageButton_Lock;
  [SerializeField]
  private float defaultScale = 1f;
  [SerializeField]
  private float changedScale = 0.9f;
  [SerializeField]
  private float SpaceValue = 1f;
  [SerializeField]
  private List<UIWidget> ObjWidgets;
  public GameObject Mask;
  [SerializeField]
  private int StageNumber;
  [SerializeField]
  private int ID;
  [SerializeField]
  private int AP;
  [SerializeField]
  private UISprite StageBounas;
  private bool anim;
  private bool UnLock;

  public bool isUnLock() => this.UnLock;

  public float defaultPosition() => ((Component) this).transform.localPosition.x;

  public float spaceValue() => this.SpaceValue;

  public int id() => this.ID;

  public int stageNumber() => this.StageNumber;

  public Action<Action> onBeforeChangeScene { get; set; }

  public void LockCondition()
  {
    ((Component) this.StageNumbersParent_UnLock).gameObject.SetActive(false);
    ((Component) this.StageButton_UnLock).gameObject.SetActive(false);
    ((Component) this.apCost).gameObject.SetActive(false);
    ((Component) this.newSprite).gameObject.SetActive(false);
    ((Component) this.clearSprite).gameObject.SetActive(false);
    ((Component) this.StageNumbersParent_Lock).gameObject.SetActive(true);
    this.StageButton_Lock.SetActive(true);
    ((Component) this.StageBounas).gameObject.SetActive(false);
    this.UnLock = false;
  }

  public void UnLockCondition()
  {
    ((Component) this.StageNumbersParent_UnLock).gameObject.SetActive(true);
    ((Component) this.StageButton_UnLock).gameObject.SetActive(true);
    ((Component) this.apCost).gameObject.SetActive(true);
    ((Component) this.clearSprite).gameObject.SetActive(true);
    ((Component) this.StageNumbersParent_Lock).gameObject.SetActive(false);
    this.StageButton_Lock.SetActive(false);
    ((Component) this.newSprite).gameObject.SetActive(false);
    ((Component) this.StageBounas).gameObject.SetActive(true);
    this.UnLock = true;
  }

  public void onSetValue()
  {
    this.defaultScale = 1f;
    this.changedScale = 0.9f;
    float cellWidth = ((Component) ((Component) this).transform.parent).GetComponent<UIGrid>().cellWidth;
    this.SpaceValue = (double) cellWidth <= 0.0 ? this.SpaceValue : cellWidth;
  }

  private void WidgetGetInit()
  {
    this.ObjWidgets = new List<UIWidget>();
    if (!this.UnLock)
    {
      foreach (Transform transform in this.StageNumbersParent_Lock)
      {
        this.StageNumbers.Add(((Component) transform).GetComponent<UISprite>());
        this.ObjWidgets.Add(((Component) transform).GetComponent<UIWidget>());
      }
      this.ObjWidgets.Add(((Component) this.StageButton_Lock.transform).GetComponent<UIWidget>());
    }
    else
    {
      foreach (Transform transform in this.StageNumbersParent_UnLock)
      {
        this.StageNumbers.Add(((Component) transform).GetComponent<UISprite>());
        this.ObjWidgets.Add(((Component) transform).GetComponent<UIWidget>());
      }
      this.ObjWidgets.Add(((Component) ((Component) this.StageButton_UnLock).transform).GetComponent<UIWidget>());
    }
    this.ObjWidgets.Add(((Component) ((Component) this.apCost).transform).GetComponent<UIWidget>());
    this.ObjWidgets.Add(((Component) ((Component) this.newSprite).transform).GetComponent<UIWidget>());
    this.ObjWidgets.Add(((Component) ((Component) this.clearSprite).transform).GetComponent<UIWidget>());
  }

  public void Init(
    QuestSConverter StageData,
    PlayerQuestSConverter[] charaque,
    int num,
    float gridWidth,
    int center,
    EventDelegate.Callback onLongPressed)
  {
    this.WidgetGetInit();
    this.StageNumber = num;
    this.ID = StageData.ID;
    this.SetButtonSprite(StageData.story_only);
    PlayerQuestSConverter stageData = ((IEnumerable<PlayerQuestSConverter>) charaque).FirstOrDefault<PlayerQuestSConverter>((Func<PlayerQuestSConverter, bool>) (x => x.questS.ID == StageData.ID));
    if (stageData != null)
    {
      EventDelegate.Set(this.StageButton_UnLock.onClick, (EventDelegate.Callback) (() =>
      {
        NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
        if (Object.op_Inequality((Object) instance, (Object) null))
          instance.playSE("SE_1002");
        this.StartCoroutine(this.PopupJudge(stageData, StageData.story_only));
      }));
      ((Component) this.newSprite).gameObject.SetActive(stageData.is_new);
      ((Component) this.clearSprite).gameObject.SetActive(stageData.is_clear);
      this.SetBonusRewardImage(stageData.clear_rewards);
    }
    this.InitValue(StageData, gridWidth, center);
    if (onLongPressed == null)
      return;
    EventDelegate.Set(this.StageButton_UnLock.onLongPress, StageData.story_only ? (EventDelegate.Callback) null : onLongPressed);
  }

  private void SetBonusRewardImage(int[] clear_rewards)
  {
    if (clear_rewards == null || clear_rewards.Length == 0 || !this.UnLock)
    {
      ((Component) this.StageBounas).gameObject.SetActive(false);
    }
    else
    {
      List<BattleStageClear> list = ((IEnumerable<int>) clear_rewards).Where<int>((Func<int, bool>) (x => MasterData.BattleStageClear.ContainsKey(x))).Select<int, BattleStageClear>((Func<int, BattleStageClear>) (x => MasterData.BattleStageClear[x])).ToList<BattleStageClear>();
      if (list.Count <= 0)
      {
        ((Component) this.StageBounas).gameObject.SetActive(false);
      }
      else
      {
        ((Component) this.StageBounas).gameObject.SetActive(true);
        UISpriteData uiSpriteData = (UISpriteData) null;
        UISprite component = ((Component) this.StageBounas).GetComponent<UISprite>();
        if (list.Count == 1)
        {
          string str = string.Format("slc_BonusIcon_{0}.png__GUI__002-14_sozai__002-14_sozai_prefab", (object) list[0].entity_type_CommonRewardType);
          uiSpriteData = component.atlas.GetSprite(str);
          if (uiSpriteData != null)
            component.spriteName = str;
        }
        if (uiSpriteData != null || list.Count <= 0)
          return;
        string str1 = "slc_BonusIcon_Other.png__GUI__002-14_sozai__002-14_sozai_prefab";
        component.spriteName = str1;
      }
    }
  }

  public void InitValue(QuestSConverter StageData, float gridWidth, int centernum)
  {
    ((Component) this).GetComponent<TweenPosition>().from.x = (float) ((this.StageNumber - centernum - 1) * 139);
    ((Component) this).GetComponent<TweenPosition>().to.x = (float) ((this.StageNumber - centernum - 1) * 139);
    foreach (UISprite stageNumber in this.StageNumbers)
    {
      if (int.Parse(((Object) ((Component) stageNumber).gameObject).name.Substring(((Object) ((Component) stageNumber).gameObject).name.Length - 2)) == this.StageNumber)
        ((Component) stageNumber).gameObject.SetActive(true);
      else
        ((Component) stageNumber).gameObject.SetActive(false);
    }
    if (!this.UnLock)
      return;
    this.AP = StageData.lost_ap;
    this.apCost.SetTextLocalize(this.AP);
    if (((Component) this.clearSprite).gameObject.activeSelf)
      return;
    foreach (UIWidget objWidget in this.ObjWidgets)
      objWidget.color = Color.white;
  }

  public void ChangeToneConditionJudge(float variationValue)
  {
    float changeValue = Mathf.Abs(variationValue) / this.SpaceValue;
    if ((double) changeValue >= 1.0)
      this.ChangeToneCondition(1f);
    else if ((double) changeValue <= 0.0)
      this.ChangeToneCondition(0.0f);
    else
      this.ChangeToneCondition(changeValue);
  }

  private void ChangeToneCondition(float changeValue)
  {
    float num = (this.defaultScale - this.changedScale) * changeValue;
    Color color = Color.Lerp(Color.white, Color.gray, changeValue);
    ((Component) this).transform.localScale = new Vector3(this.defaultScale - num, this.defaultScale - num, this.defaultScale);
    foreach (UIWidget objWidget in this.ObjWidgets)
      objWidget.color = !((Component) this.clearSprite).gameObject.activeSelf ? color : (!(((Object) objWidget).name == "slc_Clear") ? Color.Lerp(Color.white, Color.gray, 1f) : color);
  }

  public void NotTouch(bool judge)
  {
    this.Mask.SetActive(judge);
    ((Behaviour) this.StageButton_UnLock).enabled = !judge;
  }

  public void centerAnimation(bool flag)
  {
    if (this.anim != flag & flag)
    {
      Color color = Color.Lerp(Color.white, Color.gray, 0.0f);
      foreach (UIWidget objWidget in this.ObjWidgets)
        objWidget.color = color;
      ((Behaviour) ((Component) this).GetComponent<TweenScale>()).enabled = true;
      ((UITweener) ((Component) this).GetComponent<TweenScale>()).PlayForward();
    }
    else if (!flag)
      ((Behaviour) ((Component) this).GetComponent<TweenScale>()).enabled = false;
    this.anim = flag;
  }

  public IEnumerator PopupJudge(PlayerQuestSConverter StageData, bool story_only)
  {
    Player player = SMManager.Get<Player>();
    IEnumerator e;
    if (player.CheckMaxHavingUnit() || player.CheckLimitOverMaxUnitReserves())
    {
      e = PopupUtility._999_5_1();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (player.CheckMaxHavingGear())
    {
      e = PopupUtility._999_6_1(true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (player.CheckMaxHavingReisou())
    {
      e = PopupUtility.popupMaxReisou();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (player.ap - StageData.consumed_ap < 0)
    {
      if (player.ap_max - StageData.consumed_ap < 0)
      {
        e = this.maxAPshortage_Popoup();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = this.noAP_Popup((Action) (() =>
        {
          if (SMManager.Get<Player>().ap - StageData.consumed_ap < 0)
            return;
          this.changeScene(StageData, story_only);
        }));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else if (player.ap - StageData.consumed_ap >= 0)
      this.changeScene(StageData, story_only);
  }

  private void changeScene(PlayerQuestSConverter stageData, bool story_only)
  {
    if (this.onBeforeChangeScene == null)
      Quest0028Scene.changeScene(true, stageData, story_only);
    else
      this.StartCoroutine(this.doWaitChangeScene(stageData, story_only));
  }

  private IEnumerator doWaitChangeScene(PlayerQuestSConverter stageData, bool story_only)
  {
    bool bWait = true;
    this.onBeforeChangeScene((Action) (() => bWait = false));
    while (bWait)
      yield return (object) null;
    Quest0028Scene.changeScene(true, stageData, story_only);
  }

  public IEnumerator noAP_Popup(Action questChangeScene)
  {
    IEnumerator e = PopupUtility.RecoveryAP(true, questChangeScene);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator maxAPshortage_Popoup()
  {
    Future<GameObject> popupF = Res.Prefabs.popup.popup_002_2_ap1__anim_popup01.Load<GameObject>();
    IEnumerator e = popupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popupF.Result);
  }

  private void SetButtonSprite(bool story_only)
  {
    UIButton component1 = ((Component) this.StageButton_UnLock).gameObject.GetComponent<UIButton>();
    string str = ".png__GUI__002-14_sozai__002-14_sozai_prefab";
    if (story_only)
    {
      if (Object.op_Inequality((Object) component1, (Object) null))
      {
        component1.normalSprite = "ibtn_Scene_idle" + str;
        component1.pressedSprite = "ibtn_Scene_pressed" + str;
        component1.disabledSprite = "ibtn_Scene_idle" + str;
      }
      UISprite component2 = this.StageButton_Lock.GetComponent<UISprite>();
      if (!Object.op_Inequality((Object) component2, (Object) null))
        return;
      component2.spriteName = "ibtn_Scenedisabled" + str;
    }
    else
    {
      if (Object.op_Inequality((Object) component1, (Object) null))
      {
        component1.normalSprite = "ibtn_Stage_idle" + str;
        component1.pressedSprite = "ibtn_Stage_pressed" + str;
        component1.disabledSprite = "ibtn_Stage_idle" + str;
      }
      UISprite component3 = this.StageButton_Lock.GetComponent<UISprite>();
      if (!Object.op_Inequality((Object) component3, (Object) null))
        return;
      component3.spriteName = "ibtn_Stagedisabled" + str;
    }
  }
}
