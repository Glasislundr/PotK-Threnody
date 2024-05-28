// Decompiled with JetBrains decompiler
// Type: Gsc.Network.WebInternalTask`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.Network
{
  public class WebInternalTask<TRequest, TResponse> : WebInternalTask
    where TRequest : Request<TRequest, TResponse>
    where TResponse : Gsc.Network.Response<TResponse>
  {
    private readonly TRequest _request;
    private TResponse _response;
    private IErrorResponse _error;

    public TResponse Response => this._response;

    public IErrorResponse error => this._error;

    public WebInternalTask(Request<TRequest, TResponse> request)
      : base(request.GetMethod(), request.GetUrl(), request.GetPayload(), request.CustomHeaders)
    {
      this._request = (TRequest) request;
    }

    protected override WebTaskResult ProcessResponse(WebInternalResponse response)
    {
      return WebTask<TRequest, TResponse>.TryGetResponse(this._request, response, out this._response, out this._error);
    }
  }
}
