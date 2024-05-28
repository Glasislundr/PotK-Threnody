// Decompiled with JetBrains decompiler
// Type: Mypage00181TabPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Mypage00181TabPage : MonoBehaviour
{
  [SerializeField]
  public GameObject scrollContainer;
  [SerializeField]
  private GameObject textON;
  [SerializeField]
  private GameObject textOFF;
  [SerializeField]
  public GameObject tabBadge;
  [SerializeField]
  private UIButton tabButton;
  [SerializeField]
  public Mypage00181Menu.CategoryType categoryType;
  public int category_id;
  private Mypage00181Menu NoticeMenu;

  public void Init(
    Mypage00181Menu menu,
    GameObject textPrefab,
    GameObject bothPrefab,
    GameObject imagePrefab)
  {
    this.NoticeMenu = menu;
    this.category_id = (int) this.categoryType;
    this.tabBadge.SetActive(false);
    this.scrollContainer.GetComponent<Mypage00181ScrollContainer>().Init(textPrefab, bothPrefab, imagePrefab);
  }

  public void OnPushTab() => this.NoticeMenu.OnPushPageTab(this.categoryType);

  public void SetPageActive(bool active)
  {
    this.SetButton(active);
    this.scrollContainer.gameObject.SetActive(active);
  }

  public void SetButton(bool active)
  {
    if (active)
    {
      this.textON.SetActive(true);
      this.textOFF.SetActive(false);
      ((UIButtonColor) this.tabButton).isEnabled = false;
    }
    else
    {
      this.textON.SetActive(false);
      this.textOFF.SetActive(true);
      ((UIButtonColor) this.tabButton).isEnabled = true;
    }
  }

  public IEnumerator CreateScroll(OfficialInformationArticle[] officialInfoList, NGMenuBase menu)
  {
    IEnumerator e = this.scrollContainer.GetComponent<Mypage00181ScrollContainer>().CreateScrollList(officialInfoList, menu, this.tabBadge);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
