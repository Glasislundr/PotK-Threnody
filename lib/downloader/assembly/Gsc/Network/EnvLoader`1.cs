// Decompiled with JetBrains decompiler
// Type: Gsc.Network.EnvLoader`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using Gsc.DOM.Json;
using System.Linq;

#nullable disable
namespace Gsc.Network
{
  public class EnvLoader<T> : Request<EnvLoader<T>, EnvLoader<T>.Response> where T : struct, Configuration.IEnvironment
  {
    private string url;
    private static string cachedVersion;

    public EnvLoader(string url, string version)
    {
      this.url = url;
      EnvLoader<T>.cachedVersion = version;
    }

    public override string GetMethod() => "GET";

    public override string GetPath() => (string) null;

    public override string GetUrl() => this.url;

    public class Response : Gsc.Network.Response<EnvLoader<T>.Response>
    {
      public string EnvName { get; private set; }

      public Configuration.IEnvironment Env { get; private set; }

      public Response(WebInternalResponse response)
      {
        string str = EnvLoader<T>.cachedVersion ?? App.GetBundleVersion();
        EnvLoader<T>.cachedVersion = (string) null;
        using (Document document = Document.Parse(response.Payload))
        {
          string name = document.Root.GetValueByPointer("/ver_route/" + str, (string) null) ?? document.Root.GetValueByPointer("/ver_route/default", (string) null);
          Value obj1;
          if (!document.Root.GetObject().TryGetValue("environments", out obj1))
            return;
          Value obj2;
          if (name != null)
          {
            obj2 = obj1[name];
          }
          else
          {
            Member member = obj1.GetObject().First<Member>();
            name = member.Name;
            obj2 = member.Value;
          }
          T obj3 = new T();
          foreach (Member member in obj2.GetObject())
            obj3.SetValue(member.Name, member.Value.ToString());
          this.EnvName = name;
          this.Env = (Configuration.IEnvironment) obj3;
        }
      }
    }
  }
}
