namespace Core.Scripts
{
    public interface IPipelineUpdated
    {
        public void OnUpdate();
    }

    public interface IPipelineLateUpdated
    {
        public void OnLateUpdate();
    }
}