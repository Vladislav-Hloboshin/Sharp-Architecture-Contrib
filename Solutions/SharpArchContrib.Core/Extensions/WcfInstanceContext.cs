using System;
using System.ServiceModel;

namespace SharpArchContrib.Core.Extensions
{
    public class WcfInstanceContext : IExtension<InstanceContext>
    {
        private WcfInstanceContext()
        {
        }

        ///<summary>
        /// Stored in current instance context.
        ///</summary>
        public int TransactionDepth { get; set; }

        ///<summary>
        /// Gets the current instance of <see cref="WcfInstanceContext"/>
        ///</summary>
        public static WcfInstanceContext Current
        {
            get
            {
                if (OperationContext.Current == null || OperationContext.Current.InstanceContext == null)
                {
                    return null;
                }
                WcfInstanceContext context = OperationContext.Current.InstanceContext.Extensions.Find<WcfInstanceContext>();
                if (context == null)
                {
                    context = new WcfInstanceContext();
                    OperationContext.Current.InstanceContext.Extensions.Add(context);
                }
                return context;
            }
        }

        /// <summary>
        /// <see cref="IExtension{T}"/> Attach() method
        /// </summary>
        public void Attach(InstanceContext owner) { }

        /// <summary>
        /// <see cref="IExtension{T}"/> Detach() method
        /// </summary>
        public void Detach(InstanceContext owner) { }
    }
}
