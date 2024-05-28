// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.JsonFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MiniJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniLinq;

#nullable disable
namespace GameCore.Serialization
{
  public class JsonFormatter : ICrossFormatter
  {
    public Encoding Encoding;

    public JsonFormatter()
      : this(Encoding.UTF8)
    {
    }

    public JsonFormatter(Encoding encoding) => this.Encoding = encoding;

    public void Save(int rootId, TypeObject[] objects, TreeObject[] trees, Stream stream)
    {
      Dictionary<string, object>[] array1 = ((IEnumerable<TypeObject>) objects).Select<TypeObject, Dictionary<string, object>>((Func<TypeObject, Dictionary<string, object>>) (x => x.ToJson())).ToArray<Dictionary<string, object>>();
      Dictionary<string, object>[] array2 = ((IEnumerable<TreeObject>) trees).Select<TreeObject, Dictionary<string, object>>((Func<TreeObject, Dictionary<string, object>>) (x => x.ToJson())).ToArray<Dictionary<string, object>>();
      byte[] bytes = this.Encoding.GetBytes(JsonExtension.FormattedSerialize((object) new Dictionary<string, object>()
      {
        {
          "root",
          (object) rootId
        },
        {
          nameof (objects),
          (object) array1
        },
        {
          nameof (trees),
          (object) array2
        }
      }));
      stream.Write(bytes, 0, bytes.Length);
    }

    public void Load(
      Stream stream,
      out int rootId,
      out TypeObject[] objects,
      out TreeObject[] trees)
    {
      IDictionary<string, object> dictionary = (IDictionary<string, object>) Json.Deserialize(new StreamReader(stream, this.Encoding).ReadToEnd());
      rootId = (int) (long) dictionary["root"];
      objects = ((IEnumerable<object>) dictionary[nameof (objects)]).Select<object, TypeObject>((Func<object, TypeObject>) (x => TypeObject.FromJson((IDictionary<string, object>) x))).ToArray<TypeObject>();
      trees = ((IEnumerable<object>) dictionary[nameof (trees)]).Select<object, TreeObject>((Func<object, TreeObject>) (x => TreeObject.FromJson((IDictionary<string, object>) x))).ToArray<TreeObject>();
    }

    public static CrossSerializer MakeSerializer()
    {
      return new CrossSerializer((ICrossFormatter) new JsonFormatter());
    }

    public static CrossSerializer MakeSerializer(Encoding encoding)
    {
      return new CrossSerializer((ICrossFormatter) new JsonFormatter(encoding));
    }
  }
}
