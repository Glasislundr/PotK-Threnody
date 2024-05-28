// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.Serializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UniLinq;

#nullable disable
namespace GameCore.Serialization
{
  internal class Serializer
  {
    private HashSet<object> recData = new HashSet<object>((IEqualityComparer<object>) new BlobEqualityComparer());
    private SerializeContext context;
    private int initialBlobCounter;
    private int initialTreeCounter;

    public Serializer(CrossSerializer cs, SerializeContext context)
    {
      this.context = context;
      this.initialBlobCounter = this.context.blobCounter;
      this.initialTreeCounter = this.context.treeCounter;
    }

    public Tuple<TypeObject[], TreeObject[]> GetResult()
    {
      return Tuple.Create<TypeObject[], TreeObject[]>(this.context.blobDic.Values.Where<TypeObject>((Func<TypeObject, bool>) (x => x.objectId >= this.initialBlobCounter)).OrderBy<TypeObject, int>((Func<TypeObject, int>) (x => x.objectId)).ToArray<TypeObject>(), this.context.treeDic.Keys.Where<TreeObject>((Func<TreeObject, bool>) (x => x.treeId >= this.initialTreeCounter)).OrderBy<TreeObject, int>((Func<TreeObject, int>) (x => x.treeId)).ToArray<TreeObject>());
    }

    private int SerializeInternArray(int[] v)
    {
      int num;
      if (this.context.internedArrayDic.TryGetValue(v, out num))
        return num;
      int key = this.Serialize((object) v);
      this.context.internedArrayDic.Add(v, key);
      this.context.reverseInternedArrayDic.Add(key, v);
      return key;
    }

    public int Serialize(object obj)
    {
      if (obj == null)
        return 0;
      if (this.recData.Contains(obj))
        throw new Exception("recursive object does not serialize.");
      this.recData.Add(obj);
      try
      {
        System.Type type = obj.GetType();
        TypeObject typeObj;
        bool flag = this.context.blobDic.TryGetValue(obj, out typeObj);
        if (!flag)
        {
          typeObj = new TypeObject();
          typeObj.objectId = this.context.blobCounter++;
          typeObj.typeInfo = GameCore.TypeInfo.Parse(type);
          CrossSerializer.RemoveAssemblyDetails(typeObj.typeInfo);
          this.context.blobDic.Add(obj, typeObj);
        }
        TreeObject treeObject = new TreeObject();
        treeObject.objectId = typeObj.objectId;
        if (type.IsBlob())
        {
          if (!flag)
          {
            CrossSerializer.Specialized.Value obj1;
            if (CrossSerializer.Specialized.TryGetValue(type, out obj1))
            {
              SerializeInfo serializeInfo = new SerializeInfo(this, (Deserializer) null, typeObj, treeObject, type);
              obj1.Serialize(serializeInfo, obj);
            }
            if (type.IsGenericType && CrossSerializer.Specialized.TryGetValue(type.GetGenericTypeDefinition(), out obj1))
            {
              SerializeInfo serializeInfo = new SerializeInfo(this, (Deserializer) null, typeObj, treeObject, type);
              obj1.Serialize(serializeInfo, obj);
            }
            if (type.IsEnum)
            {
              System.Type underlyingType = Enum.GetUnderlyingType(type);
              CrossSerializer.Specialized.TryGetValue(underlyingType, out obj1);
              SerializeInfo serializeInfo = new SerializeInfo(this, (Deserializer) null, typeObj, treeObject, type);
              obj1.Serialize(serializeInfo, Convert.ChangeType(obj, underlyingType));
            }
          }
        }
        else
        {
          CrossSerializer.Specialized.ValueM valueM;
          if (CrossSerializer.Specialized.TryGetValueMutable(type, out valueM))
          {
            SerializeInfo serializeInfo = new SerializeInfo(this, (Deserializer) null, typeObj, treeObject, type);
            valueM.Serialize(serializeInfo, obj);
          }
          else if (type.IsGenericType && CrossSerializer.Specialized.TryGetValueMutable(type.GetGenericTypeDefinition(), out valueM))
          {
            SerializeInfo serializeInfo = new SerializeInfo(this, (Deserializer) null, typeObj, treeObject, type);
            valueM.Serialize(serializeInfo, obj);
          }
          else if (type.IsArray)
          {
            Array ar = (Array) obj;
            if (ar.Rank >= 2)
            {
              int[] array = Enumerable.Range(0, ar.Rank).Select<int, int>((Func<int, int>) (v => ar.GetLength(v))).ToArray<int>();
              treeObject.fields["lengths"] = this.SerializeInternArray(array);
            }
            int[] v1 = new int[ar.Length];
            int index = 0;
            foreach (object obj2 in ar)
            {
              v1[index] = this.Serialize(obj2);
              ++index;
            }
            treeObject.fields["__elems__"] = this.SerializeInternArray(v1);
          }
          else
          {
            foreach (FieldInfo allField in type.GetAllFields())
            {
              if (Attribute.GetCustomAttribute((MemberInfo) allField, typeof (NonSerializedAttribute)) == null)
              {
                object obj3 = allField.GetValue(obj);
                treeObject.fields[allField.Name] = this.Serialize(obj3);
              }
            }
          }
        }
        int num;
        if (!this.context.treeDic.TryGetValue(treeObject, out num))
        {
          num = this.context.treeCounter++;
          treeObject.treeId = num;
          this.context.treeDic.Add(treeObject, num);
        }
        return num;
      }
      finally
      {
        this.recData.Remove(obj);
      }
    }
  }
}
