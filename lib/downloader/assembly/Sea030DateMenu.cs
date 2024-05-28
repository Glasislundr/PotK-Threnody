// Decompiled with JetBrains decompiler
// Type: Sea030DateMenu
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
public class Sea030DateMenu : BackButtonMenuBase
{
  [SerializeField]
  private UISprite timeIcon;
  [SerializeField]
  private UILabel timeLabel;
  [SerializeField]
  private SeaDateSpotUIButton[] BtnSpots;
  [SerializeField]
  private Transform onedariIcon;
  [SerializeField]
  private GameObject aodateIcon;
  private readonly string ImageSpriteNameBase = "slc_time_{0}.png__GUI__sea_home__sea_home_prefab";
  private GameObject dateConfirmPopupPrefab;
  private Sea030HomeMenu seaHomeMenu;
  private SeaHomeManager.UnitConrtolleData current2DUnitData;

  public IEnumerator Init(
    Sea030HomeMenu _seaHomeMenu,
    SeaHomeManager.UnitConrtolleData _current2DUnitData,
    DateTime _now,
    SeaHomeTimeZone _timeZone)
  {
    Sea030DateMenu sea030DateMenu = this;
    sea030DateMenu.seaHomeMenu = _seaHomeMenu;
    sea030DateMenu.current2DUnitData = _current2DUnitData;
    IEnumerator e1;
    if (Object.op_Equality((Object) sea030DateMenu.dateConfirmPopupPrefab, (Object) null))
    {
      Future<GameObject> futurePrefab = Res.Prefabs.popup.popup_030_sea_mypage_date_confirm__anim_fade.Load<GameObject>();
      e1 = futurePrefab.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      sea030DateMenu.dateConfirmPopupPrefab = futurePrefab.Result;
      futurePrefab = (Future<GameObject>) null;
    }
    e1 = sea030DateMenu.setTime(_now, _timeZone);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    IEnumerable<SeaDateDateSpotDisplaySetting> spotDisplaySettings = ((IEnumerable<SeaDateDateSpotDisplaySetting>) MasterData.SeaDateDateSpotDisplaySettingList).Where<SeaDateDateSpotDisplaySetting>((Func<SeaDateDateSpotDisplaySetting, bool>) (x =>
    {
      if (x.start_at.HasValue && (!x.start_at.HasValue || !(x.start_at.Value <= _now)) || x.end_at.HasValue && (!x.end_at.HasValue || !(x.end_at.Value >= _now)))
        return false;
      if (x.time_zone == null)
        return true;
      return x.time_zone != null && x.time_zone.WithIn(_now);
    }));
    foreach (SeaDateSpotUIButton btnSpot in sea030DateMenu.BtnSpots)
    {
      ((UIButtonColor) btnSpot).isEnabled = false;
      ((Component) btnSpot).gameObject.SetActive(false);
    }
    foreach (SeaDateDateSpotDisplaySetting spotDisplaySetting in spotDisplaySettings)
    {
      SeaDateDateSpotDisplaySetting setting = spotDisplaySetting;
      SeaDateSpotUIButton btnSpot = sea030DateMenu.BtnSpots[setting.datespot.ID - 1];
      ((UIButtonColor) btnSpot).isEnabled = true;
      ((Component) btnSpot).gameObject.SetActive(true);
      btnSpot.onClick.Clear();
      EventDelegate.Add(btnSpot.onClick, (EventDelegate.Callback) (() => this.onDateSpot(setting)));
    }
    List<SwitchUnitComponentBase> list = ((IEnumerable<SwitchUnitComponentBase>) ((Component) sea030DateMenu).GetComponentsInChildren<SwitchUnitComponentBase>(true)).ToList<SwitchUnitComponentBase>();
    for (int index = 0; index < list.Count; ++index)
      list[index].SwitchMaterial(_current2DUnitData.UnitID);
    if (Object.op_Implicit((Object) sea030DateMenu.onedariIcon))
      ((Component) sea030DateMenu.onedariIcon).gameObject.SetActive(false);
    if (Object.op_Implicit((Object) sea030DateMenu.aodateIcon))
      sea030DateMenu.aodateIcon.gameObject.SetActive(false);
    if (PerformanceConfig.GetInstance().IsOnedariDate)
    {
      PlayerCallLetter playerCallLetter = ((IEnumerable<PlayerCallLetter>) Singleton<NGGameDataManager>.GetInstance().callLetter).Where<PlayerCallLetter>((Func<PlayerCallLetter, bool>) (x => x.same_character_id == _current2DUnitData.Unit.same_character_id)).FirstOrDefault<PlayerCallLetter>();
      if (playerCallLetter != null && playerCallLetter.call_status == 1 && playerCallLetter.call_status == 1)
      {
        Future<WebAPI.Response.SeaCallDateCondition> future = WebAPI.SeaCallDateCondition(_current2DUnitData.Unit.same_character_id, (Action<WebAPI.Response.UserError>) (e =>
        {
          if (Singleton<NGGameDataManager>.GetInstance().IsSea && string.Equals(e.Code, "SEA000"))
          {
            this.StartCoroutine(PopupUtility.SeaError(e));
          }
          else
          {
            WebAPI.DefaultUserErrorCallback(e);
            MypageScene.ChangeSceneOnError();
          }
        }));
        e1 = future.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (future.Result.conditions.condition_type_id.HasValue)
        {
          switch ((TalkMessageConditonType) future.Result.conditions.condition_type_id.Value)
          {
            case TalkMessageConditonType.GoDate:
              int condition_id = 0;
              if (future.Result.conditions.condition_id.HasValue)
                condition_id = future.Result.conditions.condition_id.Value;
              SeaDateDateSpotDisplaySetting spotDisplaySetting1 = ((IEnumerable<SeaDateDateSpotDisplaySetting>) MasterData.SeaDateDateSpotDisplaySettingList).First<SeaDateDateSpotDisplaySetting>((Func<SeaDateDateSpotDisplaySetting, bool>) (x => x.datespot.ID == condition_id));
              sea030DateMenu.SetOnedariIcon(((Component) sea030DateMenu.BtnSpots[spotDisplaySetting1.datespot.ID - 1]).transform);
              break;
            case TalkMessageConditonType.GoDateBlue:
              if (Object.op_Implicit((Object) sea030DateMenu.aodateIcon))
              {
                sea030DateMenu.aodateIcon.gameObject.SetActive(true);
                break;
              }
              break;
            case TalkMessageConditonType.GoDateRed:
              IEnumerable<SeaDateHitExpansionLottery> source = ((IEnumerable<SeaDateHitExpansionLottery>) MasterData.SeaDateHitExpansionLotteryList).Where<SeaDateHitExpansionLottery>((Func<SeaDateHitExpansionLottery, bool>) (x => x.time_zone_id_SeaHomeTimeZone != 0 && x.date_spot_id_SeaDateDateSpot != 0));
              SeaDateHitExpansionLottery seaDateHit = source.FirstOrDefault<SeaDateHitExpansionLottery>((Func<SeaDateHitExpansionLottery, bool>) (x =>
              {
                int? characterIdUnitUnit = x.same_character_id_UnitUnit;
                int sameCharacterId = _current2DUnitData.Unit.same_character_id;
                return characterIdUnitUnit.GetValueOrDefault() == sameCharacterId & characterIdUnitUnit.HasValue;
              })) ?? source.First<SeaDateHitExpansionLottery>((Func<SeaDateHitExpansionLottery, bool>) (x =>
              {
                int? characterId = x.character_id;
                int id = _current2DUnitData.Unit.character.ID;
                return characterId.GetValueOrDefault() == id & characterId.HasValue;
              }));
              SeaDateDateSpotDisplaySetting spotDisplaySetting2 = ((IEnumerable<SeaDateDateSpotDisplaySetting>) MasterData.SeaDateDateSpotDisplaySettingList).First<SeaDateDateSpotDisplaySetting>((Func<SeaDateDateSpotDisplaySetting, bool>) (x => x.datespot == seaDateHit.date_spot_id));
              if (spotDisplaySetting2 != null)
              {
                sea030DateMenu.SetOnedariIcon(((Component) sea030DateMenu.BtnSpots[spotDisplaySetting2.datespot.ID - 1]).transform);
                ((Component) sea030DateMenu.onedariIcon).gameObject.SetActive(seaDateHit.time_zone_id.WithIn(_now));
                break;
              }
              if (Object.op_Implicit((Object) sea030DateMenu.onedariIcon))
              {
                ((Component) sea030DateMenu.onedariIcon).gameObject.SetActive(false);
                break;
              }
              break;
          }
        }
        future = (Future<WebAPI.Response.SeaCallDateCondition>) null;
      }
    }
  }

