using Injection;
using UnityEngine;

namespace Core.Scripts
{
    public class InjectionInstantiator
    {
        private readonly Injector _injector;

        public InjectionInstantiator(Injector injector)
        {
            _injector = injector;
        }

        public T Instantiate<T>(T prefab) where T : Component
        {
            T instance = Object.Instantiate(prefab);
            InjectTo(instance);
            return instance;
        }

        public T Instantiate<T>(T prefab, Transform parent) where T : Component
        {
            T instance = Object.Instantiate(prefab, parent);
            InjectTo(instance);
            return instance;
        }

        public T AddComponent<T>(GameObject gameObject) where T : Component
        {
            T instance = gameObject.AddComponent<T>();
            _injector.InjectTo(instance);
            return instance;
        }

        public void InjectTo(Component root)
        {
            foreach (Component component in root.GetComponentsInChildren<Component>(includeInactive: true))
                _injector.InjectTo(component);
        }
    }
}