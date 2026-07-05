CREATE OR REPLACE PROCEDURE copy_character_to_player_proc(
    source_expression_id     INT,
    expression_name          TEXT,
    INOUT new_expression_id  INT
)
    LANGUAGE plpgsql
AS $$
DECLARE clone_batch_id uuid := gen_random_uuid();
BEGIN

-- Copy
-- Expression


insert into public.expressions (name, short_description, nav_menu_image, publish_status_id, expression_type_id, order_index)
select expression_name, short_description, nav_menu_image, 3, expression_type_id, order_index from public.expressions
where id = source_expression_id
RETURNING id INTO new_expression_id;

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
where power_paths.expression_id = source_expression_id;

-- All modifiers in progression paths
insert into stat_modifer_group_ids(old_id)
select stat_modifier_group_id from public.progression_level
                                       join public.progression_path on progression_path.id = progression_level.progression_path_id
where progression_path.expression_id = source_expression_id;

-- All modifiers in powers for a faction
insert into stat_modifer_group_ids(old_id)
select stat_modifier_group_id from public.powers
                                       join public.faction_levels on powers.id = faction_levels.power_id
                                       join public.factions on faction_levels.faction_id = factions.id
where factions.expression_id = source_expression_id;

-- Add new modifier groups, tie to old id's - this time they don't need to match up 1:1, I just need new blank groups
-- to insert into
WITH old_rows AS (
    SELECT
        old_id,
        row_number() OVER (ORDER BY old_id) AS rn
    FROM stat_modifer_group_ids
),
     inserted AS (
         INSERT INTO public.stat_modifier_groups
             DEFAULT VALUES
             RETURNING id
     ),
     new_rows AS (
         SELECT
             id AS new_id,
             row_number() OVER (ORDER BY id) AS rn
         FROM inserted
     )
INSERT INTO stat_modifer_group_ids (old_id, new_id)
SELECT
    o.old_id,
    n.new_id
FROM old_rows o
         JOIN new_rows n
              ON o.rn = n.rn;

insert into public.stat_group_mappings(stat_group_id, stat_modifier_id, modifier, scale_with_level, creation_specific_bonus, target_expression_id)
select stat_modifer_group_ids.new_id, stat_modifier_id, modifier, scale_with_level, creation_specific_bonus,
       CASE
           WHEN target_expression_id = source_expression_id
               THEN new_expression_id
           ELSE new_expression_id
           END AS expression_id from public.stat_group_mappings
                                         join stat_modifer_group_ids on stat_modifer_group_ids.old_id = stat_group_mappings.stat_group_id;


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
select name, description, level_id, area_of_effect_type_id, activation_timing_type_id, duration_id, is_power_use, game_mechanic_effect, limitation, other_fields, is_deleted, deleted_at, cost, stat_modifer_group_ids.new_id, powers.id, clone_batch_id from public.powers
                                         join power_ids on powers.id = power_ids.old_id
                                         left join stat_modifer_group_ids on powers.stat_modifier_group_id = stat_modifer_group_ids.old_id;

update power_ids set new_id = powers.id from powers 
where powers.clone_source_id = power_ids.old_id and powers.clone_batch_id = clone_batch_id;


-- Copy Factions

create temp table faction_ids (
    old_id bigint NOT NULL,
    new_id bigint NOT NULL,
    PRIMARY KEY (old_id)
);

insert into public.factions(expression_id, name, background, is_deleted, deleted_at, clone_batch_id, clone_source_id)
select new_expression_id, name, background, is_deleted, deleted_at, clone_batch_id, id from public.factions 
where expression_id = source_expression_id and is_deleted = false;

-- Get old / new key combo
insert into faction_ids (new_id, old_id)
select id, clone_source_id from public.factions where expression_id = new_expression_id and clone_batch_id == clone_batch_id;

-- Insert faction levels, complete with new power mappings
insert into public.faction_levels(faction_id, faction_rank_id, knowledge_id, knowledge_level_id, specialization, is_deleted, deleted_at, power_id, clone_batch_id, clone_source_id)
select faction_ids.new_id, faction_rank_id, knowledge_id, knowledge_level_id, specialization, is_deleted, deleted_at, power_ids.new_id, clone_batch_id, powers.id from public.faction_levels
join faction_ids on faction_ids.old_id = faction_levels.faction_id
left join power_ids on power_ids.old_id = faction_levels.power_id
where faction_levels.is_deleted = false;

-- Power Paths

insert into public.power_paths (name, description, expression_id, order_index, is_deleted)
select name, description, new_expression_id, order_index, false from public.power_paths
where expression_id = source_expression_id and is_deleted = false;

-- Power Paths
WITH source AS (
    SELECT id, name, description, order_index
    FROM power_paths
    WHERE expression_id = source_expression_id and is_deleted = false
),
inserted AS (
    insert into public.power_paths (name, description, expression_id, order_index, is_deleted)
    select name, description, new_expression_id, order_index, false from source
    RETURNING id, order_index
)
INSERT INTO id_map (table_name, old_id, new_id)
SELECT
    'power_paths',
    s.id,
    i.id
FROM source s
JOIN inserted i
    ON i.order_index = s.order_index;


-- Power Path Mappings
WITH source AS (
    SELECT id, id_map.new_id, power_id, order_index
    FROM power_path_power_mappings
    join id_map on id_map.old_id = power_path_id and id_map.table_name = 'power_paths'
),
     inserted AS (
         insert into public.power_path_power_mappings (power_path_id, power_id, order_index)
             select name, description, new_expression_id, order_index, false from source
             RETURNING id, order_index
     )
INSERT INTO id_map (table_name, old_id, new_id)
SELECT
    'power_paths',
    s.id,
    i.id
FROM source s
         JOIN inserted i
              ON i.order_index = s.order_index;
-- Power Path Mappings
-- powers (subset of copying)
--   Stat Modifiers (It's own subset of issues)
--   Categories
--   Prerequisite




-- Progression Path
-- Progression Levels
-- Stat Modifiers (It's own subset of issues)
--   Copy Group
--   Copy Group Mappings
--   Will need new Expression Ids for power specific stuff

-- Factions
-- Faction Levels
-- Powers (subset of copying)
--   Stat Modifiers (It's own subset of issues)
--   Categories




commit
$$
