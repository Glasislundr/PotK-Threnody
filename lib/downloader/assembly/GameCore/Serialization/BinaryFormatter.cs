// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.BinaryFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore.IO;
using gu3.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniLinq;

#nullable disable
namespace GameCore.Serialization
{
  public class BinaryFormatter : ICrossFormatter
  {
    private bool compressing;
    private Dictionary<string, int> strmap = new Dictionary<string, int>();
    private int strcounter;
    private string[] rstrmap;

    public BinaryFormatter()
    {
    }

    public BinaryFormatter(bool compressing) => this.compressing = compressing;

    private void RegisterString(string str)
    {
      if (this.strmap.ContainsKey(str))
        return;
      this.strmap.Add(str, this.strcounter++);
    }

    private void SaveStringMap(BinaryWriter writer)
    {
      writer.Write7BitInt(this.strmap.Count);
      foreach (KeyValuePair<string, int> keyValuePair in (IEnumerable<KeyValuePair<string, int>>) ((IEnumerable<KeyValuePair<string, int>>) this.strmap.ToArray<KeyValuePair<string, int>>()).OrderBy<KeyValuePair<string, int>, int>((Func<KeyValuePair<string, int>, int>) (x => x.Value)))
        writer.Write(keyValuePair.Key);
    }

    private void LoadStringMap(BinaryReader reader)
    {
      int length = reader.Read7BitInt();
      this.rstrmap = new string[length];
      for (int index = 0; index < length; ++index)
        this.rstrmap[index] = reader.ReadString();
    }

    private void SaveTreeObject(
      TreeObject tree,
      int minTreeId,
      int minObjectId,
      BinaryWriter writer)
    {
      writer.Write7BitInt(tree.treeId - minTreeId);
      writer.Write7BitInt(tree.objectId - minObjectId);
      writer.Write7BitInt(tree.fields.Count);
      foreach (KeyValuePair<string, int> field in tree.fields)
      {
        writer.Write7BitInt(this.strmap[field.Key]);
        writer.Write7BitInt(field.Value);
      }
    }

    private void SaveTypeObject(TypeObject obj, int minObjectId, BinaryWriter writer)
    {
      writer.Write7BitInt(obj.objectId - minObjectId);
      writer.Write7BitInt(this.strmap[obj.typeInfo.AssemblyQualifiedName]);
      if (obj.buf == null && !obj.length.HasValue)
      {
        writer.Write7BitInt(3);
        writer.Write7BitInt(obj.value);
      }
      else if (!obj.length.HasValue)
      {
        writer.Write7BitInt(1);
        writer.Write7BitInt(obj.buf.Length);
        writer.Write(obj.buf);
      }
      else
      {
        writer.Write7BitInt(2);
        writer.Write7BitInt(obj.buf.Length);
        writer.Write(obj.buf);
        writer.Write7BitInt(obj.length.Value);
      }
    }

    private TreeObject LoadTreeObject(BinaryReader reader, int minTreeId, int minObjectId)
    {
      TreeObject treeObject = new TreeObject();
      treeObject.treeId = reader.Read7BitInt() + minTreeId;
      treeObject.objectId = reader.Read7BitInt() + minObjectId;
      treeObject.fields = new AssocList<string, int>();
      int num1 = reader.Read7BitInt();
      for (int index = 0; index < num1; ++index)
      {
        string key = this.rstrmap[reader.Read7BitInt()];
        int num2 = reader.Read7BitInt();
        treeObject.fields.Add(key, num2);
      }
      return treeObject;
    }

    private TypeObject LoadTypeObject(BinaryReader reader, int minObjectId)
    {
      TypeObject typeObject = new TypeObject();
      typeObject.objectId = reader.Read7BitInt() + minObjectId;
      typeObject.typeInfo = TypeInfo.Parse(this.rstrmap[reader.Read7BitInt()]);
      switch (reader.Read7BitInt())
      {
        case 1:
          int count1 = reader.Read7BitInt();
          typeObject.buf = new byte[count1];
          reader.Read(typeObject.buf, 0, count1);
          break;
        case 3:
          typeObject.value = reader.Read7BitInt();
          break;
        default:
          int count2 = reader.Read7BitInt();
          typeObject.buf = new byte[count2];
          reader.Read(typeObject.buf, 0, count2);
          typeObject.length = new int?(reader.Read7BitInt());
          break;
      }
      return typeObject;
    }

