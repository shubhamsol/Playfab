using System;
using System.Collections.Generic;

namespace Helper
{
    public interface ISignal
    {
        string Hash { get; }
    }

    public static class Signals
    {
        private static readonly SignalHub Hub = new SignalHub();

        public static TSType Get<TSType>() where TSType : ISignal, new()
        {
            return Hub.Get<TSType>();
        }

        public static void AddListenerToHash(string signalHash, Action handler)
        {
            Hub.AddListenerToHash(signalHash, handler);
        }

        public static void RemoveListenerFromHash(string signalHash, Action handler)
        {
            Hub.RemoveListenerFromHash(signalHash, handler);
        }

    }
    public class SignalHub
    {
        private readonly Dictionary<Type, ISignal> _signals = new Dictionary<Type, ISignal>();

        public TSType Get<TSType>() where TSType : ISignal, new()
        {
            Type signalType = typeof(TSType);
            ISignal signal;

            if (_signals.TryGetValue (signalType, out signal)) 
            {
                return (TSType)signal;
            }

            return (TSType)Bind(signalType);
        }

        public void AddListenerToHash(string signalHash, Action handler)
        {
            ISignal signal = GetSignalByHash(signalHash);
            if(signal != null && signal is ASignal)
            {
                (signal as ASignal).AddListener(handler);
            }
        }

        public void RemoveListenerFromHash(string signalHash, Action handler)
        {
            ISignal signal = GetSignalByHash(signalHash);
            if (signal != null && signal is ASignal)
            {
                (signal as ASignal).RemoveListener(handler);
            }
        }

        private ISignal Bind(Type signalType)
        {
            ISignal signal;
            if(_signals.TryGetValue(signalType, out signal))
            {
                UnityEngine.Debug.LogError(string.Format("Signal already registered for type {0}", signalType));
                return signal;
            }

            signal = (ISignal)Activator.CreateInstance(signalType);
            _signals.Add(signalType, signal);
            return signal;
        }

        private ISignal Bind<T>() where T : ISignal, new()
        {
            return Bind(typeof(T));
        }

        private ISignal GetSignalByHash(string signalHash)
        {
            foreach (ISignal signal in _signals.Values)
            {
                if (signal.Hash == signalHash)
                {
                    return signal;
                }
            }

            return null;
        }
    }

    public abstract class ABaseSignal : ISignal
    {
        private string _hash;

        public string Hash
        {
            get
            {
                if (string.IsNullOrEmpty(_hash))
                {
                    _hash = this.GetType().ToString();
                }
                return _hash;
            }
        }
    }

    public abstract class ASignal : ABaseSignal
    {
        private Action _callback;

        public void AddListener(Action handler)
        {
            _callback += handler;
        }

        public void RemoveListener(Action handler)
        {
            _callback -= handler;
        }

        public void Dispatch()
        {
            if(_callback != null)
            {
                _callback();
            }
        }
    }
    public abstract class ASignal<T>: ABaseSignal
    {
        private Action<T> _callback;
        public void AddListener(Action<T> handler)
        {
            _callback += handler;
        }

        public void RemoveListener(Action<T> handler)
        {
            _callback -= handler;
        }

        public void Dispatch(T arg1)
        {
            if (_callback != null)
            {
                _callback(arg1);
            }
        }
    }

    
}
