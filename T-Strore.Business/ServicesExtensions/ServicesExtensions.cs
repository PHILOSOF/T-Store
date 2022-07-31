

namespace T_Strore.Business.ServicesExtensions;

public static class ServicesExtensions
{

    public static IDictionary<string, string> EnumToDictionary(this Type t)
    {
        if (t == null) throw new NullReferenceException();
        if (!t.IsEnum) throw new InvalidCastException("object is not an Enumeration");

        string[] names = Enum.GetNames(t);
        Array values = Enum.GetValues(t);

        return (from i in Enumerable.Range(0, names.Length)
                select new { Key = names[i], Value = (int)values.GetValue(i) * 10 })
                    .ToDictionary(k => k.Key, k => k.Value.ToString());
    }
}
