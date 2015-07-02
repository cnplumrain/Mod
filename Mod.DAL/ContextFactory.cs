using System.Runtime.Remoting.Messaging;

namespace Mod.DAL
{
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前数据上下文
        /// </summary>
        /// <returns></returns>
        public static ModContext GetCurrentContext()
        {
            var nContext = CallContext.GetData("ModContext") as ModContext;
            if (nContext == null)
            {
                nContext = new ModContext();
                CallContext.SetData("ModContext", nContext);
            }
            return nContext;
        }
      
    }
}