  private void SetOnedariIcon(Transform tran)
  {
    if (!Object.op_Implicit((Object) this.onedariIcon))
      return;
    ((Component) this.onedariIcon).gameObject.SetActive(true);
    this.onedariIcon.SetParent(tran);
    this.onedariIcon.localPosition = Vector2.op_Implicit(new Vector2(0.0f, 56f));
  }

  public IEnumerator setTime(DateTime _now, SeaHomeTimeZone _timeZone)
  {
    this.timeLabel.SetTextLocalize(_now.ToString("HH:mm"));
    if (_timeZone != null)
    {
      this.timeIcon.spriteName = string.Format(this.ImageSpriteNameBase, (object) _timeZone.image_pattern);
      yield break;
    }
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.seaHomeMenu.isReturnSelectSpot = true;
    this.seaHomeMenu.isSelectedSpot = false;
    this.seaHomeMenu.ResetParticalObj();
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  private void onDateSpot(SeaDateDateSpotDisplaySetting setting)
  {
    if (this.IsPushAndSet())
      return;
    this.seaHomeMenu.isReturnSelectSpot = false;
    this.OpenDateConfirmPopup(setting);
  }

  private void OpenDateConfirmPopup(SeaDateDateSpotDisplaySetting setting)
  {
    Singleton<PopupManager>.GetInstance().open(this.dateConfirmPopupPrefab).GetComponent<Sea030HomeDateConfirmPopup>().Init(this.current2DUnitData.PlayerUnit, setting, new Action<SeaDateDateSpotDisplaySetting>(this.DepartForDate));
  }

  public void DepartForDate(SeaDateDateSpotDisplaySetting setting)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    this.StartCoroutine(this.internalDepartForDate(setting));
  }

