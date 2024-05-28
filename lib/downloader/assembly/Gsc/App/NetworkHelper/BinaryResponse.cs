// Decompiled with JetBrains decompiler
// Type: Gsc.App.NetworkHelper.BinaryResponse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Gsc.App.NetworkHelper
{
  public class BinaryResponse : Response<BinaryResponse>
  {
    public readonly int statusCode;
    public readonly bool isJson;
    public readonly string text;
    public readonly Dictionary<string, object> jsonData;

    public BinaryResponse(Dictionary<string, object> result)
    {
      this.statusCode = 200;
      this.isJson = true;
      this.text = (string) null;
      this.jsonData = result;
    }

    public BinaryResponse(WebInternalResponse response)
    {
      this.statusCode = response.StatusCode;
      this.isJson = response.ContentType == ContentType.ApplicationJson;
      if (this.isJson)
      {
        this.jsonData = (Dictionary<string, object>) Gsc.DOM.Json.MiniJSON.Json.Deserialize(response.Payload);
        this.text = (string) null;
      }
      else
      {
        this.jsonData = new Dictionary<string, object>();
        this.text = Encoding.UTF8.GetString(response.Payload);
      }
    }
  }
}
