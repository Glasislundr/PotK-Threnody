// Decompiled with JetBrains decompiler
// Type: Startup00014MakeupRewardIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Startup00014MakeupRewardIcon : MonoBehaviour
{
  [SerializeField]
  private Color EnableColor = Color32.op_Implicit(new Color32((byte) 128, (byte) 128, (byte) 128, byte.MaxValue));
  [SerializeField]
  private Color DisableColor = Color32.op_Implicit(new Color32((byte) 64, (byte) 64, (byte) 64, byte.MaxValue));
  [Space(8f)]
  [SerializeField]
  private GameObject mDayLblBase;
  [SerializeField]
  private UILabel mDayLbl;
  [SerializeField]
  private UISprite mIconBase;
  [SerializeField]
  private CreateIconObject mIconObject;
  [SerializeField]
  private GameObject mGetMark;
  [SerializeField]
  private GameObject mTodayMark;
  [SerializeField]
  private GameObject mNextMark;
  [SerializeField]
  private GameObject mMakeupButton;
  private Startup00014MakeupMonthly mParent;
  private LoginbonusReward mReward;

  public IEnumerator Initialize(
    Startup00014MakeupMonthly parent,
    LoginbonusReward reward,
    bool today,
    bool next,
    bool isGotten,
    bool canMakeup)
  {
    this.mParent = parent;
    this.mReward = reward;
    this.mDayLbl.SetTextLocalize(string.Format("{0}日目", (object) reward.number));
    IEnumerator e = this.mIconObject.CreateThumbnail(this.mReward.reward_type, this.mReward.reward_id, this.mReward.reward_quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mIconObject.SetDetailEvent(this.mReward.reward_type, this.mReward.reward_id, this.mReward.reward_quantity);
    this.SetGetMark(isGotten);
    this.SetTodayMark(today);
    this.mNextMark.SetActive(next);
    this.SetMakeupButton(canMakeup);
  }

  public void InitializeEmpty()
  {
    this.mDayLblBase.SetActive(false);
    ((Component) this.mDayLbl).gameObject.SetActive(false);
    ((UIWidget) this.mIconBase).color = this.DisableColor;
  }

  public void SetTodayMark(bool today) => this.mTodayMark.SetActive(today);

  public void SetGetMark(bool isGotten)
  {
    this.mGetMark.SetActive(isGotten);
    GameObject icon = this.mIconObject.GetIcon();
    if (Object.op_Inequality((Object) icon.GetComponent<UnitIcon>(), (Object) null))
      icon.GetComponent<UnitIcon>().Gray = isGotten;
    else if (Object.op_Inequality((Object) icon.GetComponent<ItemIcon>(), (Object) null))
      icon.GetComponent<ItemIcon>().Gray = isGotten;
    else if (Object.op_Inequality((Object) icon.GetComponent<UniqueIcons>(), (Object) null))
      icon.GetComponent<UniqueIcons>().Gray = isGotten;
    if (isGotten)
    {
      ((UIWidget) this.mIconBase).color = this.DisableColor;
      this.SetMakeupButton(false);
    }
    else
      ((UIWidget) this.mIconBase).color = this.EnableColor;
  }

  public void SetMakeupButton(bool enable) => this.mMakeupButton.SetActive(enable);

  public LoginbonusReward GetReward() => this.mReward;

  public void OnMakeupButton() => this.mParent.OnMakeupButton(this.mReward);
}
