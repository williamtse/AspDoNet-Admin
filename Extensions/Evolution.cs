using Autofac;
using Admin.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Admin.Extensions
{
    public class Evolution: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblys = Assembly.Load("Admin");//Service是继承接口的实现方法类库名称
            var baseType = typeof(IDependency);//IDependency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            builder.RegisterAssemblyTypes(assemblys)
                  .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
                  .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