  private IEnumerator internalDepartForDate(SeaDateDateSpotDisplaySetting setting)
  {
    Sea030DateMenu sea030DateMenu = this;
    bool isSeaSeasonEnd = false;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim(true);
    CommonRoot cr = Singleton<CommonRoot>.GetInstance();
    cr.isTouchBlock = true;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    cr.loadingMode = 1;
    cr.isLoading = true;
    Future<WebAPI.Response.SeaDateStart> futureAPI = WebAPI.SeaDateStart(setting.datespot.ID, sea030DateMenu.current2DUnitData.PlayerUnit.id, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (string.Equals(e.Code, "SEA000"))
      {
        isSeaSeasonEnd = true;
        this.StartCoroutine(PopupUtility.SeaError(e));
      }
      else
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }
    }));
    IEnumerator e1 = futureAPI.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (futureAPI.Result == null)
    {
      if (!isSeaSeasonEnd)
      {
        cr.isLoading = false;
        cr.loadingMode = 0;
        cr.isTouchBlock = false;
        sea030DateMenu.IsPush = false;
        Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
        cr.isLoading = true;
        e1 = new Future<NGGameDataManager.StartSceneProxyResult>(new Func<Promise<NGGameDataManager.StartSceneProxyResult>, IEnumerator>(Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxyImpl)).Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        cr.isLoading = false;
      }
    }
    else
    {
      sea030DateMenu.seaHomeMenu.ExistSelect = futureAPI.Result.happening_id > 0 || futureAPI.Result.quiz_id > 0;
      cr.isLoading = false;
      sea030DateMenu.seaHomeMenu.DateSetting = setting;
      sea030DateMenu.seaHomeMenu.DateFlows = ((IEnumerable<int>) futureAPI.Result.script_ids).ToList<int>();
      sea030DateMenu.seaHomeMenu.NowFlowIndex = 0;
      sea030DateMenu.seaHomeMenu.EventSelectData.Clear();
      sea030DateMenu.seaHomeMenu.QuizId = Sea030HomeMenu.hasQuiz(futureAPI.Result.date_flow) ? new int?(futureAPI.Result.quiz_id) : new int?();
      sea030DateMenu.seaHomeMenu.isSelectedSpot = true;
      cr.loadingMode = 3;
      cr.isLoading = true;
      cr.isTouchBlock = false;
      sea030DateMenu.IsPush = false;
      sea030DateMenu.backScene();
    }
  }
}
