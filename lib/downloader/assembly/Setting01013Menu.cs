// Decompiled with JetBrains decompiler
// Type: Setting01013Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Setting01013Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel InpId01;
  [SerializeField]
  protected UILabel README;
  [SerializeField]
  protected UILabel TxtDescription;
  [SerializeField]
  protected UILabel TxtPopuptitle;
  [SerializeField]
  protected UILabel TxtTitle;

  private void Start()
  {
    ((Component) this.InpId01).GetComponent<UIInput>().caretColor = Color.black;
  }

  public void Initialize()
  {
    Player player = SMManager.Get<Player>();
    this.InpId01.SetTextLocalize(player.name);
    ((Component) this.InpId01).GetComponent<UIInput>().value = player.name;
    ((Component) this.InpId01).GetComponent<UIInput>().value = player.name;
    ((Component) this.InpId01).GetComponent<UIInput>().defaultText = player.name;
    // ISSUE: method pointer
    ((Component) this.InpId01).GetComponent<UIInput>().onValidate = new UIInput.OnValidate((object) this, __methodptr(onValidate));
  }

  private char onValidate(string text, int charIndex, char addedChar)
  {
    bool flag = char.IsControl(addedChar) || addedChar >= '\uE000' && addedChar <= '\uF8FF';
    Debug.Log((object) (((int) addedChar).ToString() + ":" + flag.ToString()));
    return flag ? char.MinValue : addedChar;
  }

  public void onChangeInput()
  {
    int num = this.IsPush ? 1 : 0;
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().backScene();
  }

  private IEnumerator NameEdit()
  {
    Setting01013Menu menu = this;
    menu.InpId01.SetText(menu.InpId01.text.ToConverter());
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_010_1_5__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Popup01015Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup01015Menu>();
    menu.StartCoroutine(component.Init(menu, menu.InpId01.text));
  }

  public virtual void IbtnPopupOk()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.NameEdit());
  }

  public void ErrDialog() => this.alert(Consts.GetInstance().tutorial_fail_player_name);

  private void alert(string message)
  {
    Singleton<TutorialRoot>.GetInstance().showAdvice("#character 6\n#background black\n" + message);
  }
}
