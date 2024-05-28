// Decompiled with JetBrains decompiler
// Type: CommonSeaHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CommonSeaHeader : CommonHeaderBase
{
  [SerializeField]
  private UILabel zenyLable;
  [SerializeField]
  private UILabel kisekiLable;
  [SerializeField]
  private GameObject kisekiBikkuriIcon;
  [SerializeField]
  private UILabel playerNameLable;
  [SerializeField]
  private UILabel playerLevelLable;
  [SerializeField]
  private NGTweenGaugeFillAmount playerLevelProgressGauge;
  [SerializeField]
  private NGTweenGaugeWidth apGauge;
  [SerializeField]
  private UILabel apLabel;
  [SerializeField]
  private GameObject[] dpObjects;
  [SerializeField]
  private GameObject menuObject;
  [SerializeField]
  private GameObject badgeTalkSea;
  [SerializeField]
  private GameObject badgeTalkSea2;
  private Modified<SeaPlayer> seaPlayer;
  private bool menuOpenFromTalk;
  private bool isMenuActive;

  private void Start()
  {
    this.Init();
    this.seaPlayer = SMManager.Observe<SeaPlayer>();
    this.SetPlayerParameter(this.player.Value, false);
    this.SetSeaPlayerParameter(this.seaPlayer.Value);
  }

  protected override void Update()
  {
    base.Update();
    if (this.isChangedOncePlayer)
      this.SetPlayerParameter(this.player.Value, true);
    if (this.seaPlayer.IsChangedOnce())
      this.SetSeaPlayerParameter(this.seaPlayer.Value);
    this.badgeTalkSea.SetActive(Singleton<NGGameDataManager>.GetInstance().IsTalkBadgeOn());
    this.badgeTalkSea2.SetActive(Singleton<NGGameDataManager>.GetInstance().IsTalkBadgeOn());
  }

  private void SetPlayerParameter(Player player, bool doTween)
  {
    this.playerNameLable.SetTextLocalize(player.name);
    this.playerLevelLable.SetTextLocalize(player.level);
    this.playerLevelProgressGauge.setValue(player.exp, player.exp_next + player.exp, doTween, -1f, -1f);
    this.apGauge.setValue(Mathf.Min(player.ap, player.ap_max), player.ap_max, doTween);
    if (player.ap + player.ap_overflow > player.ap_max)
      this.apLabel.SetTextLocalize(string.Format("[f6ff01]{0}[-]/{1}", (object) (player.ap + player.ap_overflow), (object) player.ap_max));
    else
      this.apLabel.SetTextLocalize(string.Format("{0}/{1}", (object) (player.ap + player.ap_overflow), (object) player.ap_max));
    this.zenyLable.SetTextLocalize(player.money);
    this.kisekiLable.SetTextLocalize(player.coin);
    this.UpdateHeaderBikkuriIcon();
  }

  public void UpdateHeaderBikkuriIcon()
  {
    this.kisekiBikkuriIcon.SetActive(Singleton<NGGameDataManager>.GetInstance().receivableGift);
  }

  private void SetSeaPlayerParameter(SeaPlayer player)
  {
    ((IEnumerable<GameObject>) this.dpObjects).ForEachIndex<GameObject>((Action<GameObject, int>) ((g, i) =>
    {
      if (Object.op_Inequality((Object) g, (Object) null) && player != null)
        g.SetActive(i < player.dp);
      else
        g.SetActive(false);
    }));
  }

  public void OnFiniseMenuTween()
  {
    if (this.menuObject.activeSelf != this.isMenuActive)
      this.menuObject.SetActive(this.isMenuActive);
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (Object.op_Inequality((Object) instance.sceneBase, (Object) null))
      instance.sceneBase.IsPush = false;
    if (this.menuObject.activeSelf || !this.menuOpenFromTalk)
      return;
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    this.menuOpenFromTalk = false;
  }

  private new bool IsPushAndSet()
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null) && instance.sceneBase.IsPush)
      return true;
    instance.sceneBase.IsPush = true;
    return false;
  }

  public void OnMenuButton()
  {
    if (this.IsPushAndSet())
      return;
    if (!this.menuObject.activeSelf)
      this.OpenMenu();
    else
      this.CloseMenu();
  }

  public void OpenMenu()
  {
    if (this.menuObject.activeSelf)
      return;
    this.isMenuActive = true;
    this.menuObject.SetActive(true);
    NGTween.playTweens(this.menuObject.GetComponentsInChildren<UITweener>(true), 3000);
    Singleton<CommonRoot>.GetInstance().isActive3DUIMask = true;
    Singleton<CommonRoot>.GetInstance().isSeaGlobalMenuOpen = true;
  }

  public void OpenMenuForTalk()
  {
    this.menuOpenFromTalk = true;
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    this.OpenMenu();
  }

  public void CloseMenu()
  {
    if (!this.menuObject.activeSelf)
      return;
    this.isMenuActive = false;
    NGTween.playTweens(this.menuObject.GetComponentsInChildren<UITweener>(true), 3000, true);
    Singleton<CommonRoot>.GetInstance().isActive3DUIMask = false;
    Singleton<CommonRoot>.GetInstance().isSeaGlobalMenuOpen = false;
  }
}
