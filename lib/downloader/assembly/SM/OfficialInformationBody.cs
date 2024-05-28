// Decompiled with JetBrains decompiler
// Type: SM.OfficialInformationBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class OfficialInformationBody : KeyCompare
  {
    public string body;
    public string param_name;
    public int official_information_article;
    public int extra_position;
    public int extra_id;
    public int img_width;
    public string body_img_url;
    public int img_height;
    public string scene_name;
    public int extra_type;

    public OfficialInformationBody()
    {
    }

    public OfficialInformationBody(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.body = (string) json[nameof (body)];
      this.param_name = (string) json[nameof (param_name)];
      this.official_information_article = (int) (long) json[nameof (official_information_article)];
      this.extra_position = (int) (long) json[nameof (extra_position)];
      this.extra_id = (int) (long) json[nameof (extra_id)];
      this.img_width = (int) (long) json[nameof (img_width)];
      this.body_img_url = (string) json[nameof (body_img_url)];
      this.img_height = (int) (long) json[nameof (img_height)];
      this.scene_name = (string) json[nameof (scene_name)];
      this.extra_type = (int) (long) json[nameof (extra_type)];
    }
  }
}
