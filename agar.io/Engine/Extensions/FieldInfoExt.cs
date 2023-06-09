using System.Reflection;

namespace agar.io.Engine.Extensions;

public static class FieldInfoExt
{
	public static bool IsSaveable(this FieldInfo fieldInfo)
	{
		return fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType == typeof(string) && !fieldInfo.IsLiteral;
	}
}