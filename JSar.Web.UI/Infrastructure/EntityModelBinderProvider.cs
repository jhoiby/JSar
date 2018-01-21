//using JSar.Web.UI.Domain.Aggregates;

//namespace JSar.Web.UI.Infrastructure
//{
//    using Domain;
//    using Microsoft.AspNetCore.Mvc.ModelBinding;

//    public class EntityModelBinderProvider : IModelBinderProvider
//    {
//        public IModelBinder GetBinder(ModelBinderProviderContext context)
//        {
//            return typeof(IAggregateRoot).IsAssignableFrom(context.Metadata.ModelType) ? new EntityModelBinder() : null;
//        }
//    }
//}