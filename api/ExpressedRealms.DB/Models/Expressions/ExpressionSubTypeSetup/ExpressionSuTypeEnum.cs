using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSubTypeSetup;

public sealed class ExpressionSubTypeEnum : SmartEnum<ExpressionSubTypeEnum, int>
{

    private ExpressionSubTypeEnum(string name, int id)
        : base(name, id)
    {}

    public static readonly ExpressionSubTypeEnum Adepts = new("Adepts", 1);
    public static readonly ExpressionSubTypeEnum Aeternari = new("Aeternari", 2);
    public static readonly ExpressionSubTypeEnum Shammas = new("Shammas", 3);
    public static readonly ExpressionSubTypeEnum Sidhe = new("Sidhe", 4);
    public static readonly ExpressionSubTypeEnum Sorcerers = new("Sorcerers", 5);
    public static readonly ExpressionSubTypeEnum Vampyre = new("Vampyre", 6);
}
