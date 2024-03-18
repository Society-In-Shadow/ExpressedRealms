namespace ExpressedRealms.DB.Interceptors;

public static class SoftDeleteExtensions
{
    public static void SoftDelete(this ISoftDelete softDelete)
    {
        softDelete.IsDeleted = true;
        softDelete.DeletedAt = DateTimeOffset.UtcNow;
    }
    public static void RestoreSoftDelete(this ISoftDelete softDelete)
    {
        softDelete.IsDeleted = false;
        softDelete.DeletedAt = null;
    }
}
