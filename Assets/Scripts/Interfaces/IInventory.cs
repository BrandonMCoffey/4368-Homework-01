namespace Assets.Scripts.Interfaces
{
    public interface IInventory<in T>
    {
        void OnCollect(T pickup);
    }
}