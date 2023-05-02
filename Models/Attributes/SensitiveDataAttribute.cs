﻿namespace Models.Attributes;

[AttributeUsage(AttributeTargets.Field|AttributeTargets.Parameter|AttributeTargets.GenericParameter|AttributeTargets.ReturnValue|AttributeTargets.Property)]
internal class SensitiveDataAttribute : Attribute
{
}