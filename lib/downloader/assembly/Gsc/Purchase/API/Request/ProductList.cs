// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.Request.ProductList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Auth;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.Request
{
  public class ProductList : Gsc.Network.Request<ProductList, Gsc.Purchase.API.Response.ProductList>
  {
    private const string ___path = "{0}/{1}/products";

    public override string GetPath()
    {
      return string.Format("{0}/{1}/products", (object) SDK.Configuration.Env.PurchaseApiPrefix, (object) Device.Platform);
    }

    public override string GetMethod() => "GET";

    protected override Dictionary<string, object> GetParameters()
    {
      return (Dictionary<string, object>) null;
    }
  }
}
