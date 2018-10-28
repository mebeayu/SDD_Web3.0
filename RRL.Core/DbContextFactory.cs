using RRL.EF;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace RRL.Core
{
    public enum DContext
    {
        RRl
    }

    public class DbContextFactory
    {
        public static DbContext GetDbContext()
        {
            DbContext obj = CallContext.GetData(typeof(RRlModelContainer).FullName) as DbContext;
            if (obj == null)
            {
                obj = new RRlModelContainer();

                CallContext.SetData(typeof(RRlModelContainer).FullName, obj);
            }

            return obj;
        }
    }
}