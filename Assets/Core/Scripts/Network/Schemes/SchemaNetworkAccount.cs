using System;
namespace Core.Scripts.Network
{
    [Serializable]
    public class SchemaNetworkAccount
    {
        #region Fields

        public string AccountIdVk;
        public string AccountIdOk;
        public string AccountIdYa;
        
        public string AccountNickName;
        public string AccountGold;
        public string AccountRating;

        public string AccountSummaryPlayTime;
        public string AccountCountSessions;
        public string AccountCountADInterstitial;
        public string AccountCountADRewards;
        public string AccountAge;
        public string AccountGender;
        
        /// <summary>
        /// Variables : Online,Offline,SearchPVP
        /// </summary>
        public string AccountStatus;

        #endregion
    }
}
