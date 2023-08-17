using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Injection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Scripts
{
    public class GamePipeline : MonoBehaviour
    {
        private readonly InjectionsBinder[] _preInitializedBinders = 
        {
                new ConfigBinder(),
        };

        private Injector _injector;
        private readonly GamePipelineUpdater _updater = new();

        //[field: SerializeField] private VFXEffect[] VFXEffects;

        public void Awake()
        {
            Application.targetFrameRate = 60;
            //MetagameEvents.Reset();
            _injector = new Injector();
            InjectComponents(_injector);
            InitializeComponents(_injector);
        }
        
        public void Start()
        {
            // _injector.Get<PresentationAssets>().IngameDebugConsole.SetActive(Static.BuildMode == BuildMode.Development);
            // StartCoroutine(_injector.Get<StartupScriptSystem>().Run());
        }

        private void InjectComponents(Injector injector)
        {
            injector.Bind(injector);
            injector.Bind(Camera.main);
            injector.Bind(EventSystem.current);
            
            injector.Bind(new InjectionInstantiator(injector));

            injector.Bind(FindObjectOfType<ManagerData>());
            injector.Bind(FindObjectOfType<ViewMainMenu>());
            
            // injector.Bind(FindObjectOfType<DataAccount>());
            // injector.Bind(FindObjectOfType<SoundManager>());
            //
            // injector.Bind(FindObjectOfType<JsonManager>());
            //
            // injector.Bind(FindObjectOfType<ViewManager>());
            // injector.Bind(FindObjectOfType<ViewMenu>());
            // injector.Bind(FindObjectOfType<ViewGame>());
            // injector.Bind(FindObjectOfType<ViewCategory>());
            // injector.Bind(FindObjectOfType<ViewMyPuzzles>());
            // injector.Bind(FindObjectOfType<ViewMenuWidgets>());
            //
            // injector.Bind(FindObjectOfType<PopupSettings>());
            // injector.Bind(FindObjectOfType<PopupAuthors>());
            // injector.Bind(FindObjectOfType<PopupNewPuzzle>());
            // injector.Bind(FindObjectOfType<PopupOffer>());
            // injector.Bind(FindObjectOfType<PopupQuests>());
            // injector.Bind(FindObjectOfType<PopupUnlockPuzzle>());
            // injector.Bind(FindObjectOfType<PopupVictory>());
            // injector.Bind(FindObjectOfType<PopupContinuePuzzle>());
            // injector.Bind(FindObjectOfType<PopupChangeDifficulty>());
            // injector.Bind(FindObjectOfType<PopupEncouragement>());
            // injector.Bind(FindObjectOfType<PopupTutorial>());
            // injector.Bind(FindObjectOfType<PopupDailyReward>());
            // injector.Bind(FindObjectOfType<PopupShop>());
            // injector.Bind(FindObjectOfType<PopupAdsDisable>());
            // injector.Bind(FindObjectOfType<PopupAdblock>());
            //
            // injector.Bind(new VFXCreator(VFXEffects));
            // injector.Bind(new GridHelper<GridScript>());
            // injector.Bind(new PriceBuilder());

            injector.CommitBindings();

        }

        private void InitializeComponents(Injector injector)
        {
            var orderedHandlers = GetAll<IPipelineInitialized>(injector).OrderBy(GetOrder);
            foreach (IPipelineInitialized handler in orderedHandlers)
                handler.Initialize();

            RegisterKeyboardHandlers();
            _updater.Register(injector);
            
        }

        private int GetOrder(IPipelineInitialized handler)
        {
            var attr = handler.GetType().GetCustomAttribute<InitializationOrderAttribute>();
            return attr?.Order ?? 0;
        }

        private void RegisterKeyboardHandlers()
        {
            // KeyboardManager keyboardManager = FindObjectOfType<KeyboardManager>();
            // GetAll<IKeyUpHandler>(_injector).ForEach(keyUpHandler => keyboardManager.Register(keyUpHandler));
            // GetAll<IKeyDownHandler>(_injector).ForEach(keyDownHandler => keyboardManager.Register(keyDownHandler));
            // GetAll<IKeyHandler>(_injector).ForEach(keyHandler => keyboardManager.Register(keyHandler));
            // GetAll<IMouseScrollHandler>(_injector).ForEach(mouseScrollHandler => keyboardManager.Register(mouseScrollHandler));
        }
        
        private List<T> GetAll<T>(Injector injector)
        {
            return injector.GetAll().Where(_ => _ is T).Cast<T>().ToList();
        }

        private void Update()
        {
            _updater.OnUpdate();
        }

        private void LateUpdate()
        {
            _updater.OnLateUpdate();
        }

        
    }
}