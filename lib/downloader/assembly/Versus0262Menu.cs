// Decompiled with JetBrains decompiler
// Type: Versus0262Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniLinq;
using UnityEngine;

#nullable disable
public class Versus0262Menu : Versus026MatchBase
{
  [SerializeField]
  private GameObject randomEnemyStatus;
  [SerializeField]
  private GameObject friendEnemyStatus;
  [SerializeField]
  private GameObject[] btnStrengthOn;
  [SerializeField]
  private GameObject btnFriendOn;
  [SerializeField]
  private UIButton btnFriendOff;
  private bool hasFriend;

  public override IEnumerator Init(PvpMatchingTypeEnum type, WebAPI.Response.PvpBoot pvpInfo)
  {
    Versus0262Menu versus0262Menu = this;
    ((Component) versus0262Menu.txtPass).GetComponent<UIInput>().caretColor = Color.black;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = versus0262Menu.\u003C\u003En__0(type, pvpInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus0262Menu.InitDisplayType();
    if (pvpInfo.is_tutorial)
    {
      if (Persist.pvpInfo.Data.currentPage == 1 && (type == PvpMatchingTypeEnum.challenge || type == PvpMatchingTypeEnum.normal))
        Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton("pvp2", new Dictionary<string, Func<Transform, UIButton>>()
        {
          {
            "versus_random3",
            (Func<Transform, UIButton>) (root =>
            {
              Transform childInFind = root.GetChildInFind("Bottom").GetChildInFind("dir_Bonus");
              UILabel component1 = ((Component) childInFind.GetChildInFind("txt_Bonus")).GetComponent<UILabel>();
              UILabel component2 = ((Component) childInFind.GetChildInFind("txt_Timelimit")).GetComponent<UILabel>();
              Bonus[] array = ((IEnumerable<Bonus>) pvpInfo.bonus).Where<Bonus>((Func<Bonus, bool>) (x => x.category != 12)).ToArray<Bonus>();
              if (array.Length != 0)
              {
                component1.text = array[0].DisplayName(true);
                component2.text = array[0].RemainingTime();
              }
              return (UIButton) null;
            })
          },
          {
            "versus_random4",
            (Func<Transform, UIButton>) (root =>
            {
              Transform childInFind1 = root.GetChildInFind("Bottom");
              Transform childInFind2 = childInFind1.GetChildInFind("ibtn_StartMatch");
              Transform childInFind3 = childInFind1.GetChildInFind("ibtn_GearRepair");
              if (this.isRepair)
              {
                ((Component) childInFind2).gameObject.SetActive(false);
                ((Component) childInFind3).gameObject.SetActive(true);
              }
              else
              {
                ((Component) childInFind2).gameObject.SetActive(true);
                ((Component) childInFind3).gameObject.SetActive(false);
              }
              return (UIButton) null;
            })
          },
          {
            "versus_random7",
            (Func<Transform, UIButton>) (root =>
            {
              UIButton[] componentsInChildren = ((Component) root.GetChildInFind("Bottom")).GetComponentsInChildren<UIButton>();
              UIButton uiButton1 = ((IEnumerable<UIButton>) componentsInChildren).First<UIButton>((Func<UIButton, bool>) (v => ((Object) v).name == "ibtn_StartMatch"));
              UIButton uiButton2 = ((IEnumerable<UIButton>) componentsInChildren).First<UIButton>((Func<UIButton, bool>) (v => ((Object) v).name == "ibtn_GearRepair"));
              if (this.isRepair)
              {
                ((Component) uiButton1).gameObject.SetActive(false);
                ((Component) uiButton2).gameObject.SetActive(true);
                return uiButton2;
              }
              ((Component) uiButton1).gameObject.SetActive(true);
              ((Component) uiButton2).gameObject.SetActive(false);
              return uiButton1;
            })
          }
        }, (Action) (() =>
        {
          Persist.pvpInfo.Data.currentPage = 2;
          Persist.pvpInfo.Flush();
          if (this.isRepair)
            this.IbtnRepair();
          else
            this.IbtnStartMatch();
        }));
      else if (Persist.pvpInfo.Data.currentPage == 3 && (type == PvpMatchingTypeEnum.challenge || type == PvpMatchingTypeEnum.normal))
        Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton("pvp4", new Dictionary<string, Func<Transform, UIButton>>()
        {
          {
            "versus_random8",
            (Func<Transform, UIButton>) (root => ((Component) root.GetChildInFind("Top")).GetComponentInChildren<UIButton>())
          }
        }, (Action) (() =>
        {
          Persist.pvpInfo.Data.currentPage = 4;
          Persist.pvpInfo.Flush();
          this.IbtnBack();
        }));
      else if (Persist.pvpInfo.Data.currentPage == 5 && versus0262Menu.isFriendMatch)
        Singleton<TutorialRoot>.GetInstance().ForceShowAdvice("pvp6", (Action) (() =>
        {
          this.StartCoroutine(this.PvpTutorialProgressFinish());
          Persist.pvpInfo.Data.currentPage = Versus026MatchBase.PVP_TUTORIAL_FRIEND_END_PAGE;
          Persist.pvpInfo.Flush();
        }));
    }
    if (pvpInfo.is_tutorial && Persist.pvpInfo.Data.currentPage >= Versus026MatchBase.PVP_TUTORIAL_FRIEND_END_PAGE)
      yield return (object) versus0262Menu.PvpTutorialProgressFinish();
    Persist.pvpInfo.Data.lastMatchingType = type;
    Persist.pvpInfo.Flush();
  }

  private IEnumerator PvpTutorialProgressFinish()
  {
    Future<WebAPI.Response.PvpTutorialProgressFinish> futureF = WebAPI.PvpTutorialProgressFinish((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.PvpTutorialProgressFinish result = futureF.Result;
  }

  public void InitDisplayType()
  {
    bool isFriendMatch = this.isFriendMatch;
    Consts instance = Consts.GetInstance();
    this.txtTitle.text = isFriendMatch ? instance.VERSUS_00262TITLE_FRIEND : instance.VERSUS_00262TITLE_RANDOM;
    this.randomEnemyStatus.SetActive(!isFriendMatch);
    this.friendEnemyStatus.SetActive(isFriendMatch);
    if (!isFriendMatch)
    {
      this.OnClickNormal();
    }
    else
    {
      this.type = PvpMatchingTypeEnum.guest;
      this.btnFriendOn.SetActive(false);
      this.hasFriend = this.pvpInfo.has_friends;
      ((UIButtonColor) this.btnFriendOff).isEnabled = this.hasFriend;
    }
  }

  public void OnClickFriend()
  {
    if (!this.hasFriend)
      return;
    this.btnFriendOn.SetActive(!this.btnFriendOn.activeSelf);
    this.type = this.btnFriendOn.activeSelf ? PvpMatchingTypeEnum.friend : PvpMatchingTypeEnum.guest;
  }

  public void OnClickNormal()
  {
    if (this.IsPush)
      return;
    this.ChangeStrengthCondition(0);
    this.type = PvpMatchingTypeEnum.normal;
  }

  public void OnClickChallenge()
  {
    if (this.IsPush)
      return;
    this.ChangeStrengthCondition(1);
    this.type = PvpMatchingTypeEnum.challenge;
  }

  private void ChangeStrengthCondition(int index)
  {
    ((IEnumerable<GameObject>) this.btnStrengthOn).ForEachIndex<GameObject>((Action<GameObject, int>) ((x, i) => x.SetActive(i == index)));
  }

  public override void IbtnWarExperience()
  {
    if (this.IsPushAndSet())
      return;
    base.IbtnWarExperience();
    Versus02622Scene.ChangeScene(true, this.pvpInfo.pvp_record, this.pvpInfo.pvp_record_by_friend);
  }

  public void CheckRoomkey()
  {
    if (!Regex.IsMatch(this.txtPass.text, "[^0-9]"))
      return;
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.VERSUS_00262POPUP_TITLE, instance.VERSUS_00262POPUP_DESCRIPTION, (Action) (() => { }));
    ((Component) this.txtPass).GetComponent<UIInput>().value = "";
    this.txtPass.text = "";
  }

  protected override string SetRoomKey(string key)
  {
    if (this.isFriendMatch)
      key += this.txtPass.text;
    Debug.Log((object) ("===roomkey: " + key));
    return key;
  }

  public enum EnemyStrength
  {
    NORMAL,
    CHALLENGE,
  }
}
