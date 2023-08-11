using System;
using System.Collections.Generic;
using Injection;
using UnityEngine;

namespace Core.Scripts
{
    public class GamePipelineUpdater
    {
        private readonly List<IPipelineUpdated> _updateItems = new();
        private readonly List<IPipelineLateUpdated> _lateUpdateItems = new();
        private readonly HashSet<int> _exceptionHashCodes = new();

        public void Register(Injector injector)
        {
            foreach (object obj in injector.GetAll())
            {
                if (obj is IPipelineLateUpdated pipelineLateUpdated)
                    _lateUpdateItems.Add(pipelineLateUpdated);

                if (obj is IPipelineUpdated pipelineUpdated)
                    _updateItems.Add(pipelineUpdated);
            }
        }

        public void OnUpdate()
        {
            foreach (IPipelineUpdated item in _updateItems)
            {
                try
                {
                    item.OnUpdate();
                }
                catch (Exception ex)
                {
                    int hashCode = ex.StackTrace.GetHashCode();
                    if (!_exceptionHashCodes.Contains(hashCode))
                    {
                        _exceptionHashCodes.Add(hashCode);
                        Debug.Log($"Exception in OnUpdate: {ex}");
                        throw;
                    }
                }
            }
        }

        public void OnLateUpdate()
        {
            foreach (IPipelineLateUpdated item in _lateUpdateItems)
            {
                try
                {
                    item.OnLateUpdate();
                }
                catch (Exception ex)
                {
                    int hashCode = ex.StackTrace.GetHashCode();
                    if (!_exceptionHashCodes.Contains(hashCode))
                    {
                        _exceptionHashCodes.Add(hashCode);
                        Debug.Log($"Exception in OnUpdate: {ex}");
                        throw;
                    }
                }
            }
        }
    }
}