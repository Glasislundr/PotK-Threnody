// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.Response.ProductList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.Response
{
  public class ProductList : Gsc.Network.Response<ProductList>
  {
    public ProductList.ProductData_t[] Products { get; private set; }

    public ProductList(byte[] payload)
    {
      Dictionary<string, object> result = Gsc.Network.Response<ProductList>.GetResult(payload);
      this.Products = Deserializer.Instance.WithArray<ProductList.ProductData_t>().Add<ProductList.ProductData_t>(new Func<object, ProductList.ProductData_t>(Deserializer.ToObject<ProductList.ProductData_t>)).Deserialize<ProductList.ProductData_t[]>(result["products"]);
    }

    public class ProductData_t : IResponseObject, IObject
    {
      public string ProductId { get; private set; }

      public float Price { get; private set; }

      public string Name { get; private set; }

      public string Currency { get; private set; }

      public string LocalizedPrice { get; private set; }

      public string Description { get; private set; }

      public ProductData_t(Dictionary<string, object> payload)
      {
        this.ProductId = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(payload["product_id"]);
        this.Price = Deserializer.Instance.Add<float>(new Func<object, float>(Deserializer.ToNumberType.float32)).Deserialize<float>(payload["price"]);
        this.Name = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(payload["name"]);
        this.Currency = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(payload["currency"]);
        this.LocalizedPrice = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(payload["localized_price"]);
        this.Description = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(payload["description"]);
      }
    }
  }
}
