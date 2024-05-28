// Decompiled with JetBrains decompiler
// Type: SM.OfficialInformationArticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class OfficialInformationArticle : KeyCompare
  {
    public string title;
    public string title_img_url;
    public string summary;
    public int priority;
    public DateTime published_at;
    public int official_update_type;
    public int category_id;
    public int id;
    public OfficialInformationArticleBodies[] bodies;
    public DateTime? postscript_update_at;

    public OfficialInformationArticle()
    {
    }

    public OfficialInformationArticle(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.title = (string) json[nameof (title)];
      this.title_img_url = (string) json[nameof (title_img_url)];
      this.summary = (string) json[nameof (summary)];
      this.priority = (int) (long) json[nameof (priority)];
      this.published_at = DateTime.Parse((string) json[nameof (published_at)]);
      this.official_update_type = (int) (long) json[nameof (official_update_type)];
      this.category_id = (int) (long) json[nameof (category_id)];
      this.id = (int) (long) json[nameof (id)];
      List<OfficialInformationArticleBodies> informationArticleBodiesList = new List<OfficialInformationArticleBodies>();
      foreach (object json1 in (List<object>) json[nameof (bodies)])
        informationArticleBodiesList.Add(json1 == null ? (OfficialInformationArticleBodies) null : new OfficialInformationArticleBodies((Dictionary<string, object>) json1));
      this.bodies = informationArticleBodiesList.ToArray();
      this.postscript_update_at = json[nameof (postscript_update_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (postscript_update_at)]));
    }

    public bool IsPast(DateTime accessTime)
    {
      return accessTime > this.published_at || ServerTime.NowAppTime() - this.published_at >= TimeSpan.FromDays(3.0);
    }

    public bool IsPastPostscript(DateTime accessTime)
    {
      if (!this.postscript_update_at.HasValue)
        return true;
      DateTime dateTime = accessTime;
      DateTime? postscriptUpdateAt = this.postscript_update_at;
      return postscriptUpdateAt.HasValue && dateTime > postscriptUpdateAt.GetValueOrDefault();
    }
  }
}
