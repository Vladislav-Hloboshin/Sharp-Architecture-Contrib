namespace WhoCanHelpMe.Infrastructure.NHibernateMaps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentNHibernate;
    using FluentNHibernate.Automapping;

    using SharpArch.Core.DomainModel;

    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.GetInterfaces().Any(x =>
                                         x.IsGenericType && 
                                         x.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>));

        }

        public override bool IsId(Member member)
        {
            return member.Name == "Id";
        }

        public override bool AbstractClassIsLayerSupertype(System.Type type)
        {
            return type == typeof(Entity) || type == typeof(EntityWithTypedId<>);
        }
    }
}