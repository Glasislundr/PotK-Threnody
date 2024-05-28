// Decompiled with JetBrains decompiler
// Type: PopupRecommendMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using LocaleTimeZone;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Recommend/Menu")]
public class PopupRecommendMenu : BackButtonPopupBase
{
  [SerializeField]
  private UIScrollView scrollView_;
  [SerializeField]
  private PopupRecommendPartAdvice advice_;
  [SerializeField]
  private PopupRecommendPartAwake awake_;
  [SerializeField]
  private PopupRecommendPartCallSkill callSkill_;
  [SerializeField]
  private PopupRecommendPartOverkillers overkillers_;
  [SerializeField]
  private PopupRecommendPartUnitType unitType_;
  [SerializeField]
  private PopupRecommendPartJobChange jobChange_;
  [SerializeField]
  private PopupRecommendPartEquipments equipments_;
  private bool isFirstBottom_;
  private PlayerUnit playerUnit_;
  private float[] yOffsets_;

  public static Future<GameObject> loadResource()
  {
    return new ResourceObject("Prefabs/unit/Popup_Recommend_Info").Load<GameObject>();
  }

  public static void open(
    GameObject prefab,
    PlayerUnit playerUnit,
    Action actChangedScene = null,
    Action<Action> actChangedQuest = null,
    bool bDisabledAccountStatus = false,
    bool bDisabledChangeQuest = false)
  {
    PopupRecommendMenu.openCommon(prefab, playerUnit, actChangedScene, actChangedQuest, bDisabledAccountStatus, bDisabledChangeQuest, false);
  }

  public static void openReturnScene(
    GameObject prefab,
    PlayerUnit playerUnit,
    Action actChangedScene,
    Action<Action> actChangedQuest,
    bool bAutoBootQuestList)
  {
    PopupRecommendMenu.openCommon(prefab, playerUnit, actChangedScene, actChangedQuest, false, false, bAutoBootQuestList).isFirstBottom_ = true;
  }

