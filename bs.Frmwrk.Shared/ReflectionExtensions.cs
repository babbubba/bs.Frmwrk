using bs.Data.Interfaces;
using bs.Data;
using bs.Frmwrk.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Common;

namespace bs.Frmwrk.Shared
{
    public static  class ReflectionExtensions
    {

        public static Type? GetTypeFromInterface(this Type interfaceType)
        {
            var result = interfaceType.GetTypesFromInterface();

            if (result is not null && result.Count() > 1 ) {
                throw new BsException(2212141235, $"There are more than one implementation of the interface or base class: '{interfaceType.FullName}'");
            }

            return result?.SingleOrDefault();
        }

        public static IEnumerable<Type?> GetTypesFromInterface(this Type interfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface);
        }

        public static IEnumerable<Type>? GetInterfacesOf(this Type concreteType, Type[]? excludedTypes = null)
        {

            var interfaces = concreteType.GetInterfaces().AsEnumerable();

            if(excludedTypes is not null && interfaces is not null)
            {
                foreach( var excludedType in excludedTypes)
                {
                    interfaces = interfaces.Where(i => i.FullName != excludedType.FullName);
                }
            }

            //if (interfaces is not null && interfaces.Count() > 1)
            //{
            //    throw new BsException(2212141431, $"There are more than one interface for the class: '{concreteType.FullName?? concreteType.Name}' ({string.Join(", ",interfaces.Select(i=>i.FullName ?? i.Name))})");
            //}

            return interfaces;// ?? throw new BsException(2212141235, $"No interface found for the class: '{concreteType.FullName ?? concreteType.Name}'");
        }

        public static Type? GetTypeByFullName(string typeFullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
           .SelectMany(s => s.GetTypes())
           .Where(t => t.FullName is not null && t.FullName == typeFullName)
           .SingleOrDefault();
        }

    }
}
