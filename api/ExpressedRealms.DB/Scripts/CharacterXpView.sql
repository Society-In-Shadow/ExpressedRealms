CREATE OR REPLACE VIEW public.character_xp_view
 AS
 WITH calc AS (
         SELECT "Characters_1"."Id" AS character_id,
            sum(blessing_level.xp_cost) AS xp_total,
            1 AS section_type_id
           FROM "Characters" "Characters_1"
             JOIN character_blessing_mapping ON "Characters_1"."Id" = character_blessing_mapping.character_id
             JOIN blessing ON blessing.id = character_blessing_mapping.blessing_id
             JOIN blessing_level ON blessing_level.id = character_blessing_mapping.blessing_level_id
          WHERE blessing.is_deleted = false AND character_blessing_mapping.is_deleted = false AND blessing.type::text = 'Advantage'::text
          GROUP BY "Characters_1"."Id"
        UNION ALL
         SELECT "Characters_1"."Id" AS character_id,
            sum(blessing_level.xp_gain) AS xp_total,
            2 AS section_type_id
           FROM "Characters" "Characters_1"
             JOIN character_blessing_mapping ON "Characters_1"."Id" = character_blessing_mapping.character_id
             JOIN blessing ON blessing.id = character_blessing_mapping.blessing_id
             JOIN blessing_level ON blessing_level.id = character_blessing_mapping.blessing_level_id
          WHERE blessing.is_deleted = false AND character_blessing_mapping.is_deleted = false AND blessing.type::text = 'Disadvantage'::text
          GROUP BY "Characters_1"."Id"
        UNION ALL
         SELECT "Characters_1"."Id" AS character_id,
            sum(knowledge_education_level.total_general_xp_cost) + count(character_knowledge_specialization.id) * 2 AS xp_total,
            3 AS section_type_id
           FROM "Characters" "Characters_1"
             JOIN character_knowledge_mapping ON "Characters_1"."Id" = character_knowledge_mapping.character_id
             LEFT JOIN character_knowledge_specialization ON character_knowledge_mapping.id = character_knowledge_specialization.knowledge_mapping_id AND character_knowledge_specialization.is_deleted = false
             JOIN knowledge_education_level ON character_knowledge_mapping.knowledge_level_id = knowledge_education_level.id
          WHERE character_knowledge_mapping.is_deleted = false
          GROUP BY "Characters_1"."Id"
        UNION ALL
         SELECT "Characters_1"."Id" AS character_id,
            sum(power_level.xp) AS xp_total,
            4 AS section_type_id
           FROM "Characters" "Characters_1"
             JOIN character_power_mapping ON "Characters_1"."Id" = character_power_mapping.character_id
             JOIN power_level ON power_level.id = character_power_mapping.power_level_id
          WHERE character_power_mapping.is_deleted = false
          GROUP BY "Characters_1"."Id"
        UNION ALL
         SELECT "Characters_1"."Id" AS character_id,
            sum(skill_level.total_xp) AS total_xp,
            5 AS section_type_id
           FROM "Characters" "Characters_1"
             JOIN "CharacterSkillsMapping" ON "Characters_1"."Id" = "CharacterSkillsMapping"."CharacterId"
             JOIN skill_level ON "CharacterSkillsMapping"."SkillLevelId" = skill_level.id
          GROUP BY "Characters_1"."Id"
        UNION ALL
         SELECT "Characters_1"."Id" AS character_id,
            agility."TotalXPCost" + con."TotalXPCost" + dex."TotalXPCost" + stre."TotalXPCost" + inti."TotalXPCost" + wil."TotalXPCost" AS xp_total,
            6 AS section_type_id
           FROM "Characters" "Characters_1"
             JOIN "StatLevels" agility ON "Characters_1"."AgilityId" = agility."Id"
             JOIN "StatLevels" con ON "Characters_1"."ConstitutionId" = con."Id"
             JOIN "StatLevels" dex ON "Characters_1"."DexterityId" = dex."Id"
             JOIN "StatLevels" stre ON "Characters_1"."StrengthId" = stre."Id"
             JOIN "StatLevels" inti ON "Characters_1"."IntelligenceId" = inti."Id"
             JOIN "StatLevels" wil ON "Characters_1"."WillpowerId" = wil."Id"
        UNION ALL
         SELECT "Characters_1"."Id" AS character_id,
            sum(contact.spent_xp) AS total_xp,
            8 AS section_type_id
           FROM "Characters" "Characters_1"
             JOIN contact ON "Characters_1"."Id" = contact.character_id
           where contact.is_deleted = false
          GROUP BY "Characters_1"."Id"
        )
SELECT "Characters"."Id" AS character_id,
    xp_section_type.name AS section_name,
       xp_section_type.id AS section_type_id,
       xp_section_type.creation_cap AS section_cap,
       COALESCE(case when xp_section_type.id = 2 then calc.xp_total when xp_section_type.id = 1 then 0 else xp_section_type.creation_cap end, 0) as true_section_cap,
       COALESCE(case when xp_section_type.id = 2 then 0 else calc.xp_total end, 0) as true_total_spent,
       COALESCE(calc.xp_total, 0::bigint) AS spent_xp,
       CASE
           WHEN calc.section_type_id = 1 THEN calc.xp_total
           ELSE COALESCE(calc.xp_total - xp_section_type.creation_cap, 0::bigint)
           END AS discretion_xp,
       xp_mappings.total_character_creation_xp AS total_character_creation_xp,
       COALESCE(calc.xp_total - xp_mappings.total_character_creation_xp, 0) AS level_xp
FROM "Characters"
	  CROSS JOIN xp_section_type
	  LEFT JOIN calc ON calc.character_id = "Characters"."Id" AND calc.section_type_id = xp_section_type.id
    LEFT JOIN public.character_xp_mapping xp_mappings
    ON xp_mappings.character_id = "Characters"."Id"
    AND xp_mappings.xp_section_type_id = xp_section_type.id;