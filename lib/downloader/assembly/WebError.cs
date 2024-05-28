// Decompiled with JetBrains decompiler
// Type: WebError
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
public class WebError
{
  public readonly WebResponse Response;
  public readonly WebRequest Request;
  public readonly int Status;

  public WebError(WebRequest request, WebResponse response)
  {
    this.Request = request;
    this.Response = response;
    this.Status = response.Status;
  }

  private WebError(WebRequest request, string message)
  {
    this.Request = request;
    this.Response = WebResponse.Zero();
    this.Response.Body = message;
    this.Status = this.Response.Status;
  }

  public bool HasResponse() => this.Status > 0;

  public bool IsClientError() => this.Status >= 400 && this.Status < 500;

  public bool IsServerError() => this.Status >= 500 && this.Status < 600;

  public string Show()
  {
    return string.Format("{0} {1} {2}", (object) this.Status, (object) this.Request.Path, (object) this.Response.Body);
  }

  public static WebError ClientError4xx(WebRequest request, WebResponse response)
  {
    return new WebError(request, response);
  }

  public static WebError ServerError5xx(WebRequest request, WebResponse response)
  {
    return new WebError(request, response);
  }

  public static WebError Timeout(WebRequest request) => new WebError(request, nameof (Timeout));

  public static WebError RetryOut(WebRequest request) => new WebError(request, nameof (RetryOut));

  public static WebError ResponseError(WebRequest request)
  {
    return new WebError(request, nameof (ResponseError));
  }

  public static WebError ResponseException(WebRequest request, Exception exception)
  {
    return new WebError(request, "InternalError");
  }
}
