// Decompiled with JetBrains decompiler
// Type: Mypage00181ScrollContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Mypage00181ScrollContainer : MonoBehaviour
{
  private GameObject informationListPrefab;
  private GameObject informationListLPrefab;
  private GameObject informationListLImagePrefab;
  public NGxScrollMasonry Scroll;

  public void Init(GameObject textPrefab, GameObject bothPrefab, GameObject imagePrefab)
  {
    this.informationListPrefab = textPrefab;
    this.informationListLPrefab = bothPrefab;
    this.informationListLImagePrefab = imagePrefab;
  }

  private GameObject GetInfomationListPrefab(OfficialInformationArticle article)
  {
    if (article.title_img_url == "")
      return this.informationListPrefab;
    return article.summary == "" ? this.informationListLImagePrefab : this.informationListLPrefab;
  }

  public IEnumerator CreateScrollList(
    OfficialInformationArticle[] officialInfoList,
    NGMenuBase menu,
    GameObject tabBadge)
  {
    Mypage00181ScrollContainer mypage00181ScrollContainer = this;
    mypage00181ScrollContainer.Scroll = ((Component) mypage00181ScrollContainer).GetComponent<NGxScrollMasonry>();
    ((Component) mypage00181ScrollContainer.Scroll.Scroll).transform.Clear();
    mypage00181ScrollContainer.Scroll.Reset();
    ((Component) mypage00181ScrollContainer.Scroll.Scroll).gameObject.SetActive(false);
    ((Component) mypage00181ScrollContainer).gameObject.SetActive(false);
    OfficialInformationArticle[] informationArticleArray = officialInfoList;
    for (int index = 0; index < informationArticleArray.Length; ++index)
    {
      OfficialInformationArticle informationArticle = informationArticleArray[index];
      GameObject scroll_list = mypage00181ScrollContainer.GetInfomationListPrefab(informationArticle).Clone();
      IEnumerator e = scroll_list.GetComponent<Mypage00181ScrollParts>().Init(informationArticle, menu, tabBadge);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      mypage00181ScrollContainer.Scroll.Add(scroll_list);
      scroll_list = (GameObject) null;
    }
    informationArticleArray = (OfficialInformationArticle[]) null;
    ((Component) mypage00181ScrollContainer.Scroll.Scroll).gameObject.SetActive(true);
    ((Component) mypage00181ScrollContainer).gameObject.SetActive(true);
    mypage00181ScrollContainer.Scroll.ResolvePosition();
  }
}
