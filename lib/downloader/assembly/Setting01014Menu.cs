// Decompiled with JetBrains decompiler
// Type: Setting01014Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Setting01014Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel InpId01;

  public void Initialize()
  {
    Player player = SMManager.Get<Player>();
    this.InpId01.SetTextLocalize(player.comment);
    ((Component) this.InpId01).GetComponent<UIInput>().defaultText = this.SetTextPlayerComment(player.comment);
    // ISSUE: method pointer
    ((Component) this.InpId01).GetComponent<UIInput>().onValidate = new UIInput.OnValidate((object) this, __methodptr(onValidate));
    ((Component) this.InpId01).GetComponent<UIInput>().value = this.SetTextPlayerComment(player.comment);
    ((Component) this.InpId01).GetComponent<UIInput>().caretColor = Color.black;
  }

  private string SetTextPlayerComment(string comment)
  {
    return comment == string.Empty ? Consts.GetInstance().FRIEND_COMMENT_DEFAULT : comment;
  }

  private char onValidate(string text, int charIndex, char addedChar)
  {
    bool flag = char.IsControl(addedChar) || addedChar >= '\uE000' && addedChar <= '\uF8FF';
    Debug.Log((object) (((int) addedChar).ToString() + ":" + flag.ToString()));
    return flag ? char.MinValue : addedChar;
  }

  public void onChangeInput()
  {
    if (this.IsPush)
      return;
    this.InpId01.SetTextLocalize(((Component) this.InpId01).GetComponent<UIInput>().value);
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().backScene();
  }

  private IEnumerator CommentEdit()
  {
    Setting01014Menu setting01014Menu = this;
    string newComment = setting01014Menu.InpId01.text;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.PlayerCommentEdit> result = WebAPI.PlayerCommentEdit(newComment, new Action<WebAPI.Response.UserError>(setting01014Menu.\u003CCommentEdit\u003Eb__7_0));
    IEnumerator e = result.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (result.Result != null)
    {
      setting01014Menu.StartCoroutine(PopupCommon.Show(Consts.GetInstance().SETTING_01014_POPUP_TITLE, Consts.Format(Consts.GetInstance().SETTING_01014_MESSAGE, (IDictionary) new Hashtable()
      {
        {
          (object) "comment",
          (object) newComment
        }
      })));
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }

  public void IbtnPopupOk() => this.StartCoroutine(this.CommentEdit());

  public void ErrDialog()
  {
    this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().SETTING_01014_POPUP_TITLE, Consts.Format(Consts.GetInstance().SETTING_01014_ERROR_MESSAGE)));
  }
}
