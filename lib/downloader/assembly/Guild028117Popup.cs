// Decompiled with JetBrains decompiler
// Type: Guild028117Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild028117Popup : BackButtonMenuBase
{
  private GuildInfoPopup guildPopup;
  private bool ngWord;
  private bool maintenance;
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel maxWord;
  private int statementCount;
  [SerializeField]
  private UILabel statement;
  [SerializeField]
  private UIInput input;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private UIWidget scrollContainerWidget;
  [SerializeField]
  private UIWidget dragScrollViewWidget;
  [SerializeField]
  private SpreadColorButton decideButton;
  private BoxCollider inputFieldCollision;
  private BoxCollider dragScrollCollision;

  private void Start() => this.input.isSelected = true;

  public void Initialize(GuildInfoPopup popup)
  {
    this.guildPopup = popup;
    this.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_7_TITLE));
    this.statement.SetTextLocalize(PlayerAffiliation.Current.guild.appearance.broadcast_message);
    this.input.value = PlayerAffiliation.Current.guild.appearance.broadcast_message.ToConverter();
    this.maxWord.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_7_STATEMENT_LIMIT, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) this.input.value.Length
      },
      {
        (object) "max",
        (object) this.input.characterLimit
      }
    }));
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component, (Object) null))
      ((UIRect) component).alpha = 0.0f;
    this.dragScrollCollision = ((Component) this.dragScrollViewWidget).GetComponent<BoxCollider>();
    ((Component) this.input).gameObject.AddComponent<UIDragScrollView>();
    this.inputFieldCollision = ((Component) this.input).gameObject.AddComponent<BoxCollider>();
    this.UpdateInputFieldDragCollision();
  }

  public void Initialize(GuildDirectory guild, GuildInfoPopup popup)
  {
    this.guildPopup = popup;
    this.ngWord = false;
    this.maintenance = false;
    this.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_7_TITLE));
  }

  private IEnumerator SendGuildStatement()
  {
    Guild028117Popup guild028117Popup = this;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.GuildBroadcastMessage> ft = WebAPI.GuildBroadcastMessage(guild028117Popup.input.value.ToConverter(), false, new Action<WebAPI.Response.UserError>(guild028117Popup.ErrorCallback));
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (guild028117Popup.maintenance)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    else if (ft.Result != null && !guild028117Popup.ngWord)
      Singleton<PopupManager>.GetInstance().open(guild028117Popup.guildPopup.guildInfoPopup).GetComponent<Guild028114Popup>().Initialize(guild028117Popup.guildPopup);
  }

  private void ErrorCallback(WebAPI.Response.UserError error)
  {
    if (error.Code.Equals("GLD011"))
    {
      this.ngWord = true;
      Singleton<PopupManager>.GetInstance().open(this.guildPopup.guildNgWordPopup).GetComponent<Guild028NgWordPopup>().Initialize((Action) (() => { }));
    }
    else if (error.Code.Equals("GLD014"))
    {
      this.maintenance = true;
      WebAPI.DefaultUserErrorCallback(error);
    }
    else
      WebAPI.DefaultUserErrorCallback(error);
  }

  public void UpdateDrawScrollViewAnchor()
  {
    if (((Component) this.statement).GetComponent<UIWidget>().height > this.scrollContainerWidget.height)
    {
      ((UIRect) this.dragScrollViewWidget).bottomAnchor.target = ((Component) this.statement).transform;
      ((UIRect) this.dragScrollViewWidget).bottomAnchor.rect = (UIRect) this.statement;
      ((UIRect) this.dragScrollViewWidget).bottomAnchor.relative = 0.0f;
      ((UIRect) this.dragScrollViewWidget).UpdateAnchors();
    }
    else
    {
      ((UIRect) this.dragScrollViewWidget).bottomAnchor.target = (Transform) null;
      this.dragScrollViewWidget.SetDimensions((int) this.scrollContainerWidget.localSize.x, (int) this.scrollContainerWidget.localSize.y);
    }
  }

  public void UpdateDragScrollViewBoxCollider()
  {
    this.dragScrollCollision.size = new Vector3(this.dragScrollCollision.size.x, this.dragScrollViewWidget.localSize.y);
    this.dragScrollCollision.center = new Vector3(this.dragScrollCollision.center.x, (float) (-(double) this.dragScrollViewWidget.localSize.y * 0.5));
    this.UpdateInputFieldDragCollision();
  }

  private void UpdateInputFieldDragCollision()
  {
    this.inputFieldCollision.size = this.dragScrollCollision.size;
    this.inputFieldCollision.center = this.dragScrollCollision.center;
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onDecideButton()
  {
    this.statement.SetTextLocalize(this.input.value);
    this.StartCoroutine(this.SendGuildStatement());
  }

  public void onChangeStatement()
  {
    if (!Object.op_Inequality((Object) ((Component) this.input).GetComponent<UILabel>(), (Object) null))
      return;
    this.maxWord.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_7_STATEMENT_LIMIT, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) this.input.value.Length
      },
      {
        (object) "max",
        (object) this.input.characterLimit
      }
    }));
    this.UpdateDrawScrollViewAnchor();
    this.scrollView.UpdatePosition();
    ((UIButtonColor) this.decideButton).isEnabled = !this.input.value.isEmptyOrWhitespace();
    if (!string.IsNullOrEmpty(this.statement.text))
      return;
    this.input.defaultText = string.Empty;
  }

  public void ResetScrollPosition() => this.scrollView.ResetPosition();

  public void EditButton() => this.input.isSelected = !this.input.isSelected;
}