    private void SaveCore(int rootId, TypeObject[] objects, TreeObject[] trees, Stream stream)
    {
      foreach (TypeObject typeObject in objects)
        this.RegisterString(typeObject.typeInfo.AssemblyQualifiedName);
      foreach (TreeObject tree in trees)
      {
        if (tree.fields != null)
        {
          foreach (KeyValuePair<string, int> field in tree.fields)
            this.RegisterString(field.Key);
        }
      }
      BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.UTF8);
      this.SaveStringMap(binaryWriter);
      int minObjectId = objects.Length == 0 ? 0 : ((IEnumerable<TypeObject>) objects).Min<TypeObject>((Func<TypeObject, int>) (x => x.objectId));
      binaryWriter.Write7BitInt(minObjectId);
      int minTreeId = trees.Length == 0 ? 0 : ((IEnumerable<TreeObject>) trees).Min<TreeObject>((Func<TreeObject, int>) (x => x.treeId));
      binaryWriter.Write7BitInt(minTreeId);
      binaryWriter.Write7BitInt(rootId);
      binaryWriter.Write7BitInt(objects.Length);
      foreach (TypeObject typeObject in objects)
        this.SaveTypeObject(typeObject, minObjectId, binaryWriter);
      binaryWriter.Write7BitInt(trees.Length);
      foreach (TreeObject tree in trees)
        this.SaveTreeObject(tree, minTreeId, minObjectId, binaryWriter);
      binaryWriter.Flush();
    }

    private void LoadCore(
      Stream stream,
      out int rootId,
      out TypeObject[] objects,
      out TreeObject[] trees)
    {
      BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8);
      this.LoadStringMap(binaryReader);
      int minObjectId = binaryReader.Read7BitInt();
      int minTreeId = binaryReader.Read7BitInt();
      rootId = binaryReader.Read7BitInt();
      int length1 = binaryReader.Read7BitInt();
      objects = new TypeObject[length1];
      for (int index = 0; index < length1; ++index)
        objects[index] = this.LoadTypeObject(binaryReader, minObjectId);
      int length2 = binaryReader.Read7BitInt();
      trees = new TreeObject[length2];
      for (int index = 0; index < length2; ++index)
        trees[index] = this.LoadTreeObject(binaryReader, minTreeId, minObjectId);
    }

    public void Save(int rootId, TypeObject[] objects, TreeObject[] trees, Stream stream)
    {
      if (this.compressing)
      {
        using (ZlibUtilStream zlibUtilStream = ZlibUtilStream.Compress(stream, (ZlibFormat) 0, (ZlibCompressionLevel) -1))
          this.SaveCore(rootId, objects, trees, (Stream) zlibUtilStream);
      }
      else
        this.SaveCore(rootId, objects, trees, stream);
    }

    public void Load(
      Stream stream,
      out int rootId,
      out TypeObject[] objects,
      out TreeObject[] trees)
    {
      if (this.compressing)
      {
        using (ZlibUtilStream zlibUtilStream = ZlibUtilStream.Decompress(stream, (ZlibFormat) 0))
          this.LoadCore((Stream) zlibUtilStream, out rootId, out objects, out trees);
      }
      else
        this.LoadCore(stream, out rootId, out objects, out trees);
    }

    public static CrossSerializer MakeSerializer()
    {
      return new CrossSerializer((ICrossFormatter) new BinaryFormatter());
    }

    public static CrossSerializer MakeSerializer(bool compressing)
    {
      return new CrossSerializer((ICrossFormatter) new BinaryFormatter(compressing));
    }
  }
}
