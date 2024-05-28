// Decompiled with JetBrains decompiler
// Type: Friend00810Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Friend00810Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel InpFriendid;
  [SerializeField]
  protected UILabel TxtExplanation;
  [SerializeField]
  protected UILabel TxtId;
  [SerializeField]
  protected UILabel TxtMyid;
  [SerializeField]
  protected UILabel TxtREADME;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private UIInput InpUI;

  private IEnumerator BackSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Friend00810Menu friend00810Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    friend00810Menu.backScene();
    return false;
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnShareWithTwitter() => SocialListener.ShareWithTwitter();

  public virtual void IbtnBnr()
  {
    App.OpenUrl(Consts.GetInstance().INVITE_CAMPAIGN_BANNER_URL + SMManager.Get<Player>().short_id);
    Debug.Log((object) ("openURL:" + Consts.GetInstance().INVITE_CAMPAIGN_BANNER_URL + SMManager.Get<Player>().short_id));
  }

  public virtual void IbtnCopy()
  {
    if (this.IsPushAndSet())
      return;
    NGUITools.clipboard = SMManager.Get<Player>().short_id.ToString();
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.USERCODE_COPY_TITLE, instance.USERCODE_COPY, (Action) (() => this.IsPush = false));
  }

  private IEnumerator SearchUser(string short_id)
  {
    Friend00810Menu friend00810Menu = this;
    IEnumerator e;
    if (short_id.CompareTo(SMManager.Get<Player>().short_id) == 0)
    {
      e = friend00810Menu.openPopup00812();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.PlayerSearch> searchF = WebAPI.PlayerSearch(short_id, new Action<WebAPI.Response.UserError>(friend00810Menu.\u003CSearchUser\u003Eb__13_0));
      e = searchF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (searchF.Result != null)
      {
        if (searchF.HasResult)
        {
          Future<GameObject> prefabF = Res.Prefabs.friend008_11.popup_008_11__anim_popup01.Load<GameObject>();
          e = prefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GameObject result = prefabF.Result;
          e = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Friend00811Menu>().SetTargetUserData(searchF.Result);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          prefabF = (Future<GameObject>) null;
        }
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  public virtual void IbtnSearch()
  {
    if (this.InpUI.value.Length <= 0 || this.IsPushAndSet())
      return;
    this.setInpFriendid(this.InpUI.value);
    this.StartCoroutine(this.SearchUser(this.InpUI.value));
  }

  private IEnumerator openPopup00812()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_12__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().openAlert(prefab.Result);
  }

  public void setKeyboardTypeNumber()
  {
    this.InpUI.keyboardType = (UIInput.KeyboardType) 4;
    this.InpUI.characterLimit = 10;
  }

  private void setId(UILabel label, string id) => label.SetTextLocalize(id);

  public void setTxtId(string id) => this.setId(this.TxtId, id);

  public void setInpFriendid(string id) => this.setId(this.InpFriendid, id);

  public void onChangeInpFriendid()
  {
  }

  public void RestoreInputLabelForNonMobileDevices() => this.InpUI.label = this.InpFriendid;
}
