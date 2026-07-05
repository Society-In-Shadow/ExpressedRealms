CREATE OR REPLACE PROCEDURE copy_expression(
    source_expression_id     INT,
    expression_name          TEXT,
    INOUT new_expression_id  INT
)
    LANGUAGE plpgsql
AS $$
DECLARE v_clone_batch_id uuid := gen_random_uuid();
BEGIN

-- Copy
-- Expression


insert into public.expressions (name, short_description, nav_menu_image, publish_status_id, expression_type_id, order_index)
select expression_name, short_description, nav_menu_image, 3, expression_type_id, order_index from public.expressions
where id = source_expression_id
RETURNING id INTO new_expression_id;


-- Copy Over Stat Modifiers across all mappings in expressions


-- Copy over stat modifier groups - they are the lowest hanging fruit
CREATE TEMP TABLE stat_modifer_group_ids (
    old_id bigint NOT NULL,
    new_id bigint NULL,
    PRIMARY KEY (old_id)
);

-- all stat modifier groups in powers and power paths
insert into stat_modifer_group_ids (old_id)
select stat_modifier_group_id from public.powers
                                       join public.power_path_power_mappings on powers.id = power_path_power_mappings.power_id
                                       join public.power_paths on power_path_power_mappings.power_path_id = power_paths.id
where power_paths.expression_id = source_expression_id and powers.stat_modifier_group_id is not null;

-- All modifiers in progression paths
insert into stat_modifer_group_ids(old_id)
select stat_modifier_group_id from public.progression_level
                                       join public.progression_path on progression_path.id = progression_level.progression_path_id
where progression_path.expression_id = source_expression_id and progression_level.stat_modifier_group_id is not null;

-- All modifiers in powers for a faction
insert into stat_modifer_group_ids(old_id)
select stat_modifier_group_id from public.powers
                                       join public.faction_levels on powers.id = faction_levels.power_id
                                       join public.factions on faction_levels.faction_id = factions.id
where factions.expression_id = source_expression_id and powers.stat_modifier_group_id is not null;

-- Add new modifier groups, tie to old id's - this time they don't need to match up 1:1, I just need new blank groups
-- to insert into

insert into public.stat_modifier_groups(clone_source_id, clone_batch_id)
select id, v_clone_batch_id from public.stat_modifier_groups 
join stat_modifer_group_ids on stat_modifer_group_ids.old_id = stat_modifier_groups.id;

update stat_modifer_group_ids set new_id = stat_modifier_groups.id from stat_modifier_groups
where stat_modifier_groups.clone_source_id = stat_modifer_group_ids.old_id and stat_modifier_groups.clone_batch_id = v_clone_batch_id;
    

insert into public.stat_group_mappings(stat_group_id, stat_modifier_id, modifier, scale_with_level, creation_specific_bonus, target_expression_id)
select stat_modifer_group_ids.new_id, stat_modifier_id, modifier, scale_with_level, creation_specific_bonus,
       CASE
           WHEN target_expression_id = source_expression_id
               THEN new_expression_id
           ELSE stat_group_mappings.target_expression_id
           END AS expression_id from public.stat_group_mappings
 join stat_modifer_group_ids on stat_modifer_group_ids.old_id = stat_group_mappings.stat_group_id;


-- Copy Powers for all expression mappings


create temp table power_ids (
    old_id bigint NOT NULL,
    new_id bigint NULL,
    PRIMARY KEY (old_id)
);

-- All powers in powers and power paths
insert into power_ids (old_id)
select power_id from public.powers
                                       join public.power_path_power_mappings on powers.id = power_path_power_mappings.power_id
                                       join public.power_paths on power_path_power_mappings.power_path_id = power_paths.id
where power_paths.expression_id = source_expression_id and powers.is_deleted = false and power_paths.is_deleted = false;

-- All powers in Factions
insert into power_ids (old_id)
select power_id from public.powers
                                       join public.faction_levels on powers.id = faction_levels.power_id
                                       join public.factions on faction_levels.faction_id = factions.id
where factions.expression_id = source_expression_id and powers.is_deleted = false;

-- Copy Powers Over
insert into public.powers(name, description, level_id, area_of_effect_type_id, activation_timing_type_id, duration_id, is_power_use, game_mechanic_effect, limitation, other_fields, is_deleted, deleted_at, cost, stat_modifier_group_id, clone_source_id, clone_batch_id)
select name, description, level_id, area_of_effect_type_id, activation_timing_type_id, duration_id, is_power_use, game_mechanic_effect, limitation, other_fields, is_deleted, deleted_at, cost, stat_modifer_group_ids.new_id, powers.id, v_clone_batch_id from public.powers
                                         join power_ids on powers.id = power_ids.old_id
                                         left join stat_modifer_group_ids on powers.stat_modifier_group_id = stat_modifer_group_ids.old_id;

update power_ids set new_id = powers.id from powers 
where powers.clone_source_id = power_ids.old_id and powers.clone_batch_id = v_clone_batch_id;

