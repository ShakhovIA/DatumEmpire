namespace Core.Scripts
{
    public interface IView
    {
        public void SetResolution();
        public void Initialization();
        public void Open();
        public void Close();
    }
}