  private static PopupRecommendMenu openCommon(
    GameObject prefab,
    PlayerUnit playerUnit,
    Action actChangedScene,
    Action<Action> actChangedQuest,
    bool bDisabledAccountStatus,
    bool bDisabledChangeQuest,
    bool bAutoBootQuestList)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true);
    PopupRecommendMenu component = gameObject.GetComponent<PopupRecommendMenu>();
    component.setTopObject(gameObject);
    gameObject.GetComponent<UIRect>().alpha = 0.0f;
    component.playerUnit_ = playerUnit;
    component.onChangedScene = actChangedScene;
    component.onChangedQuest = actChangedQuest;
    component.isDisabledAccountStatus = bDisabledAccountStatus;
    component.isDisabledChangeQuest = bDisabledChangeQuest;
    component.isAutoBootQuestList = bAutoBootQuestList;
    return component;
  }

  public GameObject skillDetailPrefab { get; private set; }

  public Action onChangedScene { get; private set; }

  public Action<Action> onChangedQuest { get; private set; }

  public bool isInitialized { get; private set; }

  public bool isDisabledAccountStatus { get; private set; }

  public bool isDisabledChangeQuest { get; private set; }

  public bool isAutoBootQuestList { get; private set; }

  private PopupRecommendPart[] recommendParts
  {
    get
    {
      return new PopupRecommendPart[7]
      {
        (PopupRecommendPart) this.advice_,
        (PopupRecommendPart) this.awake_,
        (PopupRecommendPart) this.callSkill_,
        (PopupRecommendPart) this.overkillers_,
        (PopupRecommendPart) this.unitType_,
        (PopupRecommendPart) this.jobChange_,
        (PopupRecommendPart) this.equipments_
      };
    }
  }

  private IEnumerator Start()
  {
    PopupRecommendMenu popupRecommendMenu = this;
    int[] genealogyIds = UnitEvolutionPattern.getGenealogyIds(popupRecommendMenu.playerUnit_._unit);
    UnitUnit target = popupRecommendMenu.playerUnit_.unit;
    if (genealogyIds.Length != 0)
    {
      DateTime dateTime1 = TimeZoneInfo.ConvertTime(ServerTime.NowAppTimeAddDelta(), Japan.CreateTimeZone());
      // ISSUE: reference to a compiler-generated method
      foreach (int key in ((IEnumerable<int>) genealogyIds).Skip<int>(((IEnumerable<int>) genealogyIds).FirstIndexOrNull<int>(new Func<int, bool>(popupRecommendMenu.\u003CStart\u003Eb__45_0)).Value + 1))
      {
        UnitUnit unitUnit = MasterData.UnitUnit[key];
        if (unitUnit.published_at.HasValue)
        {
          DateTime? publishedAt = unitUnit.published_at;
          DateTime dateTime2 = dateTime1;
          if ((publishedAt.HasValue ? (publishedAt.GetValueOrDefault() > dateTime2 ? 1 : 0) : 0) != 0)
            break;
        }
        target = unitUnit;
      }
    }
    popupRecommendMenu.StartCoroutine("doLoadResource");
    PopupRecommendPart[] popupRecommendPartArray = popupRecommendMenu.recommendParts;
    for (int index = 0; index < popupRecommendPartArray.Length; ++index)
      yield return (object) popupRecommendPartArray[index].doInitialize(popupRecommendMenu.playerUnit_, target);
    popupRecommendPartArray = (PopupRecommendPart[]) null;
    popupRecommendMenu.repositionScrollItems();
    if (!popupRecommendMenu.isAutoBootQuestList)
      Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popupRecommendMenu).gameObject);
    popupRecommendMenu.isInitialized = true;
  }

  private void repositionScrollItems()
  {
    if (this.GetCallChara() == null)
      ((Component) this.callSkill_).gameObject.SetActive(false);
    GameObject[] array1 = ((IEnumerable<PopupRecommendPart>) this.recommendParts).Select<PopupRecommendPart, GameObject>((Func<PopupRecommendPart, GameObject>) (x => ((Component) x).gameObject)).ToArray<GameObject>();
    if (this.yOffsets_ == null)
    {
      float[] array2 = ((IEnumerable<GameObject>) array1).Select<GameObject, float>((Func<GameObject, float>) (x => x.transform.localPosition.y)).ToArray<float>();
      this.yOffsets_ = new float[array2.Length];
      this.yOffsets_[0] = array2[0];
      for (int index = 1; index < array2.Length; ++index)
        this.yOffsets_[index] = array2[index] - array2[index - 1];
    }
    float yOffset = this.yOffsets_[0];
    int num = this.yOffsets_.Length - 1;
    for (int index = 0; index < array1.Length; ++index)
    {
      if (array1[index].activeSelf)
      {
        Transform transform = array1[index].transform;
        transform.localPosition = new Vector3(transform.localPosition.x, yOffset, transform.localPosition.z);
        if (num > index)
          yOffset += this.yOffsets_[index + 1];
      }
    }
    this.scrollView_.ResetPosition();
    if (!this.isFirstBottom_)
      return;
    GameObject target = ((IEnumerable<GameObject>) array1).Last<GameObject>();
    if (!target.activeSelf)
      return;
    this.scrollView_.RestrictWithinObject(target, false);
  }

  private CallCharacter GetCallChara()
  {
    CallCharacter[] callCharacterList = MasterData.CallCharacterList;
    CallCharacter callChara = (CallCharacter) null;
    foreach (CallCharacter callCharacter in callCharacterList)
    {
      if (callCharacter.same_character_id == this.playerUnit_.unit.same_character_id)
      {
        callChara = callCharacter;
        break;
      }
    }
    return callChara;
  }

  private IEnumerator doLoadResource()
  {
    if (Object.op_Equality((Object) this.skillDetailPrefab, (Object) null))
    {
      Future<GameObject> ld = PopupSkillDetails.createPrefabLoader(false);
      yield return (object) ld.Wait();
      this.skillDetailPrefab = ld.Result;
      ld = (Future<GameObject>) null;
    }
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
