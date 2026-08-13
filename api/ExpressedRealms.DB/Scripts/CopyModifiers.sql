CREATE OR REPLACE PROCEDURE copy_modifiers(
    p_clone_batch_id     uuid
)
LANGUAGE plpgsql
AS $$
BEGIN

-- Add new modifier groups, tie to old id's - this time they don't need to match up 1:1, I just need new blank groups
-- to insert into

insert into public.stat_modifier_groups(clone_source_id, clone_batch_id)
select id, p_clone_batch_id from public.stat_modifier_groups
                                     join stat_modifer_group_ids on stat_modifer_group_ids.old_id = stat_modifier_groups.id;

update stat_modifer_group_ids set new_id = stat_modifier_groups.id from stat_modifier_groups
where stat_modifier_groups.clone_source_id = stat_modifer_group_ids.old_id and stat_modifier_groups.clone_batch_id = p_clone_batch_id;

insert into public.stat_group_mappings(stat_group_id, stat_modifier_id, modifier, scale_with_level, creation_specific_bonus, target_expression_id)
select stat_modifer_group_ids.new_id, stat_modifier_id, modifier, scale_with_level, creation_specific_bonus, target_expression_id from public.stat_group_mappings
 join stat_modifer_group_ids on stat_modifer_group_ids.old_id = stat_group_mappings.stat_group_id;

END
$$
