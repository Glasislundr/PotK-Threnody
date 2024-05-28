// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.Deserializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UniLinq;

#nullable disable
namespace GameCore.Serialization
{
  internal class Deserializer
  {
    private CrossSerializer cs;
    private DeserializeContext context;

    public Deserializer(
      CrossSerializer cs,
      DeserializeContext context,
      TypeObject[] objects,
      TreeObject[] trees)
    {
      this.cs = cs;
      this.context = context;
      foreach (TypeObject typeObject in objects)
        context.objects.Add(typeObject.objectId, typeObject);
      foreach (TreeObject tree in trees)
        context.trees.Add(tree.treeId, tree);
    }

    private object GetBlob(int objectId, TypeObject typeObj, TreeObject treeObj, System.Type type)
    {
      object blob;
      if (this.context.objectIdToObj.TryGetValue(objectId, out blob))
        return blob;
      CrossSerializer.Specialized.Value obj1;
      if (CrossSerializer.Specialized.TryGetValue(type, out obj1))
      {
        SerializeInfo serializeInfo = new SerializeInfo((Serializer) null, this, typeObj, treeObj, type);
        return obj1.Deserialize(serializeInfo);
      }
      if (type.IsGenericType && CrossSerializer.Specialized.TryGetValue(type.GetGenericTypeDefinition(), out obj1))
      {
        SerializeInfo serializeInfo = new SerializeInfo((Serializer) null, this, typeObj, treeObj, type);
        return obj1.Deserialize(serializeInfo);
      }
      if (!type.IsEnum)
        throw new Exception("unexpected blob type");
      CrossSerializer.Specialized.TryGetValue(Enum.GetUnderlyingType(type), out obj1);
      SerializeInfo serializeInfo1 = new SerializeInfo((Serializer) null, this, typeObj, treeObj, type);
      object obj2 = obj1.Deserialize(serializeInfo1);
      return Enum.ToObject(type, obj2);
    }

    public object Deserialize(int treeId)
    {
      if (treeId == 0)
        return (object) null;
      TreeObject tree = this.context.trees[treeId];
      int objectId = tree.objectId;
      object obj;
      if (this.context.treeIdToObj.TryGetValue(tree.treeId, out obj))
        return obj;
      TypeObject typeObj = this.context.objects[tree.objectId];
      System.Type type = this.cs.BindToType(typeObj.typeInfo);
      if (type.IsBlob())
      {
        obj = this.GetBlob(objectId, typeObj, tree, type);
      }
      else
      {
        bool isNew = !this.context.objectIdToObj.TryGetValue(tree.objectId, out obj);
        CrossSerializer.Specialized.ValueM valueM;
        if (CrossSerializer.Specialized.TryGetValueMutable(type, out valueM))
        {
          SerializeInfo serializeInfo = new SerializeInfo((Serializer) null, this, typeObj, tree, type);
          if (isNew)
            obj = FormatterServices.GetUninitializedObject(type);
          valueM.DeserializeM(serializeInfo, obj, isNew);
        }
        else if (type.IsGenericType && CrossSerializer.Specialized.TryGetValueMutable(type.GetGenericTypeDefinition(), out valueM))
        {
          SerializeInfo serializeInfo = new SerializeInfo((Serializer) null, this, typeObj, tree, type);
          if (isNew)
            obj = FormatterServices.GetUninitializedObject(type);
          valueM.DeserializeM(serializeInfo, obj, isNew);
        }
        else if (type.IsArray)
        {
          obj = (object) this.DeserializeArray(type.GetElementType(), tree, obj, isNew);
        }
        else
        {
          List<FieldInfo> allFields = type.GetAllFields();
          if (isNew)
            obj = FormatterServices.GetUninitializedObject(type);
          foreach (FieldInfo element in allFields)
          {
            if (Attribute.GetCustomAttribute((MemberInfo) element, typeof (NonSerializedAttribute)) == null)
            {
              if (!tree.fields.ContainsKey(element.Name))
                Debug.LogWarning((object) string.Format("field {0} is not set.", (object) element.Name));
              else
                element.SetValue(obj, this.Deserialize(tree.fields[element.Name]));
            }
          }
        }
      }
      this.context.objectIdToObj[objectId] = obj;
      this.context.treeIdToObj[treeId] = obj;
      return obj;
    }

    private bool NextIndex(int[] indices, int[] lengths)
    {
      for (int index = indices.Length - 1; index >= 0; --index)
      {
        if (indices[index] + 1 == lengths[index])
        {
          if (index != 0)
            indices[index] = 0;
        }
        else
        {
          ++indices[index];
          return true;
        }
      }
      return false;
    }

    private Array DeserializeArray(System.Type elementType, TreeObject treeObj, object obj, bool isNew)
    {
      if (!treeObj.fields.ContainsKey("lengths"))
      {
        int[] numArray = (int[]) this.Deserialize(treeObj.fields["__elems__"]);
        Array array = isNew ? Array.CreateInstance(elementType, numArray.Length) : (Array) obj;
        for (int index = 0; index < numArray.Length; ++index)
          array.SetValue(this.Deserialize(numArray[index]), index);
        return array;
      }
      int[] numArray1 = (int[]) this.Deserialize(treeObj.fields["lengths"]);
      int[] numArray2 = (int[]) this.Deserialize(treeObj.fields["__elems__"]);
      Array array1 = isNew ? Array.CreateInstance(elementType, numArray1) : (Array) obj;
      int[] array2 = ((IEnumerable<int>) numArray1).Select<int, int>((Func<int, int>) (_ => 0)).ToArray<int>();
      for (int index = 0; index < numArray2.Length; ++index)
      {
        array1.SetValue(this.Deserialize(numArray2[index]), array2);
        this.NextIndex(array2, numArray1);
      }
      return array1;
    }
  }
}
