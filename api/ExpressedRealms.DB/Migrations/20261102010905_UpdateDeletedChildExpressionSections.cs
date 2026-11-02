using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeletedChildExpressionSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 WITH RECURSIVE DeletedAncestors AS (
                                     -- Start with deleted parents that have non-deleted children
                                     SELECT
                                         child.id,
                                         parent.is_deleted,
                                         parent.deleted_at
                                     FROM public.expression_sections child
                                     INNER JOIN public.expression_sections parent ON child.parent_id = parent.id
                                     WHERE parent.is_deleted = TRUE
                                       AND child.is_deleted = FALSE

                                     UNION ALL

                                     -- Find deeper descendants
                                     SELECT
                                         child.id,
                                         ancestor.is_deleted,
                                         ancestor.deleted_at
                                     FROM public.expression_sections child
                                     INNER JOIN DeletedAncestors ancestor ON child.parent_id = ancestor.id
                                     WHERE child.is_deleted = FALSE
                                 )
                                 update public.expression_sections target
                                 set is_deleted = ancestor.is_deleted,
                                 deleted_at = ancestor.deleted_at
                                 from DeletedAncestors ancestor
                                 where target.id = ancestor.id
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
