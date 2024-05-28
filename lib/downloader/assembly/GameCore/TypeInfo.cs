// Decompiled with JetBrains decompiler
// Type: GameCore.TypeInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Text;

#nullable disable
namespace GameCore
{
  public class TypeInfo
  {
    public string name_space;
    public string[] nested;
    public string name;
    public AssemblyInfo assembly;
    public int[] modifiers;
    public TypeInfo[] type_arguments;

    public TypeInfo Clone()
    {
      TypeInfo typeInfo = new TypeInfo();
      typeInfo.name_space = this.name_space;
      typeInfo.nested = (string[]) this.nested.Clone();
      typeInfo.name = this.name;
      typeInfo.modifiers = (int[]) this.modifiers.Clone();
      typeInfo.type_arguments = new TypeInfo[this.type_arguments.Length];
      for (int index = 0; index < this.type_arguments.Length; ++index)
        typeInfo.type_arguments[index] = this.type_arguments[index].Clone();
      if (this.assembly != null)
        typeInfo.assembly = this.assembly.Clone();
      return typeInfo;
    }

    public static TypeInfo Parse(System.Type type) => TypeInfo.Parse(type.AssemblyQualifiedName);

    public static TypeInfo Parse(string type)
    {
      TypeInfo typeInfo;
      if (!TypeInfoCache.TryGetValue(type, out typeInfo))
        typeInfo = TypeInfoParser.Parse(type);
      return typeInfo.Clone();
    }

    public System.Type Type => System.Type.GetType(this.AssemblyQualifiedName);

    public string FullName
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (this.name_space.Length != 0)
        {
          stringBuilder.Append(this.name_space);
          stringBuilder.Append('.');
        }
        stringBuilder.Append(this.name);
        foreach (string str in this.nested)
        {
          stringBuilder.Append('+');
          stringBuilder.Append(str);
        }
        if (this.type_arguments.Length != 0)
        {
          stringBuilder.Append('`');
          stringBuilder.Append(this.type_arguments.Length);
          bool flag = true;
          foreach (TypeInfo typeArgument in this.type_arguments)
          {
            if (flag)
            {
              stringBuilder.Append('[');
              flag = false;
            }
            else
              stringBuilder.Append(",");
            if (typeArgument.assembly != null)
            {
              stringBuilder.Append('[');
              stringBuilder.Append(typeArgument.AssemblyQualifiedName);
              stringBuilder.Append(']');
            }
            else
              stringBuilder.Append(typeArgument.FullName);
          }
          stringBuilder.Append(']');
        }
        bool flag1 = false;
        foreach (int modifier in this.modifiers)
        {
          switch (modifier)
          {
            case -2:
              flag1 = true;
              break;
            case -1:
              stringBuilder.Append('*');
              break;
            case 0:
              stringBuilder.Append('&');
              break;
            case 1:
              if (flag1)
              {
                stringBuilder.Append("[*]");
                flag1 = false;
                break;
              }
              stringBuilder.Append("[]");
              break;
            default:
              stringBuilder.Append('[');
              stringBuilder.Append(',', modifier - 1);
              stringBuilder.Append(']');
              break;
          }
        }
        return stringBuilder.ToString();
      }
    }

    public string AssemblyQualifiedName
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder(this.FullName);
        if (this.assembly != null)
        {
          stringBuilder.Append(", ");
          stringBuilder.Append(this.assembly.FullName);
        }
        return stringBuilder.ToString();
      }
    }

    public override int GetHashCode() => this.AssemblyQualifiedName.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj != null && obj is TypeInfo typeInfo && this.AssemblyQualifiedName == typeInfo.AssemblyQualifiedName;
    }

    public override string ToString()
    {
      return string.Format("[TypeInfo: {0}]", (object) this.AssemblyQualifiedName);
    }
  }
}
