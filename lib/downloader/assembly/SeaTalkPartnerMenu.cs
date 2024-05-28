// Decompiled with JetBrains decompiler
// Type: SeaTalkPartnerMenu
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
public class SeaTalkPartnerMenu : BackButtonMenuBase
{
  private const int ITEM_HEIGHT = 115;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private BoxCollider scrollBoxColider;
  [SerializeField]
  private UIDragScrollView dragScrollView;
  [SerializeField]
  private GameObject noPartnerView;
  [SerializeField]
  private UILabel activePartnerCountLabel;
  [SerializeField]
  private UILabel nonCallInfoLabel;
  private GameObject itemPrefab;
  private Sea030HomeMenu seaHomeMenu;
  public WebAPI.Response.SeaTalkPartner response;

  public IEnumerator onInitSceneAsync()
  {
    yield return (object) this.LoadResource();
  }

  public IEnumerator onStartSceneAsync(
    WebAPI.Response.SeaTalkPartner response,
    Sea030HomeMenu seaHomeMenu)
  {
    this.response = response;
    this.seaHomeMenu = seaHomeMenu;
    this.noPartnerView.SetActive(response.partners.Length == 0);
    if (this.noPartnerView.activeSelf)
      this.nonCallInfoLabel.text = string.Format("好感度{0}%以上の姫をデートに誘おう", (object) Consts.GetInstance().CALL_TRUST_NUM);
    this.Sort();
    ((Component) this.scrollView).transform.Clear();
    int height = 0;
    PlayerTalkPartner[] playerTalkPartnerArray = response.partners;
    for (int index = 0; index < playerTalkPartnerArray.Length; ++index)
    {
      PlayerTalkPartner playerTalkPartner = playerTalkPartnerArray[index];
      GameObject go = this.itemPrefab.Clone(((Component) this.scrollView).transform);
      yield return (object) go.GetComponent<SeaTalkPartnerItem>().Init(playerTalkPartner, response.partners);
      go.transform.localPosition = new Vector3(0.0f, (float) height, 0.0f);
      height -= 115;
      go = (GameObject) null;
    }
    playerTalkPartnerArray = (PlayerTalkPartner[]) null;
    this.scrollView.ResetPosition();
    if (!this.scrollView.shouldMoveVertically)
    {
      ((Collider) this.scrollBoxColider).enabled = ((Behaviour) this.dragScrollView).enabled = false;
      foreach (Component component in ((Component) this.scrollView).transform)
        ((Behaviour) component.GetComponent<UIDragScrollView>()).enabled = false;
    }
    this.activePartnerCountLabel.text = string.Format("Talkフレンド数　{0}/10", (object) Mathf.Min(((IEnumerable<PlayerTalkPartner>) response.partners).Count<PlayerTalkPartner>((Func<PlayerTalkPartner, bool>) (x => x.letter.call_status == 1)), 10));
  }

  private IEnumerator LoadResource()
  {
    Future<GameObject> f = new ResourceObject("Prefabs/sea030_talk/dir_talk_hemeList").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.itemPrefab = f.Result;
  }

  private void Sort()
  {
    List<PlayerTalkPartner> source1 = new List<PlayerTalkPartner>();
    List<PlayerTalkPartner> source2 = new List<PlayerTalkPartner>();
    List<PlayerTalkPartner> source3 = new List<PlayerTalkPartner>();
    List<PlayerTalkPartner> source4 = new List<PlayerTalkPartner>();
    List<PlayerTalkPartner> source5 = new List<PlayerTalkPartner>();
    foreach (PlayerTalkPartner partner in this.response.partners)
    {
      if (partner.letter.call_status == 3)
        source1.Add(partner);
      else if (partner.letter.call_status == 2 && partner.unread_count > 0)
        source2.Add(partner);
      else if (partner.letter.call_status == 1)
        source3.Add(partner);
      else if (partner.letter.call_status == 2 && partner.unread_count <= 0)
        source4.Add(partner);
      else
        source5.Add(partner);
    }
    List<PlayerTalkPartner> list1 = source1.OrderByDescending<PlayerTalkPartner, DateTime>((Func<PlayerTalkPartner, DateTime>) (x => x.message.created_at)).ThenBy<PlayerTalkPartner, int>((Func<PlayerTalkPartner, int>) (x => x.letter.player_letter_id)).ToList<PlayerTalkPartner>();
    List<PlayerTalkPartner> list2 = source2.OrderByDescending<PlayerTalkPartner, DateTime>((Func<PlayerTalkPartner, DateTime>) (x => x.message.created_at)).ThenBy<PlayerTalkPartner, int>((Func<PlayerTalkPartner, int>) (x => x.letter.player_letter_id)).ToList<PlayerTalkPartner>();
    List<PlayerTalkPartner> list3 = source3.OrderByDescending<PlayerTalkPartner, DateTime>((Func<PlayerTalkPartner, DateTime>) (x => x.message.created_at)).ThenBy<PlayerTalkPartner, int>((Func<PlayerTalkPartner, int>) (x => x.letter.player_letter_id)).ToList<PlayerTalkPartner>();
    List<PlayerTalkPartner> list4 = source4.OrderByDescending<PlayerTalkPartner, DateTime>((Func<PlayerTalkPartner, DateTime>) (x => x.message.created_at)).ThenBy<PlayerTalkPartner, int>((Func<PlayerTalkPartner, int>) (x => x.letter.player_letter_id)).ToList<PlayerTalkPartner>();
    List<PlayerTalkPartner> list5 = source5.OrderByDescending<PlayerTalkPartner, DateTime>((Func<PlayerTalkPartner, DateTime>) (x => x.message.created_at)).ThenBy<PlayerTalkPartner, int>((Func<PlayerTalkPartner, int>) (x => x.letter.player_letter_id)).ToList<PlayerTalkPartner>();
    this.response.partners = list1.Concat<PlayerTalkPartner>((IEnumerable<PlayerTalkPartner>) list2).Concat<PlayerTalkPartner>((IEnumerable<PlayerTalkPartner>) list3).Concat<PlayerTalkPartner>((IEnumerable<PlayerTalkPartner>) list4).Concat<PlayerTalkPartner>((IEnumerable<PlayerTalkPartner>) list5).ToArray<PlayerTalkPartner>();
  }

  public void OnMenu()
  {
    Singleton<CommonRoot>.GetInstance().GetSeaHeaderComponent().OpenMenuForTalk();
  }

  public void onClickedHelp()
  {
    HelpCategory helpCategory = (HelpCategory) null ?? Array.Find<HelpCategory>(MasterData.HelpCategoryList, (Predicate<HelpCategory>) (x => x.ID == 35));
    Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    if (Singleton<NGSceneManager>.GetInstance().sceneName == "help015_2")
      Help0152Scene.ChangeScene(false, helpCategory);
    else
      Help0152Scene.ChangeScene(true, helpCategory);
  }

  public override void onBackButton() => this.IbtnBack();

  private void IbtnBack()
  {
    this.seaHomeMenu.isReturnSelectSpot = true;
    this.seaHomeMenu.isSelectedSpot = false;
    this.seaHomeMenu.ResetParticalObj();
    this.backScene();
  }
}
