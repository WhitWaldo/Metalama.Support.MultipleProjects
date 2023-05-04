//using Metalama.Framework.Aspects;
//using Metalama.Framework.Code;

//namespace Metalama.Bits.Repository;

//[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
//public class IndexedAttribute : MethodAspect
//{
//    public override void BuildAspect(IAspectBuilder<IMethod> builder)
//    {
//        if (!builder.Target.IsGeneric)
//            return;
//        var declaringType = builder.Target.TypeParameters.First();

//        //Identify every string or numeric property (must be non-static and public) on the type
//        var stringTypes = declaringType.AllProperties.Where(property => property.Type.Is(typeof(string))).ToList();
//        var integerTypes = declaringType.AllProperties.Where(prop =>
//            prop.Type.Is(typeof(byte)) || prop.Type.Is(typeof(short)) || prop.Type.Is(typeof(int)) ||
//            prop.Type.Is(typeof(long)));
//        var decimalTypes = declaringType.AllProperties.Where(property =>
//            property.Type.Is(typeof(float)) || property.Type.Is(typeof(decimal)) || property.Type.Is(typeof(double)));
        
//        //Create an index property (dictionary works for now) for each property (Key: type, Value: List<Guid>)
//        //Also create a method that can be queried against for the index property
//        foreach (var p in declaringType.AllProperties)
//        {
//            DataType dataType;
//            if (p.Type.Is(typeof(string)))
//                dataType = DataType.String;
//            else if (p.Type.Is(typeof(byte)) || p.Type.Is(typeof(short)) || p.Type.Is(typeof(int)) ||
//                     p.Type.Is(typeof(long)))
//                dataType = DataType.Integer;
//            else if (p.Type.Is(typeof(float)) || p.Type.Is(typeof(decimal)) || p.Type.Is(typeof(double)))
//                dataType = DataType.Decimal;
//            else
//            //Nothing to do if it's not an "indexable" type
//                continue;
            
//            var listType = ((INamedType)TypeFactory.GetType(typeof(List<>))).WithTypeArguments(typeof(Guid));
//            var dictType =
//                ((INamedType) TypeFactory.GetType(typeof(Dictionary<,>))).WithTypeArguments(p.Type.ToType(), listType.ToType());

//            builder.Advice.IntroduceField(builder.Target.DeclaringType, $"{p.Name}_Index", dictType.ToType(),
//                IntroductionScope.Instance, OverrideStrategy.Ignore, null, null);


//        }

//        builder.Advice.IntroduceMethod(builder.Target.DeclaringType, "FilterFunc", IntroductionScope.Instance, OverrideStrategy.Ignore,  )
//    }

//    [RunTimeOrCompileTime]
//    internal enum DataType
//    {
//        Integer,
//        String,
//        Decimal
//    }
//}