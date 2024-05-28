// Decompiled with JetBrains decompiler
// Type: Gsc.Network.WebInternalResponse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine.Networking;

#nullable disable
namespace Gsc.Network
{
  public class WebInternalResponse
  {
    private readonly WeakReference request;
    public readonly byte[] Payload;
    public readonly int StatusCode;
    public readonly ContentType ContentType;

    public WebInternalResponse(UnityWebRequest request)
    {
      this.request = new WeakReference((object) request);
      this.StatusCode = WebInternalResponse.GetStatusCode(request);
      this.Payload = WebInternalResponse.GetResponsePayload(request);
      this.ContentType = WebInternalResponse.GetContentType(this);
    }

    public WebInternalResponse(int statusCode)
    {
      this.request = (WeakReference) null;
      this.StatusCode = statusCode;
      this.Payload = (byte[]) null;
      this.ContentType = ContentType.None;
    }

    public string GetResponseHeader(string name)
    {
      string responseHeader = (string) null;
      if (this.request.IsAlive)
        responseHeader = ((UnityWebRequest) this.request.Target).GetResponseHeader(name);
      return responseHeader;
    }

    private static int GetStatusCode(UnityWebRequest webRequest)
    {
      return !webRequest.isNetworkError ? (int) webRequest.responseCode : 0;
    }

    private static byte[] GetResponsePayload(UnityWebRequest webRequest)
    {
      return webRequest.downloadHandler.data;
    }

    private static ContentType GetContentType(WebInternalResponse response)
    {
      string responseHeader = response.GetResponseHeader("CONTENT-TYPE");
      if (responseHeader != null)
      {
        if (responseHeader.StartsWith("application/json"))
          return ContentType.ApplicationJson;
        if (responseHeader.StartsWith("application/octet-stream"))
          return ContentType.ApplicationOctetStream;
      }
      return ContentType.TextPlain;
    }
  }
}
