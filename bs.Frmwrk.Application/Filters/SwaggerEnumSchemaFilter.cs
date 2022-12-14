using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace bs.Frmwrk.Application.Filters
{
    public class SwaggerEnumSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name => model.Enum.Add(new OpenApiString($"{Convert.ToInt64(Enum.Parse(context.Type, name))} - {name}")));

                var names = Enum.GetNames(context.Type).ToList();
                model.Example = new OpenApiInteger(Convert.ToInt32(Enum.Parse(context.Type, names.First())));
            }
        }
    }
}