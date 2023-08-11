using System.Linq;
using Injection;
using UnityEngine;

namespace Core.Scripts
{
    public class ConfigBinder : InjectionsBinder
    {
        public const string ConfigFolderPath = "Configs/";

        public override void BindInjections(Injector injector)
        {
            // BindConfig<TooltipConfig>(injector);
            // BindConfig<CursorConfig>(injector);
            // BindConfig<CameraConfig>(injector);
        }

        private void BindConfig<T>(Injector injector) where T : ScriptableObject
        {
            injector.Bind(Resources.LoadAll<T>(ConfigFolderPath).Single());
        }
    }
}