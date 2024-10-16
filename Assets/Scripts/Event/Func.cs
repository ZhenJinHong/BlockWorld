namespace CatFramework.EventsMiao
{
    public delegate void EventMiao<T0>(object sender, T0 args) where T0 : class;
    public delegate bool TryFunc<in key, re>(key key, out re re);
}
