//using System.Collections;
//using UnityEngine;
//using System;

//namespace CatFramework.UiMiao
//{
//    public struct Value3<T>
//    {
//        public T x;
//        public T y;
//        public T z;
//        readonly string xN;
//        readonly string yN;
//        readonly string zN;
//        public readonly string Xname => xN;
//        public readonly string Yname => yN;
//        public readonly string Zname => zN;
//        public Value3(T x, T y, T z, string xN, string yN, string zN)
//        {
//            this.x = x;
//            this.y = y;
//            this.z = z;
//            this.xN = xN;
//            this.yN = yN;
//            this.zN = zN;
//        }
//        public override string ToString()
//        {
//            return $"X:{x};Y:{y};Z:{z}";
//        }
//    }
//    public abstract class T3InputField<T> : TextMiao, IInputField<Value3<T>>//实现该接口是因为读取值的需要
//    {
//        [SerializeField] TInputField<T> x;
//        [SerializeField] TInputField<T> y;
//        [SerializeField] TInputField<T> z;
//        [SerializeField] ButtonMiao enter;
//        Value3<T> value;
//        Action<Value3<T>> onSubmit;
//        protected override void Start()
//        {
//            if (enter)
//                enter.Click += Enter;
//            base.Start();
//        }
//        public string FieldName
//        {
//            get => TranslationKey;
//            set => TranslationKey = value;
//        }
//        public void AddListener(Action<Value3<T>> onSubmit)//不是所有的都需要监听，所以那三个输入字段不能在这里监听
//        {
//            this.onSubmit = onSubmit;
//        }
//        public Value3<T> GetValue()
//        {
//            if (x)
//                value.x = x.GetValue();
//            if (y)
//                value.y = y.GetValue();
//            if (z)
//                value.z = z.GetValue();
//            return value;
//        }
//        public void SetValueWithoutNotify(Value3<T> value)
//        {
//            this.value = value;
//            if (x)
//            {
//                x.SetValueWithoutNotify(value.x);
//                x.FieldName = value.Xname;
//            }
//            if (y)
//            {
//                y.SetValueWithoutNotify(value.y);
//                y.FieldName = value.Yname;
//            }
//            if (z)
//            {
//                z.SetValueWithoutNotify(value.z);
//                z.FieldName = value.Zname;
//            }
//        }
//        public void SetValue(Value3<T> value)
//        {
//            SetValueWithoutNotify(value);
//            onSubmit?.Invoke(value);
//        }
//        void Enter()
//        {
//            onSubmit?.Invoke(GetValue());
//        }
//    }
//}