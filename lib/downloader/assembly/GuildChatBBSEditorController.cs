// Decompiled with JetBrains decompiler
// Type: GuildChatBBSEditorController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildChatBBSEditorController : MonoBehaviour
{
  [SerializeField]
  private UILabel textMessage;
  [SerializeField]
  private UIInput inputField;
  [SerializeField]
  private UILabel characterCountLabel;
  [SerializeField]
  private UIButton confirmButton;
  private GuildChatManager guildChatManager;
  private bool isSendingBBSContent;
  private Action onConfirmCallback;

  private void Start() => this.inputField.isSelected = true;

  private void Update()
  {
    if (this.textMessage.text.Length < this.inputField.characterLimit + 1)
      return;
    ((UIButtonColor) this.confirmButton).isEnabled = false;
  }

  public void InitializeBBSEditorDialog(Action confirmCallback = null)
  {
    this.onConfirmCallback = confirmCallback;
    this.guildChatManager = Singleton<CommonRoot>.GetInstance().guildChatManager;
    this.inputField.value = PlayerAffiliation.Current.guild.private_message;
    this.isSendingBBSContent = false;
  }

  public void OnBBSEditorCancelButtonClicked()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    this.guildChatManager.OpenBBSViewerDialog();
  }

  public void OnBBSEditorConfirmButtonClicked()
  {
    if (this.isSendingBBSContent)
      return;
    this.isSendingBBSContent = true;
    this.StartCoroutine(this.UpdateBBSMessage());
  }

  private IEnumerator UpdateBBSMessage()
  {
    GuildChatBBSEditorController editorController = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    editorController.textMessage.SetTextLocalize(editorController.inputField.value);
    Future<WebAPI.Response.GuildPrivateMessage> future = WebAPI.GuildPrivateMessage(editorController.inputField.value.ToConverter(), false, new Action<WebAPI.Response.UserError>(editorController.ErrorCallback));
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (future.Result != null)
    {
      if (editorController.onConfirmCallback != null)
      {
        editorController.onConfirmCallback();
        editorController.onConfirmCallback = (Action) null;
      }
      Singleton<PopupManager>.GetInstance().dismiss();
      editorController.StartCoroutine(editorController.guildChatManager.OpenBBSViewerDialogCoroutine());
    }
  }

  private void ErrorCallback(WebAPI.Response.UserError error)
  {
    this.isSendingBBSContent = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (error.Code == "GLD011")
      this.StartCoroutine(this.OpenNGWordDialog());
    else if (error.Code == "GLD015")
    {
      Singleton<PopupManager>.GetInstance().dismiss();
      Singleton<CommonRoot>.GetInstance().guildChatManager.StartMaintenanceMode();
    }
    else
      WebAPI.DefaultUserErrorCallback(error);
  }

  private IEnumerator OpenNGWordDialog()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_028_guild_ng_word__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Guild028NgWordPopup>().Initialize((Action) (() => { }));
  }

  public void OnInputContentChanged()
  {
    this.characterCountLabel.SetTextLocalize(this.textMessage.text.Length.ToString() + "/" + (object) this.inputField.characterLimit);
    ((UIButtonColor) this.confirmButton).isEnabled = !this.inputField.value.isEmptyOrWhitespace();
  }
}
