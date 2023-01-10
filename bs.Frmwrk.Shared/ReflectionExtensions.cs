using bs.Frmwrk.Core.Exceptions;

namespace bs.Frmwrk.Shared
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type>? GetInterfacesOf(this Type concreteType, Type[]? excludedTypes = null)
        {
            var interfaces = concreteType.GetInterfaces().AsEnumerable();

            if (excludedTypes is not null && interfaces is not null)
            {
                foreach (var excludedType in excludedTypes)
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

        public static Type? GetTypeFromInterface(this Type interfaceType)
        {
            var result = interfaceType.GetTypesFromInterface();

            if (result is not null && result.Count() > 1)
            {
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

        /// <summary>
        /// Gets the attribute of the memeber if the memeber has the attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public static T? GetAttributeOfMember<T>(this object member) where T : System.Attribute
        {
            if (member == null) return null;
            var type = member.GetType();
            string? memberName = member.ToString();

            if (memberName == null) return null;
            var memInfo = type.GetMember(memberName);

            if (memInfo == null || memInfo.Length == 0) return null;
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}