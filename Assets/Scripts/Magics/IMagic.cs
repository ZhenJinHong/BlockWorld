namespace CatFramework.Magics
{
    public interface IMagic
    {
        bool Fire();
        void Linked(IMagicWand parent);
    }
}
