namespace CatFramework.Magics
{
    public class Magic : IMagic
    {
        protected IMagicWand Parent;
        public void Linked(IMagicWand parent) => Parent = parent;
        public virtual bool Fire()
        {
            return true;
        }
    }
}
