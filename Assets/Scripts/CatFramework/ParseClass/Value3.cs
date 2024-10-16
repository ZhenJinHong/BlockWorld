namespace CatFramework
{
    [System.Serializable]
    public struct Value3<T>//这个需要被获取重写字段的
    {
        public T x;
        public T y;
        public T z;
        readonly string xN;
        readonly string yN;
        readonly string zN;
        public readonly string Xname => xN;
        public readonly string Yname => yN;
        public readonly string Zname => zN;
        public Value3(T x, T y, T z, string xN, string yN, string zN)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.xN = xN;
            this.yN = yN;
            this.zN = zN;
        }
        public override string ToString()
        {
            return $"X:{x};Y:{y};Z:{z}";
        }
    }
}