-- Copy over Categories
insert into public.power_category_mappings(power_id, category_id)
select power_ids.new_id, category_id 
from public.power_category_mappings 
join power_ids on power_ids.old_id = power_category_mappings.power_id;

-- Copy over prerequisites
insert into public.power_prerequisites(power_id, required_amount, clone_source_id, clone_batch_id)
select power_ids.new_id, required_amount, power_prerequisites.id, v_clone_batch_id from public.power_prerequisites
join power_ids on power_ids.old_id = power_prerequisites.power_id;

with power_prerequisites_ids as(
    select id as new_id, clone_source_id as old_id
    from public.power_prerequisites
    where clone_batch_id = v_clone_batch_id
)
insert into public.power_prerequisite_powers(prerequisite_id, power_id) 
select power_prerequisites_ids.new_id, power_ids.new_id from public.power_prerequisite_powers
join power_prerequisites_ids on power_prerequisites_ids.old_id = power_prerequisite_powers.prerequisite_id
join power_ids on power_ids.old_id = power_prerequisite_powers.power_id;


-- Copy Factions


insert into public.factions(expression_id, name, background, is_deleted, deleted_at, clone_batch_id, clone_source_id)
select new_expression_id, name, background, is_deleted, deleted_at, v_clone_batch_id, id from public.factions 
where expression_id = source_expression_id and is_deleted = false;

-- Insert faction levels, complete with new power mappings
with faction_ids as (
    select id as new_id, clone_source_id as old_id
    from public.factions
    where factions.clone_batch_id = v_clone_batch_id
)
insert into public.faction_levels(faction_id, faction_rank_id, knowledge_id, knowledge_level_id, specialization, is_deleted, deleted_at, power_id, clone_batch_id, clone_source_id)
select faction_ids.new_id, faction_rank_id, knowledge_id, knowledge_level_id, specialization, is_deleted, deleted_at, power_ids.new_id, v_clone_batch_id, faction_levels.id from public.faction_levels
join faction_ids on faction_ids.old_id = faction_levels.faction_id
left join power_ids on power_ids.old_id = faction_levels.power_id
where faction_levels.is_deleted = false;


-- Copy Power Paths + Power Path Mappings


create temp table power_path_ids (
    old_id bigint NOT NULL,
    new_id bigint NOT NULL,
    PRIMARY KEY (old_id)
);

insert into public.power_paths (name, description, expression_id, order_index, is_deleted, clone_source_id, clone_batch_id)
select name, description, new_expression_id, order_index, false, id, v_clone_batch_id from public.power_paths
where expression_id = source_expression_id and is_deleted = false;

with power_path_ids as (
    select id as new_id, clone_source_id as old_id
    from public.power_paths
    where power_paths.clone_batch_id = v_clone_batch_id
)
insert into public.power_path_power_mappings (power_path_id, power_id, order_index)
select power_path_ids.new_id, power_ids.new_id, order_index from public.power_path_power_mappings
join power_path_ids on power_path_ids.old_id = power_path_power_mappings.power_path_id
join power_ids on power_ids.old_id = power_path_power_mappings.power_id;


-- Copy Progression Paths + Progression Levels


insert into public.progression_path (expression_id, name, description, is_deleted, deleted_at, clone_source_id, clone_batch_id)
select new_expression_id, name, description, false, null, id, v_clone_batch_id from public.progression_path
where expression_id = source_expression_id and is_deleted = false;

with progression_path_ids as (
    select id as new_id, clone_source_id as old_id
    from public.progression_path
    where progression_path.clone_batch_id = v_clone_batch_id
)
insert into public.progression_level(progression_path_id, xl_level, description, is_deleted, deleted_at, stat_modifier_group_id) 
select progression_path_ids.new_id, xl_level, description, false, null, stat_modifer_group_ids.new_id from public.progression_level
join progression_path_ids on progression_path_ids.old_id = progression_level.progression_path_id
join stat_modifer_group_ids on stat_modifer_group_ids.old_id = progression_level.stat_modifier_group_id
where progression_level.is_deleted = false;


-- Expression Sections

insert into public.expression_sections(expression_id, section_type_id, parent_id, name, content, order_index, clone_source_id, clone_batch_id)
select new_expression_id, section_type_id, parent_id, name, content, order_index, id, v_clone_batch_id from public.expression_sections
where expression_id = source_expression_id and is_deleted = false;

-- Update parent ids
WITH mapping AS (
    SELECT
        id AS new_id,
        clone_source_id AS old_id
    FROM expression_sections
    WHERE clone_batch_id = v_clone_batch_id
)
UPDATE expression_sections c
SET parent_id = m_parent.new_id
FROM mapping m
         JOIN expression_sections original
              ON original.id = m.old_id
         LEFT JOIN mapping m_parent
                   ON m_parent.old_id = original.parent_id
WHERE c.id = m.new_id;

commit;
END
$$
