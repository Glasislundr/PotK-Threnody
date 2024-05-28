// Decompiled with JetBrains decompiler
// Type: TutorialInputNamePage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class TutorialInputNamePage : TutorialPageBase
{
  private const int max_name_length = 8;
  [SerializeField]
  private string defaultName = "phantom";
  [SerializeField]
  private string message;
  [SerializeField]
  private GameObject popup;
  [SerializeField]
  private UI2DSprite background;
  [SerializeField]
  private UIInput input;
  [SerializeField]
  private bool isLocalCheck;
  private bool isProcessing;
  private Future<WebAPI.Response.PlayerSignup> signeup;

  private void Start() => this.input.caretColor = Color.black;

  public override void Init(TutorialProgress progress_, TutorialAdvice advice_, GameObject root_)
  {
    base.Init(progress_, advice_, root_);
    // ISSUE: method pointer
    this.input.onValidate = new UIInput.OnValidate((object) this, __methodptr(onValidate));
  }

  public override void ReleaseResources()
  {
    if (!Object.op_Implicit((Object) this.background.sprite2D))
      return;
    Object.Destroy((Object) this.background.sprite2D);
    this.background.sprite2D = (Sprite) null;
  }

  private char onValidate(string text, int charIndex, char addedChar)
  {
    return !this.isLocalCheck || (char.IsControl(addedChar) ? 1 : (addedChar < '\uE000' ? 0 : (addedChar <= '\uF8FF' ? 1 : 0))) == 0 ? addedChar : char.MinValue;
  }

  public override IEnumerator Show()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    TutorialInputNamePage tutorialInputNamePage = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (Object.op_Equality((Object) tutorialInputNamePage.background.sprite2D, (Object) null))
    {
      // ISSUE: reference to a compiler-generated method
      Res.Prefabs.BackGround.plain.Load<Sprite>().RunOn<Sprite>((MonoBehaviour) Singleton<TutorialRoot>.GetInstance(), new Action<Sprite>(tutorialInputNamePage.\u003CShow\u003Eb__13_0));
    }
    // ISSUE: reference to a compiler-generated method
    tutorialInputNamePage.StartCoroutine(tutorialInputNamePage.\u003C\u003En__0());
    tutorialInputNamePage.isProcessing = false;
    tutorialInputNamePage.input.value = tutorialInputNamePage.defaultName;
    tutorialInputNamePage.input.characterLimit = 8;
    tutorialInputNamePage.popup.SetActive(true);
    return false;
  }

  public void OnOk()
  {
    if (this.isProcessing)
      return;
    this.isProcessing = true;
    string player_name = this.input.value;
    player_name = this.input.value.ToConverter();
    this.popup.SetActive(false);
    WebAPI.TutorialTutorialValid(player_name, (Action<WebAPI.Response.UserError>) (error =>
    {
      this.popup.SetActive(true);
      this.alert(error.Reason);
    })).RunOn<WebAPI.Response.TutorialTutorialValid>((MonoBehaviour) this, (Action<WebAPI.Response.TutorialTutorialValid>) (result =>
    {
      this.popup.SetActive(true);
      if (result.is_valid)
      {
        SMManager.Get<Player>().name = player_name;
        Persist.tutorial.Data.PlayerName = player_name;
        this.NextPage();
      }
      else
        this.alert(Consts.GetInstance().tutorial_fail_player_name);
    }));
  }

  private void alert(string message)
  {
    this.isProcessing = false;
    this.advice.SetMessage("#background black\n" + message);
    this.advice.Show();
  }

  private void Update()
  {
  }
}
