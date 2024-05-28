// Decompiled with JetBrains decompiler
// Type: Mypage00181ScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Mypage00181ScrollParts : MonoBehaviour
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtSummary;
  [SerializeField]
  private UILabel txtDate;
  [SerializeField]
  private UI2DSprite slcBanner;
  [SerializeField]
  private GameObject slcNew;
  [SerializeField]
  private GameObject slcPostscript;
  public OfficialInformationArticle article;
  private string strDate = "{0}/{1} {2}:{3}";
  private string nextSceneName = "mypage001_8_2";
  private int maxTitle = 12;
  private int maxSummary = 50;

  public IEnumerator Init(
    OfficialInformationArticle officialInfo,
    NGMenuBase menu,
    GameObject tabBadge)
  {
    this.article = officialInfo;
    this.txtTitle.SetTextLocalize(this.GetText(officialInfo.title, this.maxTitle));
    this.txtDate.SetTextLocalize(string.Format(this.strDate, (object) string.Format("{0:D2}", (object) officialInfo.published_at.Month), (object) string.Format("{0:D2}", (object) officialInfo.published_at.Day), (object) string.Format("{0:D2}", (object) officialInfo.published_at.Hour), (object) string.Format("{0:D2}", (object) officialInfo.published_at.Minute)));
    if (officialInfo.summary != "")
      this.txtSummary.SetTextLocalize(this.GetText(officialInfo.summary, this.maxSummary));
    if (officialInfo.title_img_url != "")
      this.SetData(officialInfo, menu);
    this.SetBadge(officialInfo, tabBadge);
    yield return (object) null;
  }

  private string GetText(string text, int maxText)
  {
    return text.Length > maxText ? text.Substring(0, maxText) : text;
  }

  private void SetData(OfficialInformationArticle info, NGMenuBase menu)
  {
    ((MonoBehaviour) menu).StartCoroutine(this.LoadBanner(info.title_img_url));
  }

  private IEnumerator LoadBanner(string img_url)
  {
    IEnumerator e = Singleton<NGGameDataManager>.GetInstance().GetWebImage(img_url, this.slcBanner);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetBadge(OfficialInformationArticle info, GameObject tabBadge)
  {
    int num = this.SetNewSprite(info, tabBadge) ? 1 : 0;
    bool unReadPostScript = Persist.infoUnRead.Data.GetUnReadPostScript(info);
    if (num == 0 && !unReadPostScript)
      this.slcPostscript.SetActive(true);
    else
      this.slcPostscript.SetActive(false);
  }

  public bool SetNewSprite(OfficialInformationArticle info, GameObject tabBadge)
  {
    try
    {
      if (Persist.infoUnRead.Data.GetUnRead(info))
      {
        this.slcNew.SetActive(false);
        return false;
      }
    }
    catch
    {
      Persist.infoUnRead.Delete();
    }
    this.slcNew.SetActive(true);
    tabBadge.SetActive(true);
    return true;
  }

  public void IbtnNewslist()
  {
    Singleton<NGSoundManager>.GetInstance().stopVoice();
    Singleton<NGSceneManager>.GetInstance().changeScene(this.nextSceneName, true, (object) this.article);
  }
}
