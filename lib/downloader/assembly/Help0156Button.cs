// Decompiled with JetBrains decompiler
// Type: Help0156Button
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Help0156Button : MonoBehaviour
{
  [SerializeField]
  private UILabel txtHelp01;
  [SerializeField]
  private string[] answerPrefabs;
  private BeginnerNaviDetail detail;
  private GameObject popup;
  private GameObject questionPrefab;
  private GameObject answerPrefab;
  private Sprite descSprite;
  private BackButtonMenuBase _baseMenu;
  private string webLink;

  public void init(BackButtonMenuBase baseMenu) => this._baseMenu = baseMenu;

  public IEnumerator setTitleText(BeginnerNaviTitle bnTitle)
  {
    BeginnerNaviDetail bnDetail = bnTitle.detail;
    IEnumerator e;
    if (Object.op_Equality((Object) this.popup, (Object) null))
    {
      Future<GameObject> popupF = Res.Prefabs.help015_6.popup_015_6__anim_popup01.Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.popup = popupF.Result;
      Future<GameObject> questionPrefabF = Res.Prefabs.help015_6.dir_Question.Load<GameObject>();
      e = questionPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.questionPrefab = questionPrefabF.Result;
      int index = bnDetail.frameNumber - 1;
      Future<GameObject> answerPrefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/help015_6/" + this.answerPrefabs[index]);
      e = answerPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.answerPrefab = answerPrefabF.Result;
      popupF = (Future<GameObject>) null;
      questionPrefabF = (Future<GameObject>) null;
      answerPrefabF = (Future<GameObject>) null;
    }
    if (bnDetail.descriptionImage != "")
    {
      Future<Sprite> textureF = Singleton<ResourceManager>.GetInstance().Load<Sprite>("BeginnerNavi/" + bnDetail.descriptionImage);
      e = textureF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.descSprite = textureF.Result;
      textureF = (Future<Sprite>) null;
    }
    this.webLink = (string) null;
    string answerText = bnDetail.answerText;
    int num1 = answerText.IndexOf("<link=\"");
    if (num1 >= 0)
    {
      int num2 = answerText.IndexOf("\"/>");
      if (num2 > num1)
      {
        int startIndex = num1 + "<link=\"".Length;
        this.webLink = answerText.Substring(startIndex, num2 - startIndex);
      }
    }
    this.txtHelp01.SetTextLocalize(bnTitle.title);
    this.detail = bnDetail;
  }

  public void IbtnHelp()
  {
    if (!string.IsNullOrEmpty(this.webLink))
    {
      if (this._baseMenu.IsPush)
        return;
      this.toLink(this.webLink);
    }
    else
    {
      if (this._baseMenu.IsPushAndSet())
        return;
      Singleton<PopupManager>.GetInstance().open(this.popup).GetComponent<Popup0156Menu>().InitPopup(this.detail, this.descSprite, this.questionPrefab, this.answerPrefab);
    }
  }

  private void toLink(string url) => Application.OpenURL(url);
}
