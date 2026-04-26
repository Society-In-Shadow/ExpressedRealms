CREATE OR REPLACE VIEW public.character_xp_view
AS
WITH calc AS (
    SELECT characters_1.id AS character_id,
           sum(blessing_levels.xp_cost) AS xp_total,
           1 AS section_type_id
    FROM characters characters_1
             JOIN character_blessing_mappings ON characters_1.id = character_blessing_mappings.character_id
             JOIN blessings ON blessings.id = character_blessing_mappings.blessing_id
             JOIN blessing_levels ON blessing_levels.id = character_blessing_mappings.blessing_level_id
    WHERE blessings.is_deleted = false AND character_blessing_mappings.is_deleted = false AND blessings.type::text = 'Advantage'::text
    GROUP BY characters_1.id
    UNION ALL
    SELECT characters_1.id AS character_id,
           sum(blessing_levels.xp_gain) AS xp_total,
           2 AS section_type_id
    FROM characters characters_1
             JOIN character_blessing_mappings ON characters_1.id = character_blessing_mappings.character_id
             JOIN blessings ON blessings.id = character_blessing_mappings.blessing_id
             JOIN blessing_levels ON blessing_levels.id = character_blessing_mappings.blessing_level_id
    WHERE blessings.is_deleted = false AND character_blessing_mappings.is_deleted = false AND blessings.type::text = 'Disadvantage'::text
    GROUP BY characters_1.id
    UNION ALL
    SELECT c.id AS character_id,
           SUM(kel.total_general_xp_cost) + (COALESCE(spec.spec_count, 0) * 2) AS xp_total,
           3 AS section_type_id
    FROM characters c
             JOIN character_knowledge_mappings ckm ON c.id = ckm.character_id
             JOIN knowledge_education_levels kel ON ckm.knowledge_level_id = kel.id
             LEFT JOIN (
        SELECT
            knowledge_mapping_id,
            COUNT(*) AS spec_count
        FROM character_knowledge_specializations
        WHERE is_deleted = false
        GROUP BY knowledge_mapping_id
    ) spec ON ckm.id = spec.knowledge_mapping_id
    WHERE ckm.is_deleted = false
    GROUP BY c.id, spec.spec_count
    UNION ALL
    SELECT characters_1.id AS character_id,
           sum(power_levels.xp) AS xp_total,
           4 AS section_type_id
    FROM characters characters_1
             JOIN character_power_mappings ON characters_1.id = character_power_mappings.character_id
             JOIN power_levels ON power_levels.id = character_power_mappings.power_level_id
    WHERE character_power_mappings.is_deleted = false
    GROUP BY characters_1.id
    UNION ALL
    SELECT characters_1.id AS character_id,
           sum(skill_levels.total_xp) AS total_xp,
           5 AS section_type_id
    FROM characters characters_1
             JOIN character_skills_mappings ON characters_1.id = character_skills_mappings.character_id
             JOIN skill_levels ON character_skills_mappings.skill_level_id = skill_levels.id
    GROUP BY characters_1.id
    UNION ALL
    SELECT characters_1.id AS character_id,
           agility.total_xp_cost + con.total_xp_cost + dex.total_xp_cost + stre.total_xp_cost + inti.total_xp_cost + wil.total_xp_cost AS xp_total,
           6 AS section_type_id
    FROM characters characters_1
             JOIN stat_levels agility ON characters_1.agility_id = agility.id
             JOIN stat_levels con ON characters_1.constitution_id = con.id
             JOIN stat_levels dex ON characters_1.dexterity_id = dex.id
             JOIN stat_levels stre ON characters_1.strength_id = stre.id
             JOIN stat_levels inti ON characters_1.intelligence_id = inti.id
             JOIN stat_levels wil ON characters_1.willpower_id = wil.id
    UNION ALL
    SELECT characters_1.id AS character_id,
           sum(contacts.spent_xp) AS total_xp,
           8 AS section_type_id
    FROM characters characters_1
             JOIN contacts ON characters_1.id = contacts.character_id
    where contacts.is_deleted = false
    GROUP BY characters_1.id
)
SELECT characters.id AS character_id,
       xp_section_types.name AS section_name,
       xp_section_types.id AS section_type_id,
       xp_section_types.section_cap AS section_cap,
       COALESCE(case when xp_section_types.id = 2 then calc.xp_total when xp_section_types.id = 1 then 0 else xp_section_types.section_cap end, 0) as true_section_cap,
       COALESCE(case when xp_section_types.id = 2 then 0 else calc.xp_total end, 0) as true_total_spent,
       COALESCE(calc.xp_total, 0::bigint) AS spent_xp,
       CASE
           WHEN calc.section_type_id = 1 THEN calc.xp_total
           ELSE COALESCE(calc.xp_total - xp_section_types.section_cap, 0::bigint)
           END AS discretion_xp,
       xp_mappings.total_character_creation_xp AS total_character_creation_xp,
       COALESCE(calc.xp_total - xp_mappings.total_character_creation_xp, 0) AS level_xp
FROM characters
         CROSS JOIN xp_section_types
         LEFT JOIN calc ON calc.character_id = characters.id AND calc.section_type_id = xp_section_types.id
         LEFT JOIN public.character_xp_mappings xp_mappings
                   ON xp_mappings.character_id = characters.id
                       AND xp_mappings.xp_section_type_id = xp_section_types.id;