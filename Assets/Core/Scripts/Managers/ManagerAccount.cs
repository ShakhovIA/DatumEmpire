using System.Collections.Generic;
using Injection;
using UnityEngine;

namespace Core.Scripts
{
    public sealed class ManagerAccount : MonoBehaviour
    {
        #region [Injections]
        
        [Inject] private ViewMainMenu ViewMainMenu { get; set; }
        [Inject] private WidgetsMainMenu WidgetsMainMenu { get; set; }
        
        #endregion
        
        
        #region [Fields]
        [field: SerializeField] public List<DataCurrency> DataCurrencies { get; set; }
        [field: SerializeField] public List<DataClimb> DataClimbs { get; set; }
        [field: SerializeField] public List<DataHero> DataHeroes { get; set; }
        
        [field: SerializeField] public DataBet DataBet { get; set; }

        #endregion
        
        
        #region [Functions]
        

        //public void
        
        
        
        #endregion
    }


    [System.Serializable]
    public sealed class DataCurrency
    {
        [field: SerializeField] public EnumCurrency TypeCurrency { get; set; }
        [field: SerializeField] public int Count { get; set; }

        public bool AddCurrency(int value)
        {
            Count += Mathf.Abs(value);
            return true;
        }
        
        public bool RemoveCurrency(int value)
        {
            if (Count >= value)
            {
                Count -= Mathf.Abs(value);
                return true;
            }
            return false;
        }
    }
    
    [System.Serializable]
    public sealed class DataClimb
    {
        
    }
    
    [System.Serializable]
    public sealed class DataHero
    {
        
    }

    [System.Serializable]
    public sealed class DataBet
    {
        [field: SerializeField] public EnumBet TypeBet { get; set; }
        [field: SerializeField] public int Count { get; set; }
    }
}