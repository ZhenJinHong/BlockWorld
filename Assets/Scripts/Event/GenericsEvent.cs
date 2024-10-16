namespace CatFramework.EventsMiao
{
    public interface IUniqueEvents
    {

    }
    public interface IUniquePublisher
    {

    }
    public class GenericsEvent
    {
        //public Delegate Action;// 用as 或 is 转实际事件(此时引用相同),对这个委托+=注册会导致生成是另一个委托(引用不相同),因为+= 的本质是new ?

    }
    public class GenericsEvent<T0> : GenericsEvent where T0 : class
    {
        public event EventMiao<T0> Action;
        public bool EventIsEmpty => Action == null;
        public void Invoke(object sender, T0 arg0)
        {
            Action?.Invoke(sender, arg0);
        }
    }
}