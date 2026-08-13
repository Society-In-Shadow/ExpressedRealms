CREATE OR REPLACE PROCEDURE copy_character_to_player_proc(
    p_source_character_id     INT,
    p_target_player_id        uuid,
    p_character_name          TEXT,
    INOUT new_character_id  INT
)
    LANGUAGE plpgsql
AS $$
DECLARE v_clone_batch_id uuid := gen_random_uuid();
BEGIN

    -- insert new character record, and grab the id
    -- NOTE: Is Archived is based on is primary character - Copies as of time of writing come only from archetypes which are never primary
    --  or are triggered when a GO finalizes a primary character.  There is no general copy functionality available to anyone
    insert into public.characters (name, player_id, expression_id, stat_experience_points, is_in_character_creation, is_primary_character, player_number, primary_progression_id, secondary_progression_id, wealth_level, void_fragments, motes, prima_fragments, source_character_id, create_date, is_archived)
    select p_character_name, p_target_player_id, expression_id, stat_experience_points, is_in_character_creation, is_primary_character, player_number, primary_progression_id, secondary_progression_id, wealth_level, void_fragments, motes, prima_fragments, p_source_character_id, CURRENT_TIMESTAMP, is_primary_character from public.characters
    where is_deleted = false and id = p_source_character_id
    RETURNING id INTO new_character_id;

    -- copy stats over
    insert into public.character_stat_mappings (character_id, stat_type_id, stat_level_id)
    select new_character_id, stat_type_id, stat_level_id from public.character_stat_mappings
    where character_id = p_source_character_id;

    -- copy blessings over
    insert into public.character_blessing_mappings (character_id, blessing_id, blessing_level_id, notes, is_deleted)
    select new_character_id, blessing_id, blessing_level_id, notes, false from public.character_blessing_mappings
    where is_deleted = false and character_id = p_source_character_id;

    -- copy powers over
    insert into public.character_power_mappings (character_id, power_id, power_level_id, notes, is_deleted)
    select new_character_id, power_id, power_level_id, notes, false from public.character_power_mappings
    where is_deleted = false and character_id = p_source_character_id;

    -- copy skills over
    insert into public.character_skills_mappings (character_id, skill_type_id, skill_level_id)
    select new_character_id, skill_type_id, skill_level_id from public.character_skills_mappings
    where  character_id = p_source_character_id;

    -- copy over xp mappings
    insert into public.character_xp_mappings (character_id, xp_section_type_id, section_cap, spent_xp, discretion_xp, total_character_creation_xp, level_xp)
    select new_character_id, xp_section_type_id, section_cap, spent_xp, discretion_xp, total_character_creation_xp, level_xp from public.character_xp_mappings
    where character_id = p_source_character_id;

    -- copy over contacts
    insert into public.contacts (character_id, knowledge_id, knowledge_level_id, name, notes, frequency, spent_xp, is_approved, is_deleted)
    select new_character_id, knowledge_id, knowledge_level_id, name, notes, frequency, spent_xp, is_approved, false from public.contacts
    where is_deleted = false and character_id = p_source_character_id;
    
    -- Copy Over Factions
    insert into public.character_faction_mappings(character_id, approved_by_user_id, approval_reason, character_notes, request_promotion, request_reason, approval_date, faction_level_id, deleted_at, is_deleted) 
    SELECT new_character_id, approved_by_user_id, approval_reason, character_notes, request_promotion, request_reason, approval_date, faction_level_id, deleted_at, is_deleted from public.character_faction_Mappings
    where is_deleted = false and character_id = p_source_character_id;
    
    -- Copy over the specific modifier mappings
    CREATE TEMP TABLE stat_modifer_group_ids (
     old_id bigint NOT NULL,
     new_id bigint NULL,
     PRIMARY KEY (old_id)
    ) on commit drop;
    
    insert into stat_modifer_group_ids(old_id)
    select stat_modifier_group_id from public.characters
    where is_deleted = false and id = p_source_character_id AND stat_modifier_group_id IS NOT NULL;
    
    call public.copy_modifiers(v_clone_batch_id);
    
    update public.characters c
    set stat_modifier_group_id = g.new_id
    from public.characters oc
        join stat_modifer_group_ids g on g.old_id = oc.stat_modifier_group_id
    where c.id = new_character_id and oc.id = p_source_character_id;

    -- copy knowledges and knowledge specializations
    WITH source_knowledges AS (
        SELECT id, knowledge_id, knowledge_level_id, notes
        FROM public.character_knowledge_mappings
        WHERE is_deleted = false
          AND character_id = p_source_character_id
    ),
         inserted_knowledges AS (
             INSERT INTO public.character_knowledge_mappings
                 (character_id, knowledge_id, knowledge_level_id, notes, is_deleted)
                 SELECT
                     new_character_id,
                     knowledge_id,
                     knowledge_level_id,
                     notes,
                     false
                 FROM source_knowledges
                 RETURNING id AS new_id, knowledge_id
         ),
         mapping_ids AS (
             SELECT
                 src.id      AS old_id,
                 ins.new_id
             FROM inserted_knowledges ins
                      JOIN source_knowledges src ON src.knowledge_id = ins.knowledge_id
         )
    INSERT INTO public.character_knowledge_specializations
    (knowledge_mapping_id, name, description, notes, is_deleted)
    SELECT
        m.new_id,
        name,
        description,
        notes,
        false
    FROM public.character_knowledge_specializations
             JOIN mapping_ids m ON m.old_id = knowledge_mapping_id
    WHERE is_deleted = false;

    commit;
END;
$$;
