// Decompiled with JetBrains decompiler
// Type: PopupEditDeckName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CustomDeck/Popup/EditDeckName")]
public class PopupEditDeckName : BackButtonPopupBase
{
  [SerializeField]
  private UIInput input_;
  private bool disabledAutoClose_;
  private UILabel label_;
  private PlayerCustomDeck target_;
  private Action<string> onSetName_;
  private Action onClose_;
  private const int MAX_DECKNAME = 10;

  public static Future<GameObject> createPrefabLoader()
  {
    return new ResourceObject("Prefabs/quest002_8/popup_TeamName_Edit").Load<GameObject>();
  }

  public static GameObject show(
    GameObject prefab,
    string text,
    int maxLength,
    Action<string> eventSet,
    Action eventClose)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab);
    gameObject.GetComponent<PopupEditDeckName>().initialize(text, maxLength, eventSet, eventClose);
    return gameObject;
  }

  public static GameObject show(GameObject prefab, UILabel uiLabel, PlayerCustomDeck target)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab);
    gameObject.GetComponent<PopupEditDeckName>().initialize(uiLabel, target);
    return gameObject;
  }

  private void initialize(string text, int maxLength, Action<string> eventSet, Action eventClose)
  {
    this.setTopObject(((Component) this).gameObject);
    this.onSetName_ = eventSet;
    this.onClose_ = eventClose;
    this.input_.caretColor = Color.black;
    this.input_.characterLimit = maxLength;
    this.input_.defaultText = text;
    this.input_.value = text;
    // ISSUE: method pointer
    this.input_.onValidate = new UIInput.OnValidate((object) this, __methodptr(onValidate));
  }

  private void initialize(UILabel uiLabel, PlayerCustomDeck target)
  {
    this.label_ = uiLabel;
    this.target_ = target;
    this.disabledAutoClose_ = true;
    this.initialize(target.name, 10, new Action<string>(this.defSet), new Action(this.defClose));
  }

  private char onValidate(string text, int charIndex, char addedChar)
  {
    return addedChar.IsInvalidInput() ? char.MinValue : addedChar;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.onClose_();
  }

  public void onClickedOk()
  {
    if (this.IsPushAndSet())
      return;
    string str = this.input_.value.ToConverter();
    if (string.IsNullOrEmpty(str))
      str = this.input_.defaultText;
    if (str == this.input_.defaultText)
    {
      this.IsPush = false;
      this.onBackButton();
    }
    else
    {
      if (!this.disabledAutoClose_)
        Singleton<PopupManager>.GetInstance().dismiss();
      this.onSetName_(str);
    }
  }

  private void defClose()
  {
  }

  private void defSet(string text)
  {
    this.StartCoroutine(PopupEditDeckName.doSaveDeckName(this.label_, this.target_, text));
  }

  private static IEnumerator doSaveDeckName(
    UILabel uiLabel,
    PlayerCustomDeck target,
    string nextName)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    Future<WebAPI.Response.DeckEditCustomDeckName> wApi = WebAPI.DeckEditCustomDeckName(target.deck_type_id, nextName, target.deck_number);
    IEnumerator e = wApi.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<PopupManager>.GetInstance().dismiss();
    if (wApi.Result != null)
    {
      target.name = nextName;
      uiLabel.SetTextLocalize(nextName);
    }
  }
}
