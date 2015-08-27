using System;
using System.Linq;
using AutoMapper;



namespace ResourceMetadata.API.Mappers
{
    public static class ConditionExtensions
    {
        public static void IgnoreIfSourceIsNull<T>(this IMemberConfigurationExpression<T> expression)
        {
            expression.Condition(IgnoreIfSourceIsNull);
        }

        static bool IgnoreIfSourceIsNull(ResolutionContext context)
        {
            if (!context.IsSourceValueNull)
            {
                return true;
            }
            var result = context.GetContextPropertyMap().ResolveValue(context.Parent);
            return result.Value != null;
        }
    }


    public static class MappingExtensions
    {
        //public static void InheritMappingFromBaseType<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
        //{
        //    var sourceType = typeof(TSource);
        //    var desctinationType = typeof(TDestination);
        //    var sourceParentType = sourceType.BaseType;
        //    var destinationParentType = desctinationType.BaseType;

        //    mappingExpression
        //        .BeforeMap((x, y) => Mapper.Map(x, y, sourceParentType, destinationParentType))
        //        .ForAllMembers(x => x.Condition(r => NotAlreadyMapped(sourceParentType, destinationParentType, r)));
        //}

        //public static void InheritMappingFromBaseModel<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression) 
        //    where TSource: IBaseModel where TDestination: BaseEntity
        //{
        //    mappingExpression
        //        .ForMember(x => x.Attribute, m => m.MapFrom(src => src.Attribute))
        //        .ForMember(x => x.Status, m => m.MapFrom(src => src.Status));
        //}

        //public static void InheritMappingFromBaseEntity<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
        //    where TSource : BaseEntity
        //    where TDestination : IBaseModel
        //{
        //    mappingExpression
        //        .AfterMap((a,b)=>b.IsVisible = b.Status == StatusObject.Enable);
        //}

        ////public static void InheritMappingFromBaseEntity<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
        ////    where TSource : BaseEntity
        ////    where TDestination : BaseModel<IAttribute>
        ////{
        ////    mappingExpression
        ////        .ForMember(x => x.AttributeObj, m => m.MapFrom(src => JavascriptUtil.Deserialize<IAttribute>(src.Attribute)));
        ////}

        

        private static bool NotAlreadyMapped(Type sourceType, Type desitnationType, ResolutionContext r)
        {
            return !r.IsSourceValueNull &&
                   Mapper.FindTypeMapFor(sourceType, desitnationType).GetPropertyMaps().Where(
                       m => m.DestinationProperty.Name.Equals(r.MemberName)).Select(y => !y.IsMapped()).All(b => b);
        }

        public class ProxyConverter<TSource, TDestination> : ITypeConverter<TSource, TDestination>
            where TSource : class
            where TDestination : class
        {
            public TDestination Convert(ResolutionContext context)
            {
                // Look up map for base type
                var sourceType = typeof(TSource);
                var desctinationType = typeof(TDestination);
                var sourceParentType = sourceType.BaseType;
                var destinationParentType = desctinationType.BaseType;

                return Mapper.DynamicMap(context.SourceValue, sourceParentType, destinationParentType) as TDestination;
            }
        } 
    }
}
