CREATE OR REPLACE PROCEDURE copy_character_to_player_proc(
    source_character_id     INT,
    target_player_id        uuid,
    character_name          TEXT,
    INOUT new_character_id  INT
)
LANGUAGE plpgsql
AS $$
BEGIN

	-- insert new character record, and grab the id
	insert into public.characters (name, player_id, expression_id, agility_id, constitution_id, dexterity_id, intelligence_id, strength_id, willpower_id, stat_experience_points, is_in_character_creation, is_primary_character, player_number, primary_progression_id, secondary_progression_id)
	select character_name, target_player_id, expression_id, agility_id, constitution_id, dexterity_id, intelligence_id, strength_id, willpower_id, stat_experience_points, is_in_character_creation, is_primary_character, player_number, primary_progression_id, secondary_progression_id from public.characters
	where is_deleted = false and id = source_character_id
	RETURNING id INTO new_character_id;
	
	-- copy blessings over
	insert into public.character_blessing_mappings (character_id, blessing_id, blessing_level_id, notes, is_deleted)
	select new_character_id, blessing_id, blessing_level_id, notes, false from public.character_blessing_mappings
	where is_deleted = false and character_id = source_character_id;
	
	-- copy powers over
	insert into public.character_power_mappings (character_id, power_id, power_level_id, notes, is_deleted)
	select new_character_id, power_id, power_level_id, notes, false from public.character_power_mappings
	where is_deleted = false and character_id = source_character_id;
	
	-- copy skills over
	insert into public.character_skills_mappings (character_id, skill_type_id, skill_level_id)
	select new_character_id, skill_type_id, skill_level_id from public.character_skills_mappings
	where  character_id = source_character_id;
	
	-- copy over xp mappings
	insert into public.character_xp_mappings (character_id, xp_section_type_id, section_cap, spent_xp, discretion_xp, total_character_creation_xp, level_xp)
	select new_character_id, xp_section_type_id, section_cap, spent_xp, discretion_xp, total_character_creation_xp, level_xp from public.character_xp_mappings
	where character_id = source_character_id;
	
	-- copy over contacts
	insert into public.contacts (character_id, knowledge_id, knowledge_level_id, name, notes, frequency, spent_xp, is_approved, is_deleted)
	select new_character_id, knowledge_id, knowledge_level_id, name, notes, frequency, spent_xp, false, false from public.contacts
	where is_deleted = false and character_id = source_character_id;

    -- copy knowledges and knowledge specializations
	WITH source_knowledges AS (
	    SELECT id, knowledge_id, knowledge_level_id, notes
	    FROM public.character_knowledge_mappings
	    WHERE is_deleted = false
	      AND character_id = source_character_id
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









