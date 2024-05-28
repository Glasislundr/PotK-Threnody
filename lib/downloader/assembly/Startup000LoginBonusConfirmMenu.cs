// Decompiled with JetBrains decompiler
// Type: Startup000LoginBonusConfirmMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Startup000LoginBonusConfirmMenu : BackButtonMenuBase
{
  private const int DEPTH = 8;
  private const float SCALE = 0.7f;
  private const float FINISH_ICON_SCALE = 1f;
  private const int DAYS_LEFT_10_OVER_IDX = 0;
  private const int DAYS_LEFT_10_UNDER_IDX = 1;
  [SerializeField]
  private List<CreateIconObject> dirThumList = new List<CreateIconObject>();
  [SerializeField]
  private CreateIconObject dynFinishReward;
  [SerializeField]
  private UILabel txtCompleteRewardItem;
  [SerializeField]
  private GameObject[] dirRemainDays;
  [SerializeField]
  private SpriteNumberSelectDirect slcTens;
  [SerializeField]
  private SpriteNumberSelectDirect[] slcOnes;
  [SerializeField]
  private GameObject slcAchievementStamp;
  private GameObject finishStampGameObject;
  private GameObject detailPopup;

  private IEnumerator CreateItemIcon(
    CreateIconObject thum,
    LoginbonusReward reward,
    float scale,
    bool gray = false)
  {
    IEnumerator e = thum.CreateThumbnail(reward.reward_type, reward.reward_id, reward.reward_quantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject icon = thum.GetIcon();
    if (gray)
    {
      IconPrefabBase component = icon.GetComponent<IconPrefabBase>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.Gray = gray;
    }
    icon.GetComponent<UIWidget>().depth = 8;
    icon.transform.localScale = new Vector3(scale, scale, 1f);
    if (reward.reward_type == MasterDataTable.CommonRewardType.unit || reward.reward_type == MasterDataTable.CommonRewardType.material_unit)
      icon.transform.localPosition = new Vector3(0.0f, 3f, 0.0f);
    UIButton component1 = ((Component) thum).GetComponent<UIButton>();
    if (Object.op_Inequality((Object) component1, (Object) null))
    {
      component1.onClick.Clear();
      component1.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.ShowDetail(reward.reward_type, reward.reward_id))));
    }
  }

  public IEnumerator Init(WebAPI.Response.LoginbonusTop loginBonus)
  {
    Future<GameObject> popupF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.detailPopup, (Object) null))
    {
      popupF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopup = popupF.Result;
      popupF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.finishStampGameObject, (Object) null))
    {
      popupF = Res.Prefabs.startup000_14.slc_Stamp_Finished.Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.finishStampGameObject = popupF.Result;
      popupF = (Future<GameObject>) null;
    }
    List<LoginbonusReward> loginBonusRewardList = ((IEnumerable<LoginbonusReward>) MasterData.LoginbonusRewardList).Where<LoginbonusReward>((Func<LoginbonusReward, bool>) (x => x.loginbonus.ID == loginBonus.login_bonus_id)).OrderBy<LoginbonusReward, int>((Func<LoginbonusReward, int>) (x => x.number)).ToList<LoginbonusReward>();
    LoginbonusLoginbonus loginBonusData = ((IEnumerable<LoginbonusLoginbonus>) MasterData.LoginbonusLoginbonusList).FirstOrDefault<LoginbonusLoginbonus>((Func<LoginbonusLoginbonus, bool>) (x => x.ID == loginBonus.login_bonus_id));
    int rewardCnt = loginBonusRewardList.Count;
    int dirThumListCnt = this.dirThumList.Count;
    for (int i = 0; i < rewardCnt; ++i)
    {
      if (i < dirThumListCnt)
      {
        LoginbonusReward reward = loginBonusRewardList[i];
        e = this.CreateItemIcon(this.dirThumList[i], reward, 0.7f);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    int num1 = loginBonus.total_login_days % loginBonusData.draw_reward_num == 0 ? loginBonusData.draw_reward_num : loginBonus.total_login_days % loginBonusData.draw_reward_num;
    int num2 = num1 < this.dirThumList.Count<CreateIconObject>() ? num1 : this.dirThumList.Count<CreateIconObject>();
    for (int index = 0; index < num2; ++index)
      this.finishStampGameObject.Clone(((Component) this.dirThumList[index]).transform).transform.localPosition = new Vector3(0.0f, 5f, 0.0f);
    LoginbonusReward loginbonusReward = loginBonusRewardList.Last<LoginbonusReward>();
    this.txtCompleteRewardItem.SetTextLocalize(CommonRewardType.GetRewardName(loginbonusReward.reward_type, loginbonusReward.reward_id, loginbonusReward.reward_quantity));
    this.slcAchievementStamp.SetActive(false);
    int n = 28;
    bool gray = false;
    if (loginBonusData != null)
      n = loginBonusData.draw_reward_num - num2;
    if (n > 9)
    {
      ((IEnumerable<GameObject>) this.dirRemainDays).ToggleOnce(0);
      this.slcTens.setNumber(n / 10, Color.white);
      this.slcOnes[0].setNumber(n % 10, Color.white);
    }
    else
    {
      ((IEnumerable<GameObject>) this.dirRemainDays).ToggleOnce(1);
      if (n > 0)
      {
        this.slcOnes[1].setNumber(n, Color.white);
      }
      else
      {
        this.slcOnes[1].setName("Hyphen", Color.white);
        this.slcAchievementStamp.SetActive(true);
        gray = true;
      }
    }
    e = this.CreateItemIcon(this.dynFinishReward, loginBonusRewardList.Last<LoginbonusReward>(), 1f, gray);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void IbtnBack() => this.onBackButton();

  private IEnumerator ShowDetailPopup(MasterDataTable.CommonRewardType rType, int rID)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.detailPopup);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Shop00742Menu>().Init(rType, rID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  public void ShowDetail(MasterDataTable.CommonRewardType rType, int rID)
  {
    if (!Shop00742Menu.IsEnableShowPopup(rType))
      return;
    this.StartCoroutine(this.ShowDetailPopup(rType, rID));
  }
}
