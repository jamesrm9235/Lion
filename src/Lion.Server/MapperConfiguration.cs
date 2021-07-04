using AutoMapper;
using Lion.Server.Models;
using System.Collections.Generic;

namespace Lion.Server
{
    public sealed class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<Common.Entities.Bundle, Models.Bundle>();

            CreateMap<Common.Entities.Bundle, Models.CompactBundle>().ConvertUsing<CompactBundleTypeConverter>();

            CreateMap<Common.Entities.Message, Models.Message>();

            CreateMap<Common.Entities.Namespace, Models.Namespace>();
        }

        private sealed class CompactBundleTypeConverter : ITypeConverter<Common.Entities.Bundle, Models.CompactBundle>
        {
            public CompactBundle Convert(Common.Entities.Bundle source, CompactBundle destination, ResolutionContext context)
            {
                var messages = new Dictionary<string, string>();
                foreach (var message in source.Messages)
                {
                    messages.Add(message.Language, message.Value);
                }
                return new CompactBundle(source.Id, source.Namespace.Key, source.Key, messages);
            }
        }
    }
}
