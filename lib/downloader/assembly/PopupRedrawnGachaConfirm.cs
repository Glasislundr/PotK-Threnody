// Decompiled with JetBrains decompiler
// Type: PopupRedrawnGachaConfirm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupRedrawnGachaConfirm : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel txt_remain_chance;
  [SerializeField]
  private UILabel txt_remain_time;
  [SerializeField]
  private GameObject slc_base_flame;
  [SerializeField]
  private UIButton ibtn_redrawn;
  private bool isPush;
  private Gacha00613Menu gachaMenu;
  private Action onDecide;
  private Action onRetry;
  private Action onClose;
  private DateTime? expiredAt;

  private bool isPushAndSet()
  {
    if (this.isPush)
      return true;
    this.isPush = true;
    return false;
  }

  public IEnumerator Initialize(
    Gacha00613Menu menu,
    Action decide,
    Action retry,
    Action close,
    int? retry_count = null,
    DateTime? expired_at = null)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    PopupRedrawnGachaConfirm redrawnGachaConfirm = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (Object.op_Inequality((Object) ((Component) redrawnGachaConfirm).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) redrawnGachaConfirm).GetComponent<UIWidget>()).alpha = 0.0f;
    redrawnGachaConfirm.gachaMenu = menu;
    redrawnGachaConfirm.onDecide = decide;
    redrawnGachaConfirm.onRetry = retry;
    redrawnGachaConfirm.onClose = close;
    redrawnGachaConfirm.expiredAt = expired_at;
    int? nullable = retry_count;
    if (nullable.HasValue)
    {
      if (nullable.GetValueOrDefault() == 0)
      {
        ((Component) redrawnGachaConfirm.txt_remain_time).gameObject.SetActive(false);
        redrawnGachaConfirm.slc_base_flame.gameObject.SetActive(false);
        redrawnGachaConfirm.txt_remain_chance.SetTextLocalize(Consts.GetInstance().GACHA_RETRY_LIMIT);
        ((UIButtonColor) redrawnGachaConfirm.ibtn_redrawn).isEnabled = false;
      }
      else if (!expired_at.HasValue)
      {
        ((Component) redrawnGachaConfirm.txt_remain_time).gameObject.SetActive(false);
        redrawnGachaConfirm.slc_base_flame.gameObject.SetActive(false);
        redrawnGachaConfirm.txt_remain_chance.SetTextLocalize(Consts.Format(Consts.GetInstance().GACHA_RETRY_REMAIN, (IDictionary) new Hashtable()
        {
          {
            (object) nameof (retry_count),
            (object) retry_count.ToString()
          }
        }));
      }
      else
        redrawnGachaConfirm.txt_remain_chance.SetTextLocalize(Consts.Format(Consts.GetInstance().GACHA_RETRY_REMAIN, (IDictionary) new Hashtable()
        {
          {
            (object) nameof (retry_count),
            (object) retry_count.ToString()
          }
        }));
    }
    else
      redrawnGachaConfirm.txt_remain_chance.SetTextLocalize(Consts.GetInstance().GACHA_RETRY_INFINITY);
    return false;
  }

  public IEnumerator UpdateRemainTime()
  {
    if (this.expiredAt.HasValue)
    {
      while (true)
      {
        TimeSpan timeSpan = this.expiredAt.Value - ServerTime.NowAppTimeAddDelta();
        string str;
        if ((int) timeSpan.TotalHours >= 1)
          str = string.Format("{0}時間{1}分", (object) (int) timeSpan.TotalHours, (object) timeSpan.Minutes);
        else if ((int) timeSpan.TotalMinutes >= 1)
          str = string.Format("{0}分", (object) timeSpan.Minutes);
        else if ((int) timeSpan.TotalSeconds >= 1)
          str = string.Format("{0}秒", (object) timeSpan.Seconds);
        else
          break;
        this.txt_remain_time.SetTextLocalize(string.Format("確定まで{0}", (object) str));
        yield return (object) new WaitForSeconds(1f);
      }
      ((Component) this.txt_remain_time).gameObject.SetActive(false);
      this.slc_base_flame.gameObject.SetActive(false);
      this.txt_remain_chance.SetTextLocalize(Consts.GetInstance().GACHA_RETRY_LIMIT);
      ((UIButtonColor) this.ibtn_redrawn).isEnabled = false;
    }
  }

  public void onRetryButton()
  {
    if (this.isPushAndSet() || this.onRetry == null)
      return;
    ((Component) this).gameObject.SetActive(false);
    this.onRetry();
  }

  public void onDecideButton()
  {
    if (this.isPushAndSet())
      return;
    if (this.onDecide != null)
      this.onDecide();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton()
  {
    if (this.isPushAndSet())
      return;
    if (this.onClose != null)
      this.onClose();
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
