namespace DevTools
{
    using System;
    using System.Diagnostics;
    using System.ServiceModel;

    public class ServiceHost<T> : ServiceHost
    {
        public ServiceHost() : base(typeof(T), new Uri[0])
        {
        }

        public ServiceHost(params string[] baseAddresses) : base(typeof(T), ServiceHost<T>.Convert(baseAddresses))
        {
        }

        public ServiceHost(params Uri[] baseAddresses) : base(typeof(T), baseAddresses)
        {
        }

        public ServiceHost(T singleton) : base(singleton, new Uri[0])
        {
        }

        public ServiceHost(T singleton, params string[] baseAddresses) : base(singleton, ServiceHost<T>.Convert(baseAddresses))
        {
        }

        public ServiceHost(T singleton, params Uri[] baseAddresses) : base(singleton, baseAddresses)
        {
        }

        private static Uri[] Convert(string[] baseAddresses)
        {
            Converter<string, Uri> converter = delegate (string address) {
                return new Uri(address);
            };
            return Array.ConvertAll<string, Uri>(baseAddresses, converter);
        }

        public virtual T Singleton
        {
            get
            {
                if (base.SingletonInstance == null)
                {
                    return default(T);
                }
                Debug.Assert(base.SingletonInstance is T);
                return (T) base.SingletonInstance;
            }
        }
    }
}

