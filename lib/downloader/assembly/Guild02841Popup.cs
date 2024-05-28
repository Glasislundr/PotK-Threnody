// Decompiled with JetBrains decompiler
// Type: Guild02841Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild02841Popup : BackButtonMenuBase
{
  private Action act;
  private int titleID;
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel description;
  [SerializeField]
  private UI2DSprite titleImage;
  [SerializeField]
  private GameObject objUnknown;
  [SerializeField]
  private GameObject btnSetTitle;
  [SerializeField]
  private GameObject btnRemoveTitle;
  [SerializeField]
  private GameObject btnClose;
  [SerializeField]
  private GameObject slc_Popupbox;
  [Header("Dlg size")]
  [SerializeField]
  private Vector2 standardDlgSize;
  [SerializeField]
  private Vector2 noDecideBtnDlgSize;

  public IEnumerator Initialize(int id, string txt, bool hasEmblem, bool isCur, Action act)
  {
    Guild02841Popup guild02841Popup = this;
    guild02841Popup.act = act;
    guild02841Popup.titleID = id;
    guild02841Popup.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_4_1_TITLE));
    if (Object.op_Inequality((Object) ((Component) guild02841Popup).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) guild02841Popup).GetComponent<UIWidget>()).alpha = 0.0f;
    guild02841Popup.description.SetText(txt);
    ((Component) guild02841Popup.titleImage).gameObject.SetActive(hasEmblem);
    guild02841Popup.objUnknown.SetActive(!hasEmblem);
    if (hasEmblem)
    {
      IEnumerator e = guild02841Popup.CreateSprite();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    float duration1 = ((UIButtonColor) guild02841Popup.btnSetTitle.GetComponent<UIButton>()).duration;
    float duration2 = ((UIButtonColor) guild02841Popup.btnRemoveTitle.GetComponent<UIButton>()).duration;
    ((UIButtonColor) guild02841Popup.btnSetTitle.GetComponent<UIButton>()).duration = 0.0f;
    ((UIButtonColor) guild02841Popup.btnRemoveTitle.GetComponent<UIButton>()).duration = 0.0f;
    GuildRole? role1 = PlayerAffiliation.Current.role;
    GuildRole guildRole1 = GuildRole.master;
    if (!(role1.GetValueOrDefault() == guildRole1 & role1.HasValue))
    {
      GuildRole? role2 = PlayerAffiliation.Current.role;
      GuildRole guildRole2 = GuildRole.sub_master;
      if (!(role2.GetValueOrDefault() == guildRole2 & role2.HasValue))
      {
        guild02841Popup.btnSetTitle.SetActive(false);
        guild02841Popup.btnRemoveTitle.SetActive(false);
        ((UIWidget) guild02841Popup.slc_Popupbox.GetComponent<UISprite>()).SetDimensions((int) guild02841Popup.noDecideBtnDlgSize.x, (int) guild02841Popup.noDecideBtnDlgSize.y);
        goto label_11;
      }
    }
    guild02841Popup.btnSetTitle.SetActive(!isCur);
    guild02841Popup.btnRemoveTitle.SetActive(isCur);
    ((UIWidget) guild02841Popup.slc_Popupbox.GetComponent<UISprite>()).SetDimensions((int) guild02841Popup.standardDlgSize.x, (int) guild02841Popup.standardDlgSize.y);
label_11:
    ((UIButtonColor) guild02841Popup.btnSetTitle.GetComponent<UIButton>()).isEnabled = hasEmblem;
    ((UIButtonColor) guild02841Popup.btnRemoveTitle.GetComponent<UIButton>()).isEnabled = hasEmblem;
    ((UIButtonColor) guild02841Popup.btnSetTitle.GetComponent<UIButton>()).duration = duration1;
    ((UIButtonColor) guild02841Popup.btnRemoveTitle.GetComponent<UIButton>()).duration = duration2;
  }

  private IEnumerator EmblemSetAPI(int ID)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.loadingMode = 1;
    common.isLoading = true;
    Future<WebAPI.Response.GuildEmblemSetting> future = WebAPI.GuildEmblemSetting(ID, (Action<WebAPI.Response.UserError>) (e =>
    {
      common.isLoading = false;
      common.loadingMode = 0;
      Singleton<PopupManager>.GetInstance().onDismiss();
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (future.Result != null)
    {
      if (this.act != null)
      {
        this.act();
      }
      else
      {
        common.isLoading = false;
        common.loadingMode = 0;
      }
    }
  }

  private IEnumerator EmblemUnsetAPI()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.loadingMode = 1;
    common.isLoading = true;
    Future<WebAPI.Response.GuildEmblemUnsetting> future = WebAPI.GuildEmblemUnsetting(this.titleID, (Action<WebAPI.Response.UserError>) (e =>
    {
      common.isLoading = false;
      common.loadingMode = 0;
      Singleton<PopupManager>.GetInstance().onDismiss();
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (future.Result != null)
    {
      if (this.act != null)
      {
        this.act();
      }
      else
      {
        common.isLoading = false;
        common.loadingMode = 0;
      }
    }
  }

  private IEnumerator CreateSprite()
  {
    Future<Sprite> sprF = EmblemUtility.LoadGuildEmblemSprite(this.titleID);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.titleImage.sprite2D = sprF.Result;
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onChangeTitleButton() => this.StartCoroutine(this.EmblemSetAPI(this.titleID));

  public void onRemoveButton() => this.StartCoroutine(this.EmblemUnsetAPI());
}
