using SharpArchContrib.Core.Extensions;

namespace SharpArchContrib.Data.NHibernate
{
    using System;
    using System.Threading;

    using log4net;

    [Serializable]
    public abstract class TransactionManagerBase : ITransactionManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransactionManagerBase));

        [ThreadStatic]
        private static int _transactionDepth;

        public abstract string Name { get; }

        public int TransactionDepth
        {
            get
            {
                var currentWcfContext = WcfInstanceContext.Current;
                if (currentWcfContext == null)
                {
                    return _transactionDepth;
                }
                return currentWcfContext.TransactionDepth;
            }
        }

        public abstract object CommitTransaction(string factoryKey, object transactionState);

        public virtual object PopTransaction(string factoryKey, object transactionState)
        {
            var currentWcfContext = WcfInstanceContext.Current;
            if (currentWcfContext == null)
            {
                Interlocked.Decrement(ref _transactionDepth);
                Log(string.Format("[ThreadStatic] Pop Transaction to Depth {0}", _transactionDepth));
            }
            else
            {
                currentWcfContext.TransactionDepth--;
                Log(string.Format("[OperationContext({0})] Pop Transaction to Depth {1}", currentWcfContext.GetHashCode(), currentWcfContext.TransactionDepth));
            }
            return transactionState;
        }

        public virtual object PushTransaction(string factoryKey, object transactionState)
        {
            var currentWcfContext = WcfInstanceContext.Current;
            if (currentWcfContext == null)
            {
                Interlocked.Increment(ref _transactionDepth);
                Log(string.Format("[ThreadStatic] Push Transaction to Depth {0}", _transactionDepth));
            }
            else
            {
                currentWcfContext.TransactionDepth++;
                Log(string.Format("[OperationContext({0})] Push Transaction to Depth {1}", currentWcfContext.GetHashCode(), currentWcfContext.TransactionDepth));
            }
            return transactionState;
        }

        public abstract object RollbackTransaction(string factoryKey, object transactionState);

        public abstract bool TransactionIsActive(string factoryKey);

        protected void Log(string message)
        {
            Logger.Debug(string.Format("{0}: {1}", this.Name, message));
        }
    }
}