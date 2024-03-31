using System;

[AttributeUsage(AttributeTargets.Field)]
public class TypeConstructionField : Attribute
{
}

[AttributeUsage(AttributeTargets.Field)]
public class MethodConstructionField : Attribute
{
}