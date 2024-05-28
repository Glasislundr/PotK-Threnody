// Decompiled with JetBrains decompiler
// Type: MypageTransition
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
public class MypageTransition : MonoBehaviour
{
  [SerializeField]
  private MypageRootMenu menu;
  public int L_id;
  public bool hardmode;

  public void onQuest()
  {
    if (this.menu.IsPushAndSet())
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1002");
    PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
    if (source != null && ((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_xl_QuestStoryXL == 4)))
      this.StartCoroutine(this.openStorySelectPopup());
    else
      Debug.LogError((object) "quests data is null or not exists LostRagnarok");
  }

  public void onBattle()
  {
    if (this.menu.IsPushAndSet())
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1002");
    this.StartCoroutine(this.openBattleSelectPopup());
  }

  public void onEvent()
  {
    if (this.menu.IsPushAndSet())
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1002");
    Quest00217Scene.ChangeScene(true);
  }

  public void onMenu()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story001_9_1", true);
  }

  public void onInfo()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_8_1", true);
  }

  public void onMission()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("dailymission027_2", true);
  }

  public void onPanelMission()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("dailymission027_1", true);
  }

  public void onPresent()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_7", true);
  }

  public void onColosseum()
  {
    if (this.menu.IsPushAndSet())
      return;
    this._on_colosseum_func();
  }

  public void onRoulette()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("roulette", true);
  }

  public static MypageTransition.ColosseumStatus getColosseumStatus()
  {
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    if (source.Length < 3)
      return MypageTransition.ColosseumStatus.NOT_ENOUGH_UNIT;
    Player player = SMManager.Get<Player>();
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) source).OrderBy<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.cost)).ToArray<PlayerUnit>();
    int num = 0;
    for (int index = 0; index < 3; ++index)
      num += array[index].cost;
    return player.max_cost >= num ? MypageTransition.ColosseumStatus.OK : MypageTransition.ColosseumStatus.NOT_ENOUGH_COST;
  }

  private void _on_colosseum_func()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1002");
    switch (MypageTransition.getColosseumStatus())
    {
      case MypageTransition.ColosseumStatus.OK:
        Colosseum0234Scene.ChangeScene(true);
        break;
      case MypageTransition.ColosseumStatus.NOT_ENOUGH_COST:
        this.StartCoroutine(this.openPopup008161CostInsufficiency());
        break;
      case MypageTransition.ColosseumStatus.NOT_ENOUGH_UNIT:
        this.StartCoroutine(this.openPopup008161UnitInsufficiency());
        break;
    }
  }

  public void onCharacter()
  {
    if (this.menu.IsPushAndSet())
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1002");
    Quest00214Scene.ChangeScene(true);
  }

  public void onMulti()
  {
    if (this.menu.IsPushAndSet())
      return;
    this._on_multi_func();
  }

  private void _on_multi_func()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1002");
    Versus0261Scene.ChangeScene0261(true);
  }

  public void onEarth()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1039");
    this.StartCoroutine(this.menu.CloudAnimationStart());
  }

  public void onUnitEdit()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("mypage_edit", true);
  }

  public void onGuild()
  {
    if (!PlayerAffiliation.Current.isGuildMember())
    {
      if (this.menu.IsPushAndSet())
        return;
      Guild02811Scene.ChangeScene();
    }
    else
    {
      if (MypageRootMenu.CurrentMode == MypageRootMenu.Mode.STORY && PlayerAffiliation.Current.guild != null)
      {
        if (PlayerAffiliation.Current.onGvgTransition && !Persist.guildEventCheck.Data.isGuildBattleTransition)
        {
          Persist.guildEventCheck.Data.isGuildBattleTransition = true;
          Persist.guildEventCheck.Flush();
          Guild0282Scene.ChangeScene();
          return;
        }
        WebAPI.Response.GuildTop guildTopResponse = Singleton<NGGameDataManager>.GetInstance().GuildTopResponse;
        if (guildTopResponse != null && PlayerAffiliation.Current.isGuildMember() && !Persist.guildEventCheck.Data.isGuildRaidTransition && guildTopResponse.raid_period != null)
        {
          Persist.guildEventCheck.Data.isGuildRaidTransition = true;
          Persist.guildEventCheck.Flush();
          RaidTopScene.ChangeScene();
          return;
        }
      }
      this.menu.SwitchMode();
    }
  }

  public void onGuildBank()
  {
    if (this.menu.IsPushAndSet())
      return;
    Guild0287Scene.ChangeScene();
  }

  public void onGuildGift()
  {
    if (this.menu.IsPushAndSet())
      return;
    Guild0286Scene.ChangeScene(true);
  }

  public void onGuildSearch()
  {
    if (this.menu.IsPushAndSet())
      return;
    Guild02811Scene.ChangeScene();
  }

  public void onRaidShop()
  {
    if (this.menu.IsPushAndSet())
      return;
    Raid032ShopScene.ChangeScene(true);
  }

  public void onExplore()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Singleton<NGSceneManager>.GetInstance().destoryNonStackScenes();
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Explore033TopScene.changeScene();
  }

  public void onLoginBonus()
  {
    if (this.menu.IsPushAndSet())
      return;
    Startup000LoginBonusConfirmScene.changeScene(true);
  }

  public void onFriend()
  {
    if (this.menu.IsPushAndSet())
      return;
    Friend0081Scene.ChangeScene();
  }

  private IEnumerator openPopup008161UnitInsufficiency()
  {
    Consts c = Consts.GetInstance();
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_16_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject pObj = Singleton<PopupManager>.GetInstance().open(prefabF.Result);
    pObj.SetActive(false);
    e = pObj.GetComponent<Friend008161Menu>().Init(c.COLOSSEUM_ALERT_TITLE_UNIT, c.COLOSSEUM_ALERT_MESSAGE_UNIT);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    pObj.SetActive(true);
  }

  private IEnumerator openPopup008161CostInsufficiency()
  {
    Consts c = Consts.GetInstance();
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_16_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject pObj = Singleton<PopupManager>.GetInstance().open(prefabF.Result);
    pObj.SetActive(false);
    e = pObj.GetComponent<Friend008161Menu>().Init(c.COLOSSEUM_ALERT_TITLE_COST, c.COLOSSEUM_ALERT_MESSAGE_COST);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    pObj.SetActive(true);
  }

  private IEnumerator openStorySelectPopup()
  {
    MypageTransition mypageTransition = this;
    Future<GameObject> prefabF = new ResourceObject("Prefabs/mypage/dir_story_menu").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popup = prefabF.Result.Clone();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    e = popup.GetComponent<PopupMypageStorySelect>().Initialize(new Action<int, bool>(mypageTransition.\u003CopenStorySelectPopup\u003Eb__31_0), (Action) null, (MonoBehaviour) mypageTransition.menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true, isNonSe: true);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1056");
  }

  private IEnumerator openBattleSelectPopup()
  {
    MypageTransition mypageTransition = this;
    Future<GameObject> prefabF = new ResourceObject("Prefabs/mypage/dir_battle_menu").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popup = prefabF.Result.Clone();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    e = popup.GetComponent<PopupMypageBattleSelect>().Initialize(new Action<PopupMypageBattleSelect.Selection>(mypageTransition.\u003CopenBattleSelectPopup\u003Eb__32_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  private IEnumerator ChangeScene0024(int L, bool hard)
  {
    Singleton<NGSceneManager>.GetInstance().destoryNonStackScenes();
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Quest00240723Scene.ChangeScene0024(false, L, hard, true);
    yield break;
  }

  public void OnPushTotalPaymentBonus()
  {
    if (this.menu.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenTotalPaymentBonusPopup());
  }

  private IEnumerator OpenTotalPaymentBonusPopup()
  {
    MypageTransition mypageTransition = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.PaymentbonusList> api = WebAPI.PaymentbonusList(new Action<WebAPI.Response.UserError>(mypageTransition.\u003COpenTotalPaymentBonusPopup\u003Eb__35_0));
    IEnumerator e = api.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!api.HasResult || api.Result == null)
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      mypageTransition.menu.IsPush = false;
    }
    else
    {
      WebAPI.Response.PaymentbonusList response = api.Result;
      Future<GameObject> loader = new ResourceObject("Prefabs/popup/popup_TotalPaymentBonus").Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = loader.Result;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      TotalPaymentBonusPopup component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<TotalPaymentBonusPopup>();
      ((Component) component).transform.localScale = Vector3.zero;
      e = component.Initialize(response, mypageTransition.menu.EventButtonController.GetButton<TotalPaymentBonusButton>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator PushFalseWithDelay(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    this.menu.IsPush = false;
  }

  public enum ColosseumStatus
  {
    OK,
    NOT_ENOUGH_COST,
    NOT_ENOUGH_UNIT,
  }
}
