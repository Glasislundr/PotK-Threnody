// Decompiled with JetBrains decompiler
// Type: GameCore.AssemblyInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Text;

#nullable disable
namespace GameCore
{
  public class AssemblyInfo
  {
    public string name;
    public string culture;
    public string version;
    public string public_key_token;

    public string FullName
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder(this.name);
        if (this.version != null)
        {
          stringBuilder.Append(", Version=");
          stringBuilder.Append(this.version);
        }
        if (this.culture != null)
        {
          stringBuilder.Append(", Culture=");
          if (this.culture.Length == 0)
            stringBuilder.Append("neutral");
          else
            stringBuilder.Append(this.culture);
        }
        if (this.public_key_token != null)
        {
          stringBuilder.Append(", PublicKeyToken=");
          stringBuilder.Append(this.public_key_token);
        }
        return stringBuilder.ToString();
      }
    }

    public AssemblyInfo Clone()
    {
      return new AssemblyInfo()
      {
        name = this.name,
        culture = this.culture,
        version = this.version,
        public_key_token = this.public_key_token
      };
    }

    public override string ToString()
    {
      return string.Format("[AssemblyInfo: {0}]", (object) this.FullName);
    }
  }
}
