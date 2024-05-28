// Decompiled with JetBrains decompiler
// Type: MyPageCommonMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class MyPageCommonMenu : MyPageSubMenu
{
  [Header("Button Controller")]
  [SerializeField]
  private CommonGuildButton mGuildButton;
  [SerializeField]
  private MypageEventButtonController mSideSubButtonController;
  [SerializeField]
  private RaidShopButton mRaidShopBtn;
  [Header("Guild Name")]
  [SerializeField]
  private GameObject mGuildName;
  [SerializeField]
  private GameObject mNoGuildName;
  [SerializeField]
  private UILabel mGuildNameLbl;
  [Header("Bonus Infomation")]
  [SerializeField]
  private UILabel[] mBonusLbls;
  [SerializeField]
  private UILabel mPlayerExpBonusValueLbl;
  [SerializeField]
  private UILabel mUnitExpBonusValueLbl;
  [SerializeField]
  private UILabel mDropBonusValueLbl;
  [SerializeField]
  private UILabel mZenyBonusValueLbl;
  [SerializeField]
  private Color mValidColor;
  [SerializeField]
  private Color mInvalidColor;

  public IEnumerator InitSceneAsync(MypageRootMenu rootMenu)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    MyPageCommonMenu myPageCommonMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    myPageCommonMenu.RootMenu = rootMenu;
    myPageCommonMenu.SoundMgr = Singleton<NGSoundManager>.GetInstance();
    return false;
  }

  public IEnumerator OnStartSceneAsync()
  {
    MyPageCommonMenu myPageCommonMenu = this;
    PlayerAffiliation current = PlayerAffiliation.Current;
    if (current != null && current.isGuildMember())
    {
      GuildRegistration guild = current.guild;
      myPageCommonMenu.mGuildName.SetActive(true);
      myPageCommonMenu.mNoGuildName.SetActive(false);
      myPageCommonMenu.mGuildNameLbl.SetTextLocalize(guild.guild_name);
      myPageCommonMenu.UpdateBonusInfo(guild.level_bonus);
    }
    else
    {
      myPageCommonMenu.mGuildName.SetActive(false);
      myPageCommonMenu.mNoGuildName.SetActive(true);
      myPageCommonMenu.SetInvalidBonusInfo();
    }
    IEnumerator e = myPageCommonMenu.mGuildButton.UpdateButtonState(myPageCommonMenu.RootMenu.GuildTopResponse);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    myPageCommonMenu.mSideSubButtonController.UpdateButtonState();
    myPageCommonMenu.UpdateRaidShopButtonState();
  }

  public override void OnModeSwitch(MypageRootMenu.Mode mode)
  {
    if (mode != MypageRootMenu.Mode.GUILD)
      return;
    this.UpdateRaidShopButtonState();
  }

  public override void OnModeSwitched(MypageRootMenu.Mode mode)
  {
    if (mode != MypageRootMenu.Mode.STORY)
      return;
    this.UpdateRaidShopButtonState();
  }

  private void UpdateBonusInfo(GuildLevelBonus bonus)
  {
    this.mPlayerExpBonusValueLbl.SetTextLocalize(bonus.player_exp);
    this.mUnitExpBonusValueLbl.SetTextLocalize(bonus.unit_exp);
    this.mDropBonusValueLbl.SetTextLocalize(bonus.item);
    this.mZenyBonusValueLbl.SetTextLocalize(bonus.money);
    foreach (UIWidget mBonusLbl in this.mBonusLbls)
      mBonusLbl.color = this.mValidColor;
  }

  private void SetInvalidBonusInfo()
  {
    this.mPlayerExpBonusValueLbl.SetTextLocalize("-");
    this.mUnitExpBonusValueLbl.SetTextLocalize("-");
    this.mDropBonusValueLbl.SetTextLocalize("-");
    this.mZenyBonusValueLbl.SetTextLocalize("-");
    foreach (UIWidget mBonusLbl in this.mBonusLbls)
      mBonusLbl.color = this.mInvalidColor;
  }

  public void UpdateRaidShopButtonState() => this.mRaidShopBtn.UpdateButtonState();
}
