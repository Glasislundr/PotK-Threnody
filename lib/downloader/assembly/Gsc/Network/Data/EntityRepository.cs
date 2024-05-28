// Decompiled with JetBrains decompiler
// Type: Gsc.Network.Data.EntityRepository
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Components;
using Gsc.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Network.Data
{
  public class EntityRepository
  {
    private static readonly EntityRepository repository = new EntityRepository();
    private readonly Dictionary<Type, EntityRepository.ISimpleRepository> entityRepositories = new Dictionary<Type, EntityRepository.ISimpleRepository>();

    public static T Get<T>(string key) where T : Entity<T>
    {
      T obj;
      return EntityRepository.repository.GetRepository<T>().PublicList.TryGetValue(key, out obj) ? obj : default (T);
    }

    public static EntityList<T> GetAll<T>() where T : Entity<T>
    {
      return EntityRepository.repository.GetRepository<T>().PublicList;
    }

    public static void Subscribe<T>(
      NotificationObserver<EntityNotification<T>> observer,
      T sender = null)
      where T : Entity<T>
    {
      object sender1 = (object) null;
      if ((object) sender != null)
        sender1 = EntityRepository.repository.GetRepository<T>().GetSender(sender.pk);
      NotificationCenter.Instance.AddObserver<EntityNotification<T>>(observer, sender1);
    }

    public static void AllClear()
    {
      foreach (EntityRepository.ISimpleRepository simpleRepository in EntityRepository.repository.entityRepositories.Values)
        simpleRepository.AllClear();
    }

    public static void CacheClear()
    {
      foreach (EntityRepository.ISimpleRepository simpleRepository in EntityRepository.repository.entityRepositories.Values)
        simpleRepository.CacheClear();
    }

    private EntityRepository()
    {
    }

    public static uint ver { get; private set; }

    private EntityRepository.SimpleRepository<T> GetRepository<T>() where T : Entity<T>
    {
      return (EntityRepository.SimpleRepository<T>) this.GetRepository(typeof (T));
    }

    private EntityRepository.ISimpleRepository GetRepository(Type type)
    {
      EntityRepository.ISimpleRepository instance;
      if (!this.entityRepositories.TryGetValue(type, out instance))
      {
        instance = AssemblySupport.CreateInstance<EntityRepository.ISimpleRepository>("Gsc.Network.Data.EntityRepository+SimpleRepository`1[[" + type.AssemblyQualifiedName + "]]");
        this.entityRepositories.Add(type, instance);
      }
      return instance;
    }

    public static void Update(
      Dictionary<string, object> updateModels,
      Dictionary<string, object> removeModels)
    {
      if (updateModels != null)
      {
        foreach (KeyValuePair<string, object> updateModel in updateModels)
        {
          Type type = AssemblySupport.GetType(string.Format("Gsc.Data.Model.{0}", (object) updateModel.Key));
          if (!(type == (Type) null))
          {
            AssemblySupport.MethodInfo constructor = AssemblySupport.GetConstructor(type, typeof (Dictionary<string, object>));
            EntityRepository.ISimpleRepository repository = EntityRepository.repository.GetRepository(constructor.Type);
            object obj = (object) null;
            foreach (Dictionary<string, object> dictionary in (List<object>) updateModel.Value)
              repository.Push(constructor.CreateInstance<IEntity>((object) dictionary), (!dictionary.TryGetValue("_is_mine", out obj) ? 0 : ((bool) obj ? 1 : 0)) != 0);
          }
        }
      }
      if (removeModels != null)
      {
        foreach (KeyValuePair<string, object> removeModel in removeModels)
        {
          Type type = AssemblySupport.GetType(string.Format("Gsc.Data.Model.{0}", (object) removeModel.Key));
          if (!(type == (Type) null))
          {
            EntityRepository.ISimpleRepository repository = EntityRepository.repository.GetRepository(type);
            foreach (string key in (List<object>) removeModel.Value)
              repository.Remove(key);
          }
        }
      }
      if (++EntityRepository.ver != 0U)
        return;
      EntityRepository.ver = 1U;
    }

    private interface ISimpleRepository
    {
      void Push(IEntity value, bool isPermanent);

      void Remove(string key);

      void AllClear();

      void CacheClear();
    }

    private class SimpleRepository<T> : EntityRepository.ISimpleRepository where T : Entity<T>
    {
      public readonly EntityList<T> PublicList;
      private readonly SortedList<string, T> entityList = new SortedList<string, T>();
      private readonly SortedList<string, T> permanentEntitityList = new SortedList<string, T>();
      private SortedList<string, object> senderList = new SortedList<string, object>();

      public SimpleRepository() => this.PublicList = new EntityList<T>(this.entityList);

      public void Push(T value, bool isPermanent)
      {
        object sender = !this.entityList.TryGetValue(value.pk, out T _) ? (this.senderList[value.pk] = new object()) : this.senderList[value.pk];
        this.entityList[value.pk] = value;
        if (isPermanent)
          this.permanentEntitityList[value.pk] = value;
        NotificationCenter.Instance.Post<EntityNotification<T>>(new EntityNotification<T>(value, EntityNotificationType.Update), sender);
      }

      void EntityRepository.ISimpleRepository.Push(IEntity value, bool isPermanent)
      {
        this.Push((T) value, isPermanent);
      }

      public void Remove(string key)
      {
        T entity;
        if (this.entityList.TryGetValue(key, out entity))
        {
          object sender = this.senderList[key];
          this.entityList.Remove(key);
          this.senderList.Remove(key);
          NotificationCenter.Instance.Post<EntityNotification<T>>(new EntityNotification<T>(entity, EntityNotificationType.Remove), sender);
          NotificationCenter.Instance.RemoveObserversWithSender<EntityNotification<T>>(sender);
        }
        this.permanentEntitityList.Remove(key);
      }

      public object GetSender(string key)
      {
        object sender;
        this.senderList.TryGetValue(key, out sender);
        return sender;
      }

      public void AllClear()
      {
        IList<T> values = this.entityList.Values;
        for (int index = 0; index < values.Count; ++index)
          NotificationCenter.Instance.RemoveObserversWithSender<EntityNotification<T>>(this.senderList[values[index].pk]);
        this.entityList.Clear();
        this.senderList.Clear();
        this.permanentEntitityList.Clear();
      }

      public void CacheClear()
      {
        IList<T> values1 = this.entityList.Values;
        for (int index = 0; index < values1.Count; ++index)
        {
          T obj = values1[index];
          if (!this.permanentEntitityList.ContainsKey(obj.pk))
            NotificationCenter.Instance.RemoveObserversWithSender<EntityNotification<T>>(this.senderList[obj.pk]);
        }
        this.entityList.Clear();
        SortedList<string, object> sortedList = new SortedList<string, object>();
        IList<T> values2 = this.permanentEntitityList.Values;
        for (int index = 0; index < values2.Count; ++index)
        {
          T obj = values2[index];
          this.entityList.Add(obj.pk, obj);
          sortedList.Add(obj.pk, this.senderList[obj.pk]);
        }
        this.senderList.Clear();
        this.senderList = sortedList;
      }
    }
  }
}
