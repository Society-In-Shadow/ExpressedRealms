using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class EnforceSnakeCaseForWholeDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_assigned_xp_mapping_AspNetUsers_assigned_by_user_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_assigned_xp_mapping_Characters_character_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_assigned_xp_mapping_Players_player_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_assigned_xp_mapping_assigned_xp_type_assigned_xp_type_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_assigned_xp_mapping_event_event_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_assigned_xp_mapping_audit_trail_AspNetUsers_actor_user_id",
                table: "assigned_xp_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_assigned_xp_mapping_audit_trail_assigned_xp_mapping_assigne~",
                table: "assigned_xp_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_assigned_xp_type_audit_trail_AspNetUsers_actor_user_id",
                table: "assigned_xp_type_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_assigned_xp_type_audit_trail_assigned_xp_type_assigned_xp_t~",
                table: "assigned_xp_type_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_blessing_audit_trail_AspNetUsers_actor_user_id",
                table: "blessing_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_blessing_audit_trail_blessing_blessing_id",
                table: "blessing_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_blessing_level_blessing_blessing_id",
                table: "blessing_level");

            migrationBuilder.DropForeignKey(
                name: "FK_blessing_level_stat_modifier_group_stat_modifier_group",
                table: "blessing_level");

            migrationBuilder.DropForeignKey(
                name: "FK_blessing_level_audit_trail_AspNetUsers_actor_user_id",
                table: "blessing_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_blessing_level_audit_trail_blessing_blessing_id",
                table: "blessing_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_blessing_level_audit_trail_blessing_level_blessing_level_id",
                table: "blessing_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_character_blessing_mapping_Characters_character_id",
                table: "character_blessing_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_blessing_mapping_blessing_blessing_id",
                table: "character_blessing_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_blessing_mapping_blessing_level_blessing_level_id",
                table: "character_blessing_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_knowledge_mapping_Characters_character_id",
                table: "character_knowledge_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_knowledge_mapping_knowledge_education_level_knowl~",
                table: "character_knowledge_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_knowledge_mapping_knowledge_knowledge_id",
                table: "character_knowledge_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_knowledge_specialization_character_knowledge_mapp~",
                table: "character_knowledge_specialization");

            migrationBuilder.DropForeignKey(
                name: "FK_character_power_mapping_Characters_character_id",
                table: "character_power_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_power_mapping_power_level_power_level_id",
                table: "character_power_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_power_mapping_power_power_id",
                table: "character_power_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_xp_mapping_Characters_character_id",
                table: "character_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_character_xp_mapping_xp_section_type_xp_section_type_id",
                table: "character_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ExpressionSections_FactionId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Players_PlayerId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_AgilityId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_ConstitutionId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_DexterityId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_IntelligenceId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_StrengthId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_WillpowerId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_expression_ExpressionId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_progression_path_primary_progression_id",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_progression_path_secondary_progression_id",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterSkillsMapping_Characters_CharacterId",
                table: "CharacterSkillsMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterSkillsMapping_SkillType_SkillTypeId",
                table: "CharacterSkillsMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterSkillsMapping_skill_level_SkillLevelId",
                table: "CharacterSkillsMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_Players_player_id",
                table: "checkin");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_event_event_id",
                table: "checkin");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_audit_trail_AspNetUsers_actor_user_id",
                table: "checkin_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_audit_trail_checkin_checkin_id",
                table: "checkin_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_AspNetUsers_act~",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_checkin_checkin~",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_checkin_questio~",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_event_question_~",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_question_response_checkin_checkin_id",
                table: "checkin_question_response");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_question_response_event_question_event_question_id",
                table: "checkin_question_response");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_stage_mapping_AspNetUsers_approver_user_id",
                table: "checkin_stage_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_stage_mapping_checkin_checkin_id",
                table: "checkin_stage_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_checkin_stage_mapping_checkin_stage_checkin_stage_id",
                table: "checkin_stage_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_contact_Characters_character_id",
                table: "contact");

            migrationBuilder.DropForeignKey(
                name: "FK_contact_knowledge_education_level_knowledge_level_id",
                table: "contact");

            migrationBuilder.DropForeignKey(
                name: "FK_contact_knowledge_knowledge_id",
                table: "contact");

            migrationBuilder.DropForeignKey(
                name: "FK_contact_audit_trail_AspNetUsers_actor_user_id",
                table: "contact_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_contact_audit_trail_contact_contact_id",
                table: "contact_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_event_audit_trail_AspNetUsers_actor_user_id",
                table: "event_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_event_audit_trail_event_event_id",
                table: "event_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_event_question_event_event_id",
                table: "event_question");

            migrationBuilder.DropForeignKey(
                name: "FK_event_question_question_type_question_type_id",
                table: "event_question");

            migrationBuilder.DropForeignKey(
                name: "FK_event_question_audit_trail_AspNetUsers_actor_user_id",
                table: "event_question_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_event_question_audit_trail_event_question_event_question_id",
                table: "event_question_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_event_schedule_item_audit_trail_AspNetUsers_actor_user_id",
                table: "event_schedule_item_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_event_schedule_item_audit_trail_event_EventId",
                table: "event_schedule_item_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_event_schedule_item_audit_trail_event_schedule_item_event_s~",
                table: "event_schedule_item_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_expression_ExpressionPublishStatus_publish_status_id",
                table: "expression");

            migrationBuilder.DropForeignKey(
                name: "FK_expression_expression_type_expression_type_id",
                table: "expression");

            migrationBuilder.DropForeignKey(
                name: "FK_Expression_AuditTrail_AspNetUsers_ActorUserId",
                table: "Expression_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_Expression_AuditTrail_expression_ExpressionId",
                table: "Expression_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_ActorUserId",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_AuditTrail_ExpressionSections_SectionId",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_AuditTrail_expression_ExpressionId",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSections_ExpressionSectionTypes_SectionTypeId",
                table: "ExpressionSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSections_ExpressionSections_ParentId",
                table: "ExpressionSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSections_expression_ExpressionId",
                table: "ExpressionSections");

            migrationBuilder.DropForeignKey(
                name: "FK_knowledge_knowledge_type_knowledge_type_id",
                table: "knowledge");

            migrationBuilder.DropForeignKey(
                name: "FK_knowledges_audit_trail_AspNetUsers_actor_user_id",
                table: "knowledges_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_knowledges_audit_trail_knowledge_knowledge_id",
                table: "knowledges_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_permission_permission_resource_permission_resource_id",
                table: "permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_AuditTrail_AspNetUsers_ActorUserId",
                table: "Player_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_AuditTrail_Players_PlayerId",
                table: "Player_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_AspNetUsers_UserId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_power_power_activation_timing_type_ActivationTimingTypeId",
                table: "power");

            migrationBuilder.DropForeignKey(
                name: "FK_power_power_area_of_effect_type_AreaOfEffectTypeId",
                table: "power");

            migrationBuilder.DropForeignKey(
                name: "FK_power_power_duration_DurationId",
                table: "power");

            migrationBuilder.DropForeignKey(
                name: "FK_power_power_level_LevelId",
                table: "power");

            migrationBuilder.DropForeignKey(
                name: "FK_power_power_path_power_path_id",
                table: "power");

            migrationBuilder.DropForeignKey(
                name: "FK_power_stat_modifier_group_stat_modifier_group",
                table: "power");

            migrationBuilder.DropForeignKey(
                name: "FK_power_audit_trail_AspNetUsers_actor_user_id",
                table: "power_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_power_audit_trail_power_path_power_path_id",
                table: "power_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_power_audit_trail_power_power_id",
                table: "power_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_power_category_mapping_power_PowerId",
                table: "power_category_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_power_category_mapping_power_category_CategoryId",
                table: "power_category_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_expression_expression_id",
                table: "power_path");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_audit_trail_AspNetUsers_actor_user_id",
                table: "power_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_audit_trail_expression_expression_id",
                table: "power_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_audit_trail_power_path_power_path_id",
                table: "power_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_power_prerequisite_power_power_id",
                table: "power_prerequisite");

            migrationBuilder.DropForeignKey(
                name: "FK_power_prerequisite_power_power_power_id",
                table: "power_prerequisite_power");

            migrationBuilder.DropForeignKey(
                name: "FK_power_prerequisite_power_power_prerequisite_prerequisite_id",
                table: "power_prerequisite_power");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_level_progression_path_progression_path_id",
                table: "progression_level");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_level_stat_modifier_group_stat_modifier_group",
                table: "progression_level");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_level_audit_trail_AspNetUsers_actor_user_id",
                table: "progression_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_level_audit_trail_progression_level_progression~",
                table: "progression_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_level_audit_trail_progression_path_progression_~",
                table: "progression_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_path_expression_expression_id",
                table: "progression_path");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_path_audit_trail_AspNetUsers_actor_user_id",
                table: "progression_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_path_audit_trail_expression_expression_id",
                table: "progression_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_path_audit_trail_progression_path_progression_p~",
                table: "progression_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_audit_trail_AspNetUsers_actor_user_id",
                table: "role_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_audit_trail_role_role_id",
                table: "role_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_permission_permission_id",
                table: "role_permission_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_role_role_id",
                table: "role_permission_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_AspNetUsers_actor_user_~",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelBenefit_SkillType_SkillTypeId",
                table: "SkillLevelBenefit");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelBenefit_modifier_type_ModifierTypeId",
                table: "SkillLevelBenefit");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelBenefit_skill_level_SkillLevelId",
                table: "SkillLevelBenefit");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelDescriptionMapping_SkillType_SkillTypeId",
                table: "SkillLevelDescriptionMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelDescriptionMapping_skill_level_SkillLevelId",
                table: "SkillLevelDescriptionMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillType_SkillSubType_SkillSubTypeId",
                table: "SkillType");

            migrationBuilder.DropForeignKey(
                name: "FK_stat_group_mapping_expression_target_expression_id",
                table: "stat_group_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_stat_group_mapping_stat_modifier_group_stat_group_id",
                table: "stat_group_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_stat_group_mapping_stat_modifier_stat_modifier_id",
                table: "stat_group_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_StatDescriptionMappings_StatLevels_StatLevelId",
                table: "StatDescriptionMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_StatDescriptionMappings_StateTypes_StatTypeId",
                table: "StatDescriptionMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_ActorUserId",
                table: "User_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_UserId",
                table: "User_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_mapping_AspNetUsers_user_id",
                table: "user_role_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_mapping_role_role_id",
                table: "user_role_mapping");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_mapping_audit_trail_AspNetUsers_actor_user_id",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_mapping_audit_trail_AspNetUsers_user_id",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_mapping_audit_trail_role_role_id",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_mapping_audit_trail_user_role_mapping_user_role_m~",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetRoles_RoleId",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetUsers_ActorUserId",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetUsers_MappingUserId",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_xp_section_type",
                table: "xp_section_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles_AuditTrail",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_role_mapping_audit_trail",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_role_mapping",
                table: "user_role_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_AuditTrail",
                table: "User_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permission_mapping_audit_trail",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permission_mapping",
                table: "role_permission_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_audit_trail",
                table: "role_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_progression_path_audit_trail",
                table: "progression_path_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_progression_path",
                table: "progression_path");

            migrationBuilder.DropPrimaryKey(
                name: "PK_progression_level_audit_trail",
                table: "progression_level_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_progression_level",
                table: "progression_level");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player_AuditTrail",
                table: "Player_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission_resource",
                table: "permission_resource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission",
                table: "permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_event_schedule_item_audit_trail",
                table: "event_schedule_item_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_event_question_audit_trail",
                table: "event_question_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_event_audit_trail",
                table: "event_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contact_audit_trail",
                table: "contact_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Characters",
                table: "Characters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_character_xp_mapping",
                table: "character_xp_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_assigned_xp_type_audit_trail",
                table: "assigned_xp_type_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_assigned_xp_type",
                table: "assigned_xp_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_assigned_xp_mapping_audit_trail",
                table: "assigned_xp_mapping_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_assigned_xp_mapping",
                table: "assigned_xp_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatLevels",
                table: "StatLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StateTypes",
                table: "StateTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatDescriptionMappings",
                table: "StatDescriptionMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stat_modifier_group",
                table: "stat_modifier_group");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stat_modifier",
                table: "stat_modifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stat_group_mapping",
                table: "stat_group_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkillType",
                table: "SkillType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkillSubType",
                table: "SkillSubType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkillLevelDescriptionMapping",
                table: "SkillLevelDescriptionMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkillLevelBenefit",
                table: "SkillLevelBenefit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_skill_level",
                table: "skill_level");

            migrationBuilder.DropPrimaryKey(
                name: "PK_question_type",
                table: "question_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_prerequisite_power",
                table: "power_prerequisite_power");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_prerequisite",
                table: "power_prerequisite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_path_audit_trail",
                table: "power_path_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_path",
                table: "power_path");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_level",
                table: "power_level");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_duration",
                table: "power_duration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_category_mapping",
                table: "power_category_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_category",
                table: "power_category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_audit_trail",
                table: "power_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_area_of_effect_type",
                table: "power_area_of_effect_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_activation_timing_type",
                table: "power_activation_timing_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power",
                table: "power");

            migrationBuilder.DropPrimaryKey(
                name: "PK_modifier_type",
                table: "modifier_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_knowledges_audit_trail",
                table: "knowledges_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_knowledge_type",
                table: "knowledge_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_knowledge_education_level",
                table: "knowledge_education_level");

            migrationBuilder.DropPrimaryKey(
                name: "PK_knowledge",
                table: "knowledge");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpressionSectionTypes",
                table: "ExpressionSectionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpressionSections",
                table: "ExpressionSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpressionSection_AuditTrail",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpressionPublishStatus",
                table: "ExpressionPublishStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expression_type",
                table: "expression_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expression_AuditTrail",
                table: "Expression_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expression",
                table: "expression");

            migrationBuilder.DropPrimaryKey(
                name: "PK_event_schedule_item",
                table: "event_schedule_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_event_question",
                table: "event_question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_event",
                table: "event");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contact",
                table: "contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_checkin_stage_mapping",
                table: "checkin_stage_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_checkin_stage",
                table: "checkin_stage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_checkin_question_response",
                table: "checkin_question_response");

            migrationBuilder.DropPrimaryKey(
                name: "PK_checkin_event_question_response_audit_trail",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_checkin_audit_trail",
                table: "checkin_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_checkin",
                table: "checkin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterSkillsMapping",
                table: "CharacterSkillsMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_character_power_mapping",
                table: "character_power_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_character_knowledge_specialization",
                table: "character_knowledge_specialization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_character_knowledge_mapping",
                table: "character_knowledge_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_character_blessing_mapping",
                table: "character_blessing_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_blessing_level_audit_trail",
                table: "blessing_level_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_blessing_level",
                table: "blessing_level");

            migrationBuilder.DropPrimaryKey(
                name: "PK_blessing_audit_trail",
                table: "blessing_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_blessing",
                table: "blessing");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "players");

            migrationBuilder.RenameTable(
                name: "Characters",
                newName: "characters");

            migrationBuilder.RenameTable(
                name: "StatLevels",
                newName: "stat_levels");

            migrationBuilder.RenameTable(
                name: "StateTypes",
                newName: "state_types");

            migrationBuilder.RenameTable(
                name: "StatDescriptionMappings",
                newName: "stat_description_mappings");

            migrationBuilder.RenameTable(
                name: "stat_modifier_group",
                newName: "stat_modifier_groups");

            migrationBuilder.RenameTable(
                name: "stat_modifier",
                newName: "stat_modifiers");

            migrationBuilder.RenameTable(
                name: "stat_group_mapping",
                newName: "stat_group_mappings");

            migrationBuilder.RenameTable(
                name: "SkillType",
                newName: "skill_types");

            migrationBuilder.RenameTable(
                name: "SkillSubType",
                newName: "skill_sub_types");

            migrationBuilder.RenameTable(
                name: "SkillLevelDescriptionMapping",
                newName: "skill_level_description_mappings");

            migrationBuilder.RenameTable(
                name: "SkillLevelBenefit",
                newName: "skill_level_benefits");

            migrationBuilder.RenameTable(
                name: "skill_level",
                newName: "skill_levels");

            migrationBuilder.RenameTable(
                name: "question_type",
                newName: "question_types");

            migrationBuilder.RenameTable(
                name: "power_prerequisite_power",
                newName: "power_prerequisite_powers");

            migrationBuilder.RenameTable(
                name: "power_prerequisite",
                newName: "power_prerequisites");

            migrationBuilder.RenameTable(
                name: "power_path_audit_trail",
                newName: "power_path_audit_trails");

            migrationBuilder.RenameTable(
                name: "power_path",
                newName: "power_paths");

            migrationBuilder.RenameTable(
                name: "power_level",
                newName: "power_levels");

            migrationBuilder.RenameTable(
                name: "power_duration",
                newName: "power_durations");

            migrationBuilder.RenameTable(
                name: "power_category_mapping",
                newName: "power_category_mappings");

            migrationBuilder.RenameTable(
                name: "power_category",
                newName: "power_categories");

            migrationBuilder.RenameTable(
                name: "power_audit_trail",
                newName: "power_audit_trails");

            migrationBuilder.RenameTable(
                name: "power_area_of_effect_type",
                newName: "power_area_of_effect_types");

            migrationBuilder.RenameTable(
                name: "power_activation_timing_type",
                newName: "power_activation_timing_types");

            migrationBuilder.RenameTable(
                name: "power",
                newName: "powers");

            migrationBuilder.RenameTable(
                name: "modifier_type",
                newName: "modifier_types");

            migrationBuilder.RenameTable(
                name: "knowledges_audit_trail",
                newName: "knowledge_audit_trails");

            migrationBuilder.RenameTable(
                name: "knowledge_type",
                newName: "knowledge_types");

            migrationBuilder.RenameTable(
                name: "knowledge_education_level",
                newName: "knowledge_education_levels");

            migrationBuilder.RenameTable(
                name: "knowledge",
                newName: "knowledges");

            migrationBuilder.RenameTable(
                name: "ExpressionSectionTypes",
                newName: "expression_section_types");

            migrationBuilder.RenameTable(
                name: "ExpressionSections",
                newName: "expression_sections");

            migrationBuilder.RenameTable(
                name: "ExpressionSection_AuditTrail",
                newName: "expression_section_audit_trails");

            migrationBuilder.RenameTable(
                name: "ExpressionPublishStatus",
                newName: "expression_publish_status");

            migrationBuilder.RenameTable(
                name: "expression_type",
                newName: "expression_types");

            migrationBuilder.RenameTable(
                name: "Expression_AuditTrail",
                newName: "expression_audit_trails");

            migrationBuilder.RenameTable(
                name: "expression",
                newName: "expressions");

            migrationBuilder.RenameTable(
                name: "event_schedule_item",
                newName: "event_schedule_items");

            migrationBuilder.RenameTable(
                name: "event_question",
                newName: "event_questions");

            migrationBuilder.RenameTable(
                name: "event",
                newName: "events");

            migrationBuilder.RenameTable(
                name: "contact",
                newName: "contacts");

            migrationBuilder.RenameTable(
                name: "checkin_stage_mapping",
                newName: "checkin_stage_mappings");

            migrationBuilder.RenameTable(
                name: "checkin_stage",
                newName: "checkin_stages");

            migrationBuilder.RenameTable(
                name: "checkin_question_response",
                newName: "checkin_question_responses");

            migrationBuilder.RenameTable(
                name: "checkin_event_question_response_audit_trail",
                newName: "checkin_question_response_audit_trail");

            migrationBuilder.RenameTable(
                name: "checkin_audit_trail",
                newName: "checkin_audit_trails");

            migrationBuilder.RenameTable(
                name: "checkin",
                newName: "checkins");

            migrationBuilder.RenameTable(
                name: "CharacterSkillsMapping",
                newName: "character_skills_mappings");

            migrationBuilder.RenameTable(
                name: "character_power_mapping",
                newName: "character_power_mappings");

            migrationBuilder.RenameTable(
                name: "character_knowledge_specialization",
                newName: "character_knowledge_specializations");

            migrationBuilder.RenameTable(
                name: "character_knowledge_mapping",
                newName: "character_knowledge_mappings");

            migrationBuilder.RenameTable(
                name: "character_blessing_mapping",
                newName: "character_blessing_mappings");

            migrationBuilder.RenameTable(
                name: "blessing_level_audit_trail",
                newName: "blessing_level_audit_trails");

            migrationBuilder.RenameTable(
                name: "blessing_level",
                newName: "blessing_levels");

            migrationBuilder.RenameTable(
                name: "blessing_audit_trail",
                newName: "blessing_audit_trails");

            migrationBuilder.RenameTable(
                name: "blessing",
                newName: "blessings");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "UserRoles_AuditTrail",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "UserRoles_AuditTrail",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserRoles_AuditTrail",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "UserRoles_AuditTrail",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "MappingUserId",
                table: "UserRoles_AuditTrail",
                newName: "mapping_user_id");

            migrationBuilder.RenameColumn(
                name: "ChangedProperties",
                table: "UserRoles_AuditTrail",
                newName: "changed_properties");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "UserRoles_AuditTrail",
                newName: "actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_AuditTrail_RoleId",
                table: "UserRoles_AuditTrail",
                newName: "ix_user_roles_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_AuditTrail_MappingUserId",
                table: "UserRoles_AuditTrail",
                newName: "ix_user_roles_audit_trail_mapping_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_AuditTrail_ActorUserId",
                table: "UserRoles_AuditTrail",
                newName: "ix_user_roles_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_mapping_audit_trail_user_role_mapping_id",
                table: "user_role_mapping_audit_trail",
                newName: "ix_user_role_mapping_audit_trail_user_role_mapping_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_mapping_audit_trail_user_id",
                table: "user_role_mapping_audit_trail",
                newName: "ix_user_role_mapping_audit_trail_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_mapping_audit_trail_role_id",
                table: "user_role_mapping_audit_trail",
                newName: "ix_user_role_mapping_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_mapping_audit_trail_actor_user_id",
                table: "user_role_mapping_audit_trail",
                newName: "ix_user_role_mapping_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_mapping_user_id",
                table: "user_role_mapping",
                newName: "ix_user_role_mapping_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_mapping_role_id",
                table: "user_role_mapping",
                newName: "ix_user_role_mapping_role_id");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "User_AuditTrail",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "User_AuditTrail",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User_AuditTrail",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "User_AuditTrail",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ChangedProperties",
                table: "User_AuditTrail",
                newName: "changed_properties");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "User_AuditTrail",
                newName: "actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_User_AuditTrail_UserId",
                table: "User_AuditTrail",
                newName: "ix_user_audit_trail_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_User_AuditTrail_ActorUserId",
                table: "User_AuditTrail",
                newName: "ix_user_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_mapping_audit_trail_role_id",
                table: "role_permission_mapping_audit_trail",
                newName: "ix_role_permission_mapping_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_mapping_audit_trail_permission_id",
                table: "role_permission_mapping_audit_trail",
                newName: "ix_role_permission_mapping_audit_trail_permission_id");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_mapping_audit_trail_actor_user_id",
                table: "role_permission_mapping_audit_trail",
                newName: "ix_role_permission_mapping_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail",
                newName: "ix_role_permission_mapping_audit_trail_role_permission_mapping");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_mapping_role_id",
                table: "role_permission_mapping",
                newName: "ix_role_permission_mapping_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_mapping_permission_id",
                table: "role_permission_mapping",
                newName: "ix_role_permission_mapping_permission_id");

            migrationBuilder.RenameIndex(
                name: "IX_role_audit_trail_role_id",
                table: "role_audit_trail",
                newName: "ix_role_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_role_audit_trail_actor_user_id",
                table: "role_audit_trail",
                newName: "ix_role_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_progression_path_audit_trail_progression_path_id",
                table: "progression_path_audit_trail",
                newName: "ix_progression_path_audit_trail_progression_path_id");

            migrationBuilder.RenameIndex(
                name: "IX_progression_path_audit_trail_expression_id",
                table: "progression_path_audit_trail",
                newName: "ix_progression_path_audit_trail_expression_id");

            migrationBuilder.RenameIndex(
                name: "IX_progression_path_audit_trail_actor_user_id",
                table: "progression_path_audit_trail",
                newName: "ix_progression_path_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_progression_path_expression_id",
                table: "progression_path",
                newName: "ix_progression_path_expression_id");

            migrationBuilder.RenameIndex(
                name: "IX_progression_level_audit_trail_progression_path_id",
                table: "progression_level_audit_trail",
                newName: "ix_progression_level_audit_trail_progression_path_id");

            migrationBuilder.RenameIndex(
                name: "IX_progression_level_audit_trail_progression_level_id",
                table: "progression_level_audit_trail",
                newName: "ix_progression_level_audit_trail_progression_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_progression_level_audit_trail_actor_user_id",
                table: "progression_level_audit_trail",
                newName: "ix_progression_level_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "stat_modifier_group",
                table: "progression_level",
                newName: "stat_modifier_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_progression_level_progression_path_id",
                table: "progression_level",
                newName: "ix_progression_level_progression_path_id");

            migrationBuilder.RenameIndex(
                name: "IX_progression_level_stat_modifier_group",
                table: "progression_level",
                newName: "ix_progression_level_stat_modifier_group_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "players",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "players",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "players",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Players_player_number",
                table: "players",
                newName: "ix_players_player_number");

            migrationBuilder.RenameIndex(
                name: "IX_Players_lookup_id",
                table: "players",
                newName: "ix_players_lookup_id");

            migrationBuilder.RenameIndex(
                name: "IX_Players_UserId",
                table: "players",
                newName: "ix_players_user_id");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Player_AuditTrail",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "Player_AuditTrail",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Player_AuditTrail",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Player_AuditTrail",
                newName: "player_id");

            migrationBuilder.RenameColumn(
                name: "ChangedProperties",
                table: "Player_AuditTrail",
                newName: "changed_properties");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "Player_AuditTrail",
                newName: "actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Player_AuditTrail_PlayerId",
                table: "Player_AuditTrail",
                newName: "ix_player_audit_trail_player_id");

            migrationBuilder.RenameIndex(
                name: "IX_Player_AuditTrail_ActorUserId",
                table: "Player_AuditTrail",
                newName: "ix_player_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_permission_permission_resource_id",
                table: "permission",
                newName: "ix_permission_permission_resource_id");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "event_schedule_item_audit_trail",
                newName: "event_id");

            migrationBuilder.RenameIndex(
                name: "IX_event_schedule_item_audit_trail_event_schedule_item_id",
                table: "event_schedule_item_audit_trail",
                newName: "ix_event_schedule_item_audit_trail_event_schedule_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_event_schedule_item_audit_trail_actor_user_id",
                table: "event_schedule_item_audit_trail",
                newName: "ix_event_schedule_item_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_event_schedule_item_audit_trail_EventId",
                table: "event_schedule_item_audit_trail",
                newName: "ix_event_schedule_item_audit_trail_event_id");

            migrationBuilder.RenameIndex(
                name: "IX_event_question_audit_trail_event_question_id",
                table: "event_question_audit_trail",
                newName: "ix_event_question_audit_trail_event_question_id");

            migrationBuilder.RenameIndex(
                name: "IX_event_question_audit_trail_actor_user_id",
                table: "event_question_audit_trail",
                newName: "ix_event_question_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_event_audit_trail_event_id",
                table: "event_audit_trail",
                newName: "ix_event_audit_trail_event_id");

            migrationBuilder.RenameIndex(
                name: "IX_event_audit_trail_actor_user_id",
                table: "event_audit_trail",
                newName: "ix_event_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_contact_audit_trail_contact_id",
                table: "contact_audit_trail",
                newName: "ix_contact_audit_trail_contact_id");

            migrationBuilder.RenameIndex(
                name: "IX_contact_audit_trail_actor_user_id",
                table: "contact_audit_trail",
                newName: "ix_contact_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "characters",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Background",
                table: "characters",
                newName: "background");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "characters",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WillpowerId",
                table: "characters",
                newName: "willpower_id");

            migrationBuilder.RenameColumn(
                name: "StrengthId",
                table: "characters",
                newName: "strength_id");

            migrationBuilder.RenameColumn(
                name: "StatExperiencePoints",
                table: "characters",
                newName: "stat_experience_points");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "characters",
                newName: "player_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "characters",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IntelligenceId",
                table: "characters",
                newName: "intelligence_id");

            migrationBuilder.RenameColumn(
                name: "FactionId",
                table: "characters",
                newName: "faction_id");

            migrationBuilder.RenameColumn(
                name: "ExpressionId",
                table: "characters",
                newName: "expression_id");

            migrationBuilder.RenameColumn(
                name: "DexterityId",
                table: "characters",
                newName: "dexterity_id");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "characters",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "ConstitutionId",
                table: "characters",
                newName: "constitution_id");

            migrationBuilder.RenameColumn(
                name: "AgilityId",
                table: "characters",
                newName: "agility_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_secondary_progression_id",
                table: "characters",
                newName: "ix_characters_secondary_progression_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_primary_progression_id",
                table: "characters",
                newName: "ix_characters_primary_progression_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_WillpowerId",
                table: "characters",
                newName: "ix_characters_willpower_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_StrengthId",
                table: "characters",
                newName: "ix_characters_strength_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_PlayerId",
                table: "characters",
                newName: "ix_characters_player_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_IntelligenceId",
                table: "characters",
                newName: "ix_characters_intelligence_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_FactionId",
                table: "characters",
                newName: "ix_characters_faction_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_ExpressionId",
                table: "characters",
                newName: "ix_characters_expression_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_DexterityId",
                table: "characters",
                newName: "ix_characters_dexterity_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_ConstitutionId",
                table: "characters",
                newName: "ix_characters_constitution_id");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_AgilityId",
                table: "characters",
                newName: "ix_characters_agility_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_xp_mapping_xp_section_type_id",
                table: "character_xp_mapping",
                newName: "ix_character_xp_mapping_xp_section_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_assigned_xp_type_audit_trail_assigned_xp_type_id",
                table: "assigned_xp_type_audit_trail",
                newName: "ix_assigned_xp_type_audit_trail_assigned_xp_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_assigned_xp_type_audit_trail_actor_user_id",
                table: "assigned_xp_type_audit_trail",
                newName: "ix_assigned_xp_type_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "assigned_xp_type",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "assigned_xp_type",
                newName: "description");

            migrationBuilder.RenameIndex(
                name: "IX_assigned_xp_mapping_audit_trail_assigned_xp_mapping_id",
                table: "assigned_xp_mapping_audit_trail",
                newName: "ix_assigned_xp_mapping_audit_trail_assigned_xp_mapping_id");

            migrationBuilder.RenameIndex(
                name: "IX_assigned_xp_mapping_audit_trail_actor_user_id",
                table: "assigned_xp_mapping_audit_trail",
                newName: "ix_assigned_xp_mapping_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_assigned_xp_mapping_player_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_player_id");

            migrationBuilder.RenameIndex(
                name: "IX_assigned_xp_mapping_event_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_event_id");

            migrationBuilder.RenameIndex(
                name: "IX_assigned_xp_mapping_character_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_character_id");

            migrationBuilder.RenameIndex(
                name: "IX_assigned_xp_mapping_assigned_xp_type_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_assigned_xp_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_assigned_xp_mapping_assigned_by_user_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_assigned_by_user_id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "AspNetUserTokens",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUserTokens",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                newName: "login_provider");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUserTokens",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AspNetUsers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUsers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AspNetUsers",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                newName: "two_factor_enabled");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                table: "AspNetUsers",
                newName: "security_stamp");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                newName: "phone_number_confirmed");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "AspNetUsers",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "AspNetUsers",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                newName: "normalized_user_name");

            migrationBuilder.RenameColumn(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                newName: "normalized_email");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                table: "AspNetUsers",
                newName: "lockout_end");

            migrationBuilder.RenameColumn(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                newName: "lockout_enabled");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                newName: "email_confirmed");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                table: "AspNetUsers",
                newName: "access_failed_count");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "AspNetUserRoles",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUserRoles",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                newName: "ix_asp_net_user_roles_role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUserLogins",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ProviderDisplayName",
                table: "AspNetUserLogins",
                newName: "provider_display_name");

            migrationBuilder.RenameColumn(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                newName: "provider_key");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                newName: "login_provider");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                newName: "ix_asp_net_user_logins_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUserClaims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUserClaims",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "AspNetUserClaims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "AspNetUserClaims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                newName: "ix_asp_net_user_claims_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetRoles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetRoles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                table: "AspNetRoles",
                newName: "normalized_name");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "AspNetRoles",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetRoleClaims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "AspNetRoleClaims",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "AspNetRoleClaims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "AspNetRoleClaims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                newName: "ix_asp_net_role_claims_role_id");

            migrationBuilder.RenameColumn(
                name: "Bonus",
                table: "stat_levels",
                newName: "bonus");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "stat_levels",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "XPCost",
                table: "stat_levels",
                newName: "xp_cost");

            migrationBuilder.RenameColumn(
                name: "TotalXPCost",
                table: "stat_levels",
                newName: "total_xp_cost");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "state_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "state_types",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "state_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "state_types",
                newName: "short_name");

            migrationBuilder.RenameColumn(
                name: "ReasonableExpectation",
                table: "stat_description_mappings",
                newName: "reasonable_expectation");

            migrationBuilder.RenameColumn(
                name: "StatLevelId",
                table: "stat_description_mappings",
                newName: "stat_level_id");

            migrationBuilder.RenameColumn(
                name: "StatTypeId",
                table: "stat_description_mappings",
                newName: "stat_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_StatDescriptionMappings_StatLevelId",
                table: "stat_description_mappings",
                newName: "ix_stat_description_mappings_stat_level_id");

            migrationBuilder.RenameColumn(
                name: "scales_with_level",
                table: "stat_group_mappings",
                newName: "scale_with_level");

            migrationBuilder.RenameIndex(
                name: "IX_stat_group_mapping_target_expression_id",
                table: "stat_group_mappings",
                newName: "ix_stat_group_mappings_target_expression_id");

            migrationBuilder.RenameIndex(
                name: "IX_stat_group_mapping_stat_modifier_id",
                table: "stat_group_mappings",
                newName: "ix_stat_group_mappings_stat_modifier_id");

            migrationBuilder.RenameIndex(
                name: "IX_stat_group_mapping_stat_group_id",
                table: "stat_group_mappings",
                newName: "ix_stat_group_mappings_stat_group_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "skill_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "skill_types",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "skill_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SkillSubTypeId",
                table: "skill_types",
                newName: "skill_sub_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_SkillType_SkillSubTypeId",
                table: "skill_types",
                newName: "ix_skill_types_skill_sub_type_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "skill_sub_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "skill_sub_types",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "skill_sub_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "skill_level_description_mappings",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "SkillTypeId",
                table: "skill_level_description_mappings",
                newName: "skill_type_id");

            migrationBuilder.RenameColumn(
                name: "SkillLevelId",
                table: "skill_level_description_mappings",
                newName: "skill_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_SkillLevelDescriptionMapping_SkillTypeId",
                table: "skill_level_description_mappings",
                newName: "ix_skill_level_description_mappings_skill_type_id");

            migrationBuilder.RenameColumn(
                name: "Modifier",
                table: "skill_level_benefits",
                newName: "modifier");

            migrationBuilder.RenameColumn(
                name: "ModifierTypeId",
                table: "skill_level_benefits",
                newName: "modifier_type_id");

            migrationBuilder.RenameColumn(
                name: "SkillTypeId",
                table: "skill_level_benefits",
                newName: "skill_type_id");

            migrationBuilder.RenameColumn(
                name: "SkillLevelId",
                table: "skill_level_benefits",
                newName: "skill_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_SkillLevelBenefit_SkillTypeId",
                table: "skill_level_benefits",
                newName: "ix_skill_level_benefits_skill_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_SkillLevelBenefit_ModifierTypeId",
                table: "skill_level_benefits",
                newName: "ix_skill_level_benefits_modifier_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_prerequisite_power_power_id",
                table: "power_prerequisite_powers",
                newName: "ix_power_prerequisite_powers_power_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_prerequisite_power_id",
                table: "power_prerequisites",
                newName: "ix_power_prerequisites_power_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_path_audit_trail_power_path_id",
                table: "power_path_audit_trails",
                newName: "ix_power_path_audit_trails_power_path_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_path_audit_trail_expression_id",
                table: "power_path_audit_trails",
                newName: "ix_power_path_audit_trails_expression_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_path_audit_trail_actor_user_id",
                table: "power_path_audit_trails",
                newName: "ix_power_path_audit_trails_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_path_expression_id",
                table: "power_paths",
                newName: "ix_power_paths_expression_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "power_durations",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "power_durations",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "power_durations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "power_category_mappings",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "PowerId",
                table: "power_category_mappings",
                newName: "power_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_category_mapping_CategoryId",
                table: "power_category_mappings",
                newName: "ix_power_category_mappings_category_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "power_categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "power_categories",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "power_categories",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_power_audit_trail_power_path_id",
                table: "power_audit_trails",
                newName: "ix_power_audit_trails_power_path_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_audit_trail_power_id",
                table: "power_audit_trails",
                newName: "ix_power_audit_trails_power_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_audit_trail_actor_user_id",
                table: "power_audit_trails",
                newName: "ix_power_audit_trails_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "power_area_of_effect_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "power_area_of_effect_types",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "power_area_of_effect_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "power_activation_timing_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "power_activation_timing_types",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "power_activation_timing_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "powers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Limitation",
                table: "powers",
                newName: "limitation");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "powers",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "powers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "stat_modifier_group",
                table: "powers",
                newName: "stat_modifier_group_id");

            migrationBuilder.RenameColumn(
                name: "OtherFields",
                table: "powers",
                newName: "other_fields");

            migrationBuilder.RenameColumn(
                name: "LevelId",
                table: "powers",
                newName: "level_id");

            migrationBuilder.RenameColumn(
                name: "IsPowerUse",
                table: "powers",
                newName: "is_power_use");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "powers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GameMechanicEffect",
                table: "powers",
                newName: "game_mechanic_effect");

            migrationBuilder.RenameColumn(
                name: "DurationId",
                table: "powers",
                newName: "duration_id");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "powers",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "AreaOfEffectTypeId",
                table: "powers",
                newName: "area_of_effect_type_id");

            migrationBuilder.RenameColumn(
                name: "ActivationTimingTypeId",
                table: "powers",
                newName: "activation_timing_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_stat_modifier_group",
                table: "powers",
                newName: "ix_powers_stat_modifier_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_power_path_id",
                table: "powers",
                newName: "ix_powers_power_path_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_LevelId",
                table: "powers",
                newName: "ix_powers_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_DurationId",
                table: "powers",
                newName: "ix_powers_duration_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_AreaOfEffectTypeId",
                table: "powers",
                newName: "ix_powers_area_of_effect_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_ActivationTimingTypeId",
                table: "powers",
                newName: "ix_powers_activation_timing_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_knowledges_audit_trail_knowledge_id",
                table: "knowledge_audit_trails",
                newName: "ix_knowledge_audit_trails_knowledge_id");

            migrationBuilder.RenameIndex(
                name: "IX_knowledges_audit_trail_actor_user_id",
                table: "knowledge_audit_trails",
                newName: "ix_knowledge_audit_trails_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_knowledge_knowledge_type_id",
                table: "knowledges",
                newName: "ix_knowledges_knowledge_type_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "expression_section_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "expression_section_types",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expression_section_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "expression_sections",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "expression_sections",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expression_sections",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SectionTypeId",
                table: "expression_sections",
                newName: "section_type_id");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "expression_sections",
                newName: "parent_id");

            migrationBuilder.RenameColumn(
                name: "OrderIndex",
                table: "expression_sections",
                newName: "order_index");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "expression_sections",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "ExpressionId",
                table: "expression_sections",
                newName: "expression_id");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "expression_sections",
                newName: "deleted_at");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSections_SectionTypeId",
                table: "expression_sections",
                newName: "ix_expression_sections_section_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSections_ParentId",
                table: "expression_sections",
                newName: "ix_expression_sections_parent_id");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSections_ExpressionId",
                table: "expression_sections",
                newName: "ix_expression_sections_expression_id");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "expression_section_audit_trails",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "expression_section_audit_trails",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expression_section_audit_trails",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "expression_section_audit_trails",
                newName: "section_id");

            migrationBuilder.RenameColumn(
                name: "ExpressionId",
                table: "expression_section_audit_trails",
                newName: "expression_id");

            migrationBuilder.RenameColumn(
                name: "ChangedProperties",
                table: "expression_section_audit_trails",
                newName: "changed_properties");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "expression_section_audit_trails",
                newName: "actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSection_AuditTrail_SectionId",
                table: "expression_section_audit_trails",
                newName: "ix_expression_section_audit_trails_section_id");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSection_AuditTrail_ExpressionId",
                table: "expression_section_audit_trails",
                newName: "ix_expression_section_audit_trails_expression_id");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSection_AuditTrail_ActorUserId",
                table: "expression_section_audit_trails",
                newName: "ix_expression_section_audit_trails_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "expression_publish_status",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "expression_publish_status",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expression_publish_status",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "expression_audit_trails",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "expression_audit_trails",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expression_audit_trails",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ExpressionId",
                table: "expression_audit_trails",
                newName: "expression_id");

            migrationBuilder.RenameColumn(
                name: "ChangedProperties",
                table: "expression_audit_trails",
                newName: "changed_properties");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "expression_audit_trails",
                newName: "actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Expression_AuditTrail_ExpressionId",
                table: "expression_audit_trails",
                newName: "ix_expression_audit_trails_expression_id");

            migrationBuilder.RenameIndex(
                name: "IX_Expression_AuditTrail_ActorUserId",
                table: "expression_audit_trails",
                newName: "ix_expression_audit_trails_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "nav_menu_item",
                table: "expressions",
                newName: "nav_menu_image");

            migrationBuilder.RenameIndex(
                name: "IX_expression_publish_status_id",
                table: "expressions",
                newName: "ix_expressions_publish_status_id");

            migrationBuilder.RenameIndex(
                name: "IX_expression_expression_type_id",
                table: "expressions",
                newName: "ix_expressions_expression_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_event_question_question_type_id",
                table: "event_questions",
                newName: "ix_event_questions_question_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_event_question_event_id",
                table: "event_questions",
                newName: "ix_event_questions_event_id");

            migrationBuilder.RenameIndex(
                name: "IX_contact_knowledge_level_id",
                table: "contacts",
                newName: "ix_contacts_knowledge_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_contact_knowledge_id",
                table: "contacts",
                newName: "ix_contacts_knowledge_id");

            migrationBuilder.RenameIndex(
                name: "IX_contact_character_id",
                table: "contacts",
                newName: "ix_contacts_character_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_stage_mapping_checkin_stage_id",
                table: "checkin_stage_mappings",
                newName: "ix_checkin_stage_mappings_checkin_stage_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_stage_mapping_checkin_id",
                table: "checkin_stage_mappings",
                newName: "ix_checkin_stage_mappings_checkin_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_stage_mapping_approver_user_id",
                table: "checkin_stage_mappings",
                newName: "ix_checkin_stage_mappings_approver_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_question_response_event_question_id",
                table: "checkin_question_responses",
                newName: "ix_checkin_question_responses_event_question_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_event_question_response_audit_trail_event_question_~",
                table: "checkin_question_response_audit_trail",
                newName: "ix_checkin_question_response_audit_trail_event_question_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_event_question_response_audit_trail_checkin_id_even~",
                table: "checkin_question_response_audit_trail",
                newName: "ix_checkin_question_response_audit_trail_checkin_id_event_ques");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_event_question_response_audit_trail_actor_user_id",
                table: "checkin_question_response_audit_trail",
                newName: "ix_checkin_question_response_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_audit_trail_checkin_id",
                table: "checkin_audit_trails",
                newName: "ix_checkin_audit_trails_checkin_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_audit_trail_actor_user_id",
                table: "checkin_audit_trails",
                newName: "ix_checkin_audit_trails_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_player_id",
                table: "checkins",
                newName: "ix_checkins_player_id");

            migrationBuilder.RenameIndex(
                name: "IX_checkin_event_id",
                table: "checkins",
                newName: "ix_checkins_event_id");

            migrationBuilder.RenameColumn(
                name: "SkillLevelId",
                table: "character_skills_mappings",
                newName: "skill_level_id");

            migrationBuilder.RenameColumn(
                name: "SkillTypeId",
                table: "character_skills_mappings",
                newName: "skill_type_id");

            migrationBuilder.RenameColumn(
                name: "CharacterId",
                table: "character_skills_mappings",
                newName: "character_id");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterSkillsMapping_SkillTypeId",
                table: "character_skills_mappings",
                newName: "ix_character_skills_mappings_skill_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterSkillsMapping_SkillLevelId",
                table: "character_skills_mappings",
                newName: "ix_character_skills_mappings_skill_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_power_mapping_power_level_id",
                table: "character_power_mappings",
                newName: "ix_character_power_mappings_power_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_power_mapping_power_id",
                table: "character_power_mappings",
                newName: "ix_character_power_mappings_power_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_power_mapping_character_id",
                table: "character_power_mappings",
                newName: "ix_character_power_mappings_character_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_knowledge_specialization_knowledge_mapping_id",
                table: "character_knowledge_specializations",
                newName: "ix_character_knowledge_specializations_knowledge_mapping_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_knowledge_mapping_knowledge_level_id",
                table: "character_knowledge_mappings",
                newName: "ix_character_knowledge_mappings_knowledge_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_knowledge_mapping_knowledge_id",
                table: "character_knowledge_mappings",
                newName: "ix_character_knowledge_mappings_knowledge_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_knowledge_mapping_character_id",
                table: "character_knowledge_mappings",
                newName: "ix_character_knowledge_mappings_character_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_blessing_mapping_character_id",
                table: "character_blessing_mappings",
                newName: "ix_character_blessing_mappings_character_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_blessing_mapping_blessing_level_id",
                table: "character_blessing_mappings",
                newName: "ix_character_blessing_mappings_blessing_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_character_blessing_mapping_blessing_id",
                table: "character_blessing_mappings",
                newName: "ix_character_blessing_mappings_blessing_id");

            migrationBuilder.RenameIndex(
                name: "IX_blessing_level_audit_trail_blessing_level_id",
                table: "blessing_level_audit_trails",
                newName: "ix_blessing_level_audit_trails_blessing_level_id");

            migrationBuilder.RenameIndex(
                name: "IX_blessing_level_audit_trail_blessing_id",
                table: "blessing_level_audit_trails",
                newName: "ix_blessing_level_audit_trails_blessing_id");

            migrationBuilder.RenameIndex(
                name: "IX_blessing_level_audit_trail_actor_user_id",
                table: "blessing_level_audit_trails",
                newName: "ix_blessing_level_audit_trails_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "stat_modifier_group",
                table: "blessing_levels",
                newName: "stat_modifier_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_blessing_level_stat_modifier_group",
                table: "blessing_levels",
                newName: "ix_blessing_levels_stat_modifier_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_blessing_level_blessing_id",
                table: "blessing_levels",
                newName: "ix_blessing_levels_blessing_id");

            migrationBuilder.RenameIndex(
                name: "IX_blessing_audit_trail_blessing_id",
                table: "blessing_audit_trails",
                newName: "ix_blessing_audit_trails_blessing_id");

            migrationBuilder.RenameIndex(
                name: "IX_blessing_audit_trail_actor_user_id",
                table: "blessing_audit_trails",
                newName: "ix_blessing_audit_trails_actor_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_xp_section_type",
                table: "xp_section_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles_audit_trail",
                table: "UserRoles_AuditTrail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_role_mapping_audit_trail",
                table: "user_role_mapping_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_role_mapping",
                table: "user_role_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_audit_trail",
                table: "User_AuditTrail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_permission_mapping_audit_trail",
                table: "role_permission_mapping_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_permission_mapping",
                table: "role_permission_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_audit_trail",
                table: "role_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role",
                table: "role",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_progression_path_audit_trail",
                table: "progression_path_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_progression_path",
                table: "progression_path",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_progression_level_audit_trail",
                table: "progression_level_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_progression_level",
                table: "progression_level",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_players",
                table: "players",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_player_audit_trail",
                table: "Player_AuditTrail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_permission_resource",
                table: "permission_resource",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_permission",
                table: "permission",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_event_schedule_item_audit_trail",
                table: "event_schedule_item_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_event_question_audit_trail",
                table: "event_question_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_event_audit_trail",
                table: "event_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_contact_audit_trail",
                table: "contact_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_characters",
                table: "characters",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_xp_mapping",
                table: "character_xp_mapping",
                columns: new[] { "character_id", "xp_section_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_assigned_xp_type_audit_trail",
                table: "assigned_xp_type_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_assigned_xp_type",
                table: "assigned_xp_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_assigned_xp_mapping_audit_trail",
                table: "assigned_xp_mapping_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_assigned_xp_mapping",
                table: "assigned_xp_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_tokens",
                table: "AspNetUserTokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_users",
                table: "AspNetUsers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_roles",
                table: "AspNetUserRoles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_logins",
                table: "AspNetUserLogins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_claims",
                table: "AspNetUserClaims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_roles",
                table: "AspNetRoles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_role_claims",
                table: "AspNetRoleClaims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_stat_levels",
                table: "stat_levels",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_state_types",
                table: "state_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_stat_description_mappings",
                table: "stat_description_mappings",
                columns: new[] { "stat_type_id", "stat_level_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_stat_modifier_groups",
                table: "stat_modifier_groups",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_stat_modifiers",
                table: "stat_modifiers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_stat_group_mappings",
                table: "stat_group_mappings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_skill_types",
                table: "skill_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_skill_sub_types",
                table: "skill_sub_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_skill_level_description_mappings",
                table: "skill_level_description_mappings",
                columns: new[] { "skill_level_id", "skill_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_skill_level_benefits",
                table: "skill_level_benefits",
                columns: new[] { "skill_level_id", "skill_type_id", "modifier_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_skill_levels",
                table: "skill_levels",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_question_types",
                table: "question_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_prerequisite_powers",
                table: "power_prerequisite_powers",
                columns: new[] { "prerequisite_id", "power_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_prerequisites",
                table: "power_prerequisites",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_path_audit_trails",
                table: "power_path_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_paths",
                table: "power_paths",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_levels",
                table: "power_levels",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_durations",
                table: "power_durations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_category_mappings",
                table: "power_category_mappings",
                columns: new[] { "power_id", "category_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_categories",
                table: "power_categories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_audit_trails",
                table: "power_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_area_of_effect_types",
                table: "power_area_of_effect_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_power_activation_timing_types",
                table: "power_activation_timing_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_powers",
                table: "powers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_modifier_types",
                table: "modifier_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_knowledge_audit_trails",
                table: "knowledge_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_knowledge_types",
                table: "knowledge_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_knowledge_education_levels",
                table: "knowledge_education_levels",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_knowledges",
                table: "knowledges",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expression_section_types",
                table: "expression_section_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expression_sections",
                table: "expression_sections",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expression_section_audit_trails",
                table: "expression_section_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expression_publish_status",
                table: "expression_publish_status",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expression_types",
                table: "expression_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expression_audit_trails",
                table: "expression_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expressions",
                table: "expressions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_event_schedule_items",
                table: "event_schedule_items",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_event_questions",
                table: "event_questions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_events",
                table: "events",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_contacts",
                table: "contacts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkin_stage_mappings",
                table: "checkin_stage_mappings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkin_stages",
                table: "checkin_stages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkin_question_responses",
                table: "checkin_question_responses",
                columns: new[] { "checkin_id", "event_question_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkin_question_response_audit_trail",
                table: "checkin_question_response_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkin_audit_trails",
                table: "checkin_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkins",
                table: "checkins",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_skills_mappings",
                table: "character_skills_mappings",
                columns: new[] { "character_id", "skill_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_power_mappings",
                table: "character_power_mappings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_knowledge_specializations",
                table: "character_knowledge_specializations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_knowledge_mappings",
                table: "character_knowledge_mappings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_blessing_mappings",
                table: "character_blessing_mappings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_blessing_level_audit_trails",
                table: "blessing_level_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_blessing_levels",
                table: "blessing_levels",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_blessing_audit_trails",
                table: "blessing_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_blessings",
                table: "blessings",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "AspNetRoleClaims",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_claims_asp_net_users_user_id",
                table: "AspNetUserClaims",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_logins_asp_net_users_user_id",
                table: "AspNetUserLogins",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "AspNetUserRoles",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_asp_net_users_user_id",
                table: "AspNetUserRoles",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                table: "AspNetUserTokens",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_assigned_xp_type_assigned_xp_type_id",
                table: "assigned_xp_mapping",
                column: "assigned_xp_type_id",
                principalTable: "assigned_xp_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_characters_character_id",
                table: "assigned_xp_mapping",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_events_event_id",
                table: "assigned_xp_mapping",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_players_player_id",
                table: "assigned_xp_mapping",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_users_assigned_by_user_id",
                table: "assigned_xp_mapping",
                column: "assigned_by_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_audit_trail_assigned_xp_mapping_assigne",
                table: "assigned_xp_mapping_audit_trail",
                column: "assigned_xp_mapping_id",
                principalTable: "assigned_xp_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_audit_trail_users_actor_user_id",
                table: "assigned_xp_mapping_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_type_audit_trail_assigned_xp_type_assigned_xp_t",
                table: "assigned_xp_type_audit_trail",
                column: "assigned_xp_type_id",
                principalTable: "assigned_xp_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_type_audit_trail_users_actor_user_id",
                table: "assigned_xp_type_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_blessing_audit_trails_blessings_blessing_id",
                table: "blessing_audit_trails",
                column: "blessing_id",
                principalTable: "blessings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_blessing_audit_trails_users_actor_user_id",
                table: "blessing_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_blessing_level_audit_trails_blessing_levels_blessing_level_",
                table: "blessing_level_audit_trails",
                column: "blessing_level_id",
                principalTable: "blessing_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_blessing_level_audit_trails_blessings_blessing_id",
                table: "blessing_level_audit_trails",
                column: "blessing_id",
                principalTable: "blessings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_blessing_level_audit_trails_users_actor_user_id",
                table: "blessing_level_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_blessing_levels_blessings_blessing_id",
                table: "blessing_levels",
                column: "blessing_id",
                principalTable: "blessings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_blessing_levels_stat_modifier_groups_stat_modifier_group_id",
                table: "blessing_levels",
                column: "stat_modifier_group_id",
                principalTable: "stat_modifier_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_blessing_mappings_blessing_levels_blessing_level_",
                table: "character_blessing_mappings",
                column: "blessing_level_id",
                principalTable: "blessing_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_blessing_mappings_blessings_blessing_id",
                table: "character_blessing_mappings",
                column: "blessing_id",
                principalTable: "blessings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_blessing_mappings_characters_character_id",
                table: "character_blessing_mappings",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_knowledge_mappings_characters_character_id",
                table: "character_knowledge_mappings",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_knowledge_mappings_knowledge_education_levels_kno",
                table: "character_knowledge_mappings",
                column: "knowledge_level_id",
                principalTable: "knowledge_education_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_knowledge_mappings_knowledges_knowledge_id",
                table: "character_knowledge_mappings",
                column: "knowledge_id",
                principalTable: "knowledges",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_knowledge_specializations_character_knowledge_map",
                table: "character_knowledge_specializations",
                column: "knowledge_mapping_id",
                principalTable: "character_knowledge_mappings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_power_mappings_characters_character_id",
                table: "character_power_mappings",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_power_mappings_power_levels_power_level_id",
                table: "character_power_mappings",
                column: "power_level_id",
                principalTable: "power_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_power_mappings_powers_power_id",
                table: "character_power_mappings",
                column: "power_id",
                principalTable: "powers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_skills_mappings_characters_character_id",
                table: "character_skills_mappings",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_skills_mappings_skill_levels_skill_level_id",
                table: "character_skills_mappings",
                column: "skill_level_id",
                principalTable: "skill_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_skills_mappings_skill_types_skill_type_id",
                table: "character_skills_mappings",
                column: "skill_type_id",
                principalTable: "skill_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_xp_mapping_characters_character_id",
                table: "character_xp_mapping",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_xp_mapping_xp_section_types_xp_section_type_id",
                table: "character_xp_mapping",
                column: "xp_section_type_id",
                principalTable: "xp_section_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_expression_sections_faction_id",
                table: "characters",
                column: "faction_id",
                principalTable: "expression_sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_expressions_expression_id",
                table: "characters",
                column: "expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_players_player_id",
                table: "characters",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_progression_path_primary_progression_id",
                table: "characters",
                column: "primary_progression_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_progression_path_secondary_progression_id",
                table: "characters",
                column: "secondary_progression_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_agility_id",
                table: "characters",
                column: "agility_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_constitution_id",
                table: "characters",
                column: "constitution_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_dexterity_id",
                table: "characters",
                column: "dexterity_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_intelligence_id",
                table: "characters",
                column: "intelligence_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_strength_id",
                table: "characters",
                column: "strength_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_willpower_id",
                table: "characters",
                column: "willpower_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_audit_trails_checkins_checkin_id",
                table: "checkin_audit_trails",
                column: "checkin_id",
                principalTable: "checkins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_audit_trails_users_actor_user_id",
                table: "checkin_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trail_checkin_question_resp",
                table: "checkin_question_response_audit_trail",
                columns: new[] { "checkin_id", "event_question_id" },
                principalTable: "checkin_question_responses",
                principalColumns: new[] { "checkin_id", "event_question_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trail_checkins_checkin_id",
                table: "checkin_question_response_audit_trail",
                column: "checkin_id",
                principalTable: "checkins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trail_event_questions_event",
                table: "checkin_question_response_audit_trail",
                column: "event_question_id",
                principalTable: "event_questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trail_users_actor_user_id",
                table: "checkin_question_response_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_responses_checkins_checkin_id",
                table: "checkin_question_responses",
                column: "checkin_id",
                principalTable: "checkins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_responses_event_questions_event_question_id",
                table: "checkin_question_responses",
                column: "event_question_id",
                principalTable: "event_questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_stage_mappings_checkin_stages_checkin_stage_id",
                table: "checkin_stage_mappings",
                column: "checkin_stage_id",
                principalTable: "checkin_stages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_stage_mappings_checkins_checkin_id",
                table: "checkin_stage_mappings",
                column: "checkin_id",
                principalTable: "checkins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_stage_mappings_users_approver_user_id",
                table: "checkin_stage_mappings",
                column: "approver_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkins_events_event_id",
                table: "checkins",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkins_players_player_id",
                table: "checkins",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_contact_audit_trail_contacts_contact_id",
                table: "contact_audit_trail",
                column: "contact_id",
                principalTable: "contacts",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_contact_audit_trail_users_actor_user_id",
                table: "contact_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_contacts_characters_character_id",
                table: "contacts",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_contacts_knowledge_education_levels_knowledge_level_id",
                table: "contacts",
                column: "knowledge_level_id",
                principalTable: "knowledge_education_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_contacts_knowledges_knowledge_id",
                table: "contacts",
                column: "knowledge_id",
                principalTable: "knowledges",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_event_audit_trail_events_event_id",
                table: "event_audit_trail",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_event_audit_trail_users_actor_user_id",
                table: "event_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_event_question_audit_trail_event_questions_event_question_id",
                table: "event_question_audit_trail",
                column: "event_question_id",
                principalTable: "event_questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_event_question_audit_trail_users_actor_user_id",
                table: "event_question_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_event_questions_events_event_id",
                table: "event_questions",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_event_questions_question_types_question_type_id",
                table: "event_questions",
                column: "question_type_id",
                principalTable: "question_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_event_schedule_item_audit_trail_event_schedule_items_event_",
                table: "event_schedule_item_audit_trail",
                column: "event_schedule_item_id",
                principalTable: "event_schedule_items",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_event_schedule_item_audit_trail_events_event_id",
                table: "event_schedule_item_audit_trail",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_event_schedule_item_audit_trail_users_actor_user_id",
                table: "event_schedule_item_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expression_audit_trails_expressions_expression_id",
                table: "expression_audit_trails",
                column: "expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expression_audit_trails_users_actor_user_id",
                table: "expression_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expression_section_audit_trails_expression_sections_section",
                table: "expression_section_audit_trails",
                column: "section_id",
                principalTable: "expression_sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expression_section_audit_trails_expressions_expression_id",
                table: "expression_section_audit_trails",
                column: "expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expression_section_audit_trails_users_actor_user_id",
                table: "expression_section_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expression_sections_expression_section_types_section_type_id",
                table: "expression_sections",
                column: "section_type_id",
                principalTable: "expression_section_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expression_sections_expression_sections_parent_id",
                table: "expression_sections",
                column: "parent_id",
                principalTable: "expression_sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expression_sections_expressions_expression_id",
                table: "expression_sections",
                column: "expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expressions_expression_publish_status_publish_status_id",
                table: "expressions",
                column: "publish_status_id",
                principalTable: "expression_publish_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_expressions_expression_types_expression_type_id",
                table: "expressions",
                column: "expression_type_id",
                principalTable: "expression_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_knowledge_audit_trails_knowledges_knowledge_id",
                table: "knowledge_audit_trails",
                column: "knowledge_id",
                principalTable: "knowledges",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_knowledge_audit_trails_users_actor_user_id",
                table: "knowledge_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_knowledges_knowledge_types_knowledge_type_id",
                table: "knowledges",
                column: "knowledge_type_id",
                principalTable: "knowledge_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_permission_permission_resource_permission_resource_id",
                table: "permission",
                column: "permission_resource_id",
                principalTable: "permission_resource",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_audit_trail_asp_net_users_actor_user_id",
                table: "Player_AuditTrail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_audit_trail_players_player_id",
                table: "Player_AuditTrail",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_players_users_user_id",
                table: "players",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_audit_trails_power_paths_power_path_id",
                table: "power_audit_trails",
                column: "power_path_id",
                principalTable: "power_paths",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_audit_trails_powers_power_id",
                table: "power_audit_trails",
                column: "power_id",
                principalTable: "powers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_audit_trails_users_actor_user_id",
                table: "power_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_category_mappings_power_categories_category_id",
                table: "power_category_mappings",
                column: "category_id",
                principalTable: "power_categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_category_mappings_powers_power_id",
                table: "power_category_mappings",
                column: "power_id",
                principalTable: "powers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_path_audit_trails_expressions_expression_id",
                table: "power_path_audit_trails",
                column: "expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_path_audit_trails_power_paths_power_path_id",
                table: "power_path_audit_trails",
                column: "power_path_id",
                principalTable: "power_paths",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_path_audit_trails_users_actor_user_id",
                table: "power_path_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_paths_expressions_expression_id",
                table: "power_paths",
                column: "expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_prerequisite_powers_power_prerequisites_prerequisite_",
                table: "power_prerequisite_powers",
                column: "prerequisite_id",
                principalTable: "power_prerequisites",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_prerequisite_powers_powers_power_id",
                table: "power_prerequisite_powers",
                column: "power_id",
                principalTable: "powers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_prerequisites_powers_power_id",
                table: "power_prerequisites",
                column: "power_id",
                principalTable: "powers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_powers_power_activation_timing_types_activation_timing_type",
                table: "powers",
                column: "activation_timing_type_id",
                principalTable: "power_activation_timing_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_powers_power_area_of_effect_types_area_of_effect_type_id",
                table: "powers",
                column: "area_of_effect_type_id",
                principalTable: "power_area_of_effect_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_powers_power_durations_duration_id",
                table: "powers",
                column: "duration_id",
                principalTable: "power_durations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_powers_power_levels_level_id",
                table: "powers",
                column: "level_id",
                principalTable: "power_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_powers_power_paths_power_path_id",
                table: "powers",
                column: "power_path_id",
                principalTable: "power_paths",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_powers_stat_modifier_groups_stat_modifier_group_id",
                table: "powers",
                column: "stat_modifier_group_id",
                principalTable: "stat_modifier_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_progression_level_progression_path_progression_path_id",
                table: "progression_level",
                column: "progression_path_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_progression_level_stat_modifier_groups_stat_modifier_group_",
                table: "progression_level",
                column: "stat_modifier_group_id",
                principalTable: "stat_modifier_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_progression_level_audit_trail_progression_level_progression",
                table: "progression_level_audit_trail",
                column: "progression_level_id",
                principalTable: "progression_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_progression_level_audit_trail_progression_path_progression_",
                table: "progression_level_audit_trail",
                column: "progression_path_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_progression_level_audit_trail_users_actor_user_id",
                table: "progression_level_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_progression_path_expressions_expression_id",
                table: "progression_path",
                column: "expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_progression_path_audit_trail_expressions_expression_id",
                table: "progression_path_audit_trail",
                column: "expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_progression_path_audit_trail_progression_path_progression_p",
                table: "progression_path_audit_trail",
                column: "progression_path_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_progression_path_audit_trail_users_actor_user_id",
                table: "progression_path_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_audit_trail_role_role_id",
                table: "role_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_audit_trail_users_actor_user_id",
                table: "role_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_permission_permission_id",
                table: "role_permission_mapping",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_role_role_id",
                table: "role_permission_mapping",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trail_role_permission_mapping",
                table: "role_permission_mapping_audit_trail",
                column: "role_permission_mapping_id",
                principalTable: "role_permission_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trail_users_actor_user_id",
                table: "role_permission_mapping_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_skill_level_benefits_modifier_types_modifier_type_id",
                table: "skill_level_benefits",
                column: "modifier_type_id",
                principalTable: "modifier_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_skill_level_benefits_skill_levels_skill_level_id",
                table: "skill_level_benefits",
                column: "skill_level_id",
                principalTable: "skill_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_skill_level_benefits_skill_types_skill_type_id",
                table: "skill_level_benefits",
                column: "skill_type_id",
                principalTable: "skill_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_skill_level_description_mappings_skill_levels_skill_level_id",
                table: "skill_level_description_mappings",
                column: "skill_level_id",
                principalTable: "skill_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_skill_level_description_mappings_skill_types_skill_type_id",
                table: "skill_level_description_mappings",
                column: "skill_type_id",
                principalTable: "skill_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_skill_types_skill_sub_types_skill_sub_type_id",
                table: "skill_types",
                column: "skill_sub_type_id",
                principalTable: "skill_sub_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_stat_description_mappings_stat_levels_stat_level_id",
                table: "stat_description_mappings",
                column: "stat_level_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_stat_description_mappings_state_types_stat_type_id",
                table: "stat_description_mappings",
                column: "stat_type_id",
                principalTable: "state_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_stat_group_mappings_expressions_target_expression_id",
                table: "stat_group_mappings",
                column: "target_expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_stat_group_mappings_stat_modifier_groups_stat_group_id",
                table: "stat_group_mappings",
                column: "stat_group_id",
                principalTable: "stat_modifier_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_stat_group_mappings_stat_modifiers_stat_modifier_id",
                table: "stat_group_mappings",
                column: "stat_modifier_id",
                principalTable: "stat_modifiers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_audit_trail_asp_net_users_actor_user_id",
                table: "User_AuditTrail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_audit_trail_asp_net_users_user_id",
                table: "User_AuditTrail",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_role_role_id",
                table: "user_role_mapping",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_users_user_id",
                table: "user_role_mapping",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trail_asp_net_users_actor_user_id",
                table: "user_role_mapping_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trail_asp_net_users_user_id",
                table: "user_role_mapping_audit_trail",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trail_role_role_id",
                table: "user_role_mapping_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trail_user_role_mapping_user_role_m",
                table: "user_role_mapping_audit_trail",
                column: "user_role_mapping_id",
                principalTable: "user_role_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_roles_role_id",
                table: "UserRoles_AuditTrail",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_users_actor_user_id",
                table: "UserRoles_AuditTrail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_users_mapping_user_id",
                table: "UserRoles_AuditTrail",
                column: "mapping_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_claims_asp_net_users_user_id",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_logins_asp_net_users_user_id",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_asp_net_users_user_id",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_assigned_xp_type_assigned_xp_type_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_characters_character_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_events_event_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_players_player_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_users_assigned_by_user_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_audit_trail_assigned_xp_mapping_assigne",
                table: "assigned_xp_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_audit_trail_users_actor_user_id",
                table: "assigned_xp_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_type_audit_trail_assigned_xp_type_assigned_xp_t",
                table: "assigned_xp_type_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_type_audit_trail_users_actor_user_id",
                table: "assigned_xp_type_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_blessing_audit_trails_blessings_blessing_id",
                table: "blessing_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_blessing_audit_trails_users_actor_user_id",
                table: "blessing_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_blessing_level_audit_trails_blessing_levels_blessing_level_",
                table: "blessing_level_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_blessing_level_audit_trails_blessings_blessing_id",
                table: "blessing_level_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_blessing_level_audit_trails_users_actor_user_id",
                table: "blessing_level_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_blessing_levels_blessings_blessing_id",
                table: "blessing_levels");

            migrationBuilder.DropForeignKey(
                name: "fk_blessing_levels_stat_modifier_groups_stat_modifier_group_id",
                table: "blessing_levels");

            migrationBuilder.DropForeignKey(
                name: "fk_character_blessing_mappings_blessing_levels_blessing_level_",
                table: "character_blessing_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_blessing_mappings_blessings_blessing_id",
                table: "character_blessing_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_blessing_mappings_characters_character_id",
                table: "character_blessing_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_knowledge_mappings_characters_character_id",
                table: "character_knowledge_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_knowledge_mappings_knowledge_education_levels_kno",
                table: "character_knowledge_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_knowledge_mappings_knowledges_knowledge_id",
                table: "character_knowledge_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_knowledge_specializations_character_knowledge_map",
                table: "character_knowledge_specializations");

            migrationBuilder.DropForeignKey(
                name: "fk_character_power_mappings_characters_character_id",
                table: "character_power_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_power_mappings_power_levels_power_level_id",
                table: "character_power_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_power_mappings_powers_power_id",
                table: "character_power_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_skills_mappings_characters_character_id",
                table: "character_skills_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_skills_mappings_skill_levels_skill_level_id",
                table: "character_skills_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_skills_mappings_skill_types_skill_type_id",
                table: "character_skills_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_xp_mapping_characters_character_id",
                table: "character_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_character_xp_mapping_xp_section_types_xp_section_type_id",
                table: "character_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_expression_sections_faction_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_expressions_expression_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_players_player_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_progression_path_primary_progression_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_progression_path_secondary_progression_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_agility_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_constitution_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_dexterity_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_intelligence_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_strength_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_willpower_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_audit_trails_checkins_checkin_id",
                table: "checkin_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_audit_trails_users_actor_user_id",
                table: "checkin_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trail_checkin_question_resp",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trail_checkins_checkin_id",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trail_event_questions_event",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trail_users_actor_user_id",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_responses_checkins_checkin_id",
                table: "checkin_question_responses");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_responses_event_questions_event_question_id",
                table: "checkin_question_responses");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_stage_mappings_checkin_stages_checkin_stage_id",
                table: "checkin_stage_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_stage_mappings_checkins_checkin_id",
                table: "checkin_stage_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_stage_mappings_users_approver_user_id",
                table: "checkin_stage_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_checkins_events_event_id",
                table: "checkins");

            migrationBuilder.DropForeignKey(
                name: "fk_checkins_players_player_id",
                table: "checkins");

            migrationBuilder.DropForeignKey(
                name: "fk_contact_audit_trail_contacts_contact_id",
                table: "contact_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_contact_audit_trail_users_actor_user_id",
                table: "contact_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_contacts_characters_character_id",
                table: "contacts");

            migrationBuilder.DropForeignKey(
                name: "fk_contacts_knowledge_education_levels_knowledge_level_id",
                table: "contacts");

            migrationBuilder.DropForeignKey(
                name: "fk_contacts_knowledges_knowledge_id",
                table: "contacts");

            migrationBuilder.DropForeignKey(
                name: "fk_event_audit_trail_events_event_id",
                table: "event_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_event_audit_trail_users_actor_user_id",
                table: "event_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_event_question_audit_trail_event_questions_event_question_id",
                table: "event_question_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_event_question_audit_trail_users_actor_user_id",
                table: "event_question_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_event_questions_events_event_id",
                table: "event_questions");

            migrationBuilder.DropForeignKey(
                name: "fk_event_questions_question_types_question_type_id",
                table: "event_questions");

            migrationBuilder.DropForeignKey(
                name: "fk_event_schedule_item_audit_trail_event_schedule_items_event_",
                table: "event_schedule_item_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_event_schedule_item_audit_trail_events_event_id",
                table: "event_schedule_item_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_event_schedule_item_audit_trail_users_actor_user_id",
                table: "event_schedule_item_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_expression_audit_trails_expressions_expression_id",
                table: "expression_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_expression_audit_trails_users_actor_user_id",
                table: "expression_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_expression_section_audit_trails_expression_sections_section",
                table: "expression_section_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_expression_section_audit_trails_expressions_expression_id",
                table: "expression_section_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_expression_section_audit_trails_users_actor_user_id",
                table: "expression_section_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_expression_sections_expression_section_types_section_type_id",
                table: "expression_sections");

            migrationBuilder.DropForeignKey(
                name: "fk_expression_sections_expression_sections_parent_id",
                table: "expression_sections");

            migrationBuilder.DropForeignKey(
                name: "fk_expression_sections_expressions_expression_id",
                table: "expression_sections");

            migrationBuilder.DropForeignKey(
                name: "fk_expressions_expression_publish_status_publish_status_id",
                table: "expressions");

            migrationBuilder.DropForeignKey(
                name: "fk_expressions_expression_types_expression_type_id",
                table: "expressions");

            migrationBuilder.DropForeignKey(
                name: "fk_knowledge_audit_trails_knowledges_knowledge_id",
                table: "knowledge_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_knowledge_audit_trails_users_actor_user_id",
                table: "knowledge_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_knowledges_knowledge_types_knowledge_type_id",
                table: "knowledges");

            migrationBuilder.DropForeignKey(
                name: "fk_permission_permission_resource_permission_resource_id",
                table: "permission");

            migrationBuilder.DropForeignKey(
                name: "fk_player_audit_trail_asp_net_users_actor_user_id",
                table: "Player_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_player_audit_trail_players_player_id",
                table: "Player_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_players_users_user_id",
                table: "players");

            migrationBuilder.DropForeignKey(
                name: "fk_power_audit_trails_power_paths_power_path_id",
                table: "power_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_power_audit_trails_powers_power_id",
                table: "power_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_power_audit_trails_users_actor_user_id",
                table: "power_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_power_category_mappings_power_categories_category_id",
                table: "power_category_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_power_category_mappings_powers_power_id",
                table: "power_category_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_power_path_audit_trails_expressions_expression_id",
                table: "power_path_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_power_path_audit_trails_power_paths_power_path_id",
                table: "power_path_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_power_path_audit_trails_users_actor_user_id",
                table: "power_path_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_power_paths_expressions_expression_id",
                table: "power_paths");

            migrationBuilder.DropForeignKey(
                name: "fk_power_prerequisite_powers_power_prerequisites_prerequisite_",
                table: "power_prerequisite_powers");

            migrationBuilder.DropForeignKey(
                name: "fk_power_prerequisite_powers_powers_power_id",
                table: "power_prerequisite_powers");

            migrationBuilder.DropForeignKey(
                name: "fk_power_prerequisites_powers_power_id",
                table: "power_prerequisites");

            migrationBuilder.DropForeignKey(
                name: "fk_powers_power_activation_timing_types_activation_timing_type",
                table: "powers");

            migrationBuilder.DropForeignKey(
                name: "fk_powers_power_area_of_effect_types_area_of_effect_type_id",
                table: "powers");

            migrationBuilder.DropForeignKey(
                name: "fk_powers_power_durations_duration_id",
                table: "powers");

            migrationBuilder.DropForeignKey(
                name: "fk_powers_power_levels_level_id",
                table: "powers");

            migrationBuilder.DropForeignKey(
                name: "fk_powers_power_paths_power_path_id",
                table: "powers");

            migrationBuilder.DropForeignKey(
                name: "fk_powers_stat_modifier_groups_stat_modifier_group_id",
                table: "powers");

            migrationBuilder.DropForeignKey(
                name: "fk_progression_level_progression_path_progression_path_id",
                table: "progression_level");

            migrationBuilder.DropForeignKey(
                name: "fk_progression_level_stat_modifier_groups_stat_modifier_group_",
                table: "progression_level");

            migrationBuilder.DropForeignKey(
                name: "fk_progression_level_audit_trail_progression_level_progression",
                table: "progression_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_progression_level_audit_trail_progression_path_progression_",
                table: "progression_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_progression_level_audit_trail_users_actor_user_id",
                table: "progression_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_progression_path_expressions_expression_id",
                table: "progression_path");

            migrationBuilder.DropForeignKey(
                name: "fk_progression_path_audit_trail_expressions_expression_id",
                table: "progression_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_progression_path_audit_trail_progression_path_progression_p",
                table: "progression_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_progression_path_audit_trail_users_actor_user_id",
                table: "progression_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_audit_trail_role_role_id",
                table: "role_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_audit_trail_users_actor_user_id",
                table: "role_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_permission_permission_id",
                table: "role_permission_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_role_role_id",
                table: "role_permission_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trail_role_permission_mapping",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trail_users_actor_user_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_skill_level_benefits_modifier_types_modifier_type_id",
                table: "skill_level_benefits");

            migrationBuilder.DropForeignKey(
                name: "fk_skill_level_benefits_skill_levels_skill_level_id",
                table: "skill_level_benefits");

            migrationBuilder.DropForeignKey(
                name: "fk_skill_level_benefits_skill_types_skill_type_id",
                table: "skill_level_benefits");

            migrationBuilder.DropForeignKey(
                name: "fk_skill_level_description_mappings_skill_levels_skill_level_id",
                table: "skill_level_description_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_skill_level_description_mappings_skill_types_skill_type_id",
                table: "skill_level_description_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_skill_types_skill_sub_types_skill_sub_type_id",
                table: "skill_types");

            migrationBuilder.DropForeignKey(
                name: "fk_stat_description_mappings_stat_levels_stat_level_id",
                table: "stat_description_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_stat_description_mappings_state_types_stat_type_id",
                table: "stat_description_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_stat_group_mappings_expressions_target_expression_id",
                table: "stat_group_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_stat_group_mappings_stat_modifier_groups_stat_group_id",
                table: "stat_group_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_stat_group_mappings_stat_modifiers_stat_modifier_id",
                table: "stat_group_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_user_audit_trail_asp_net_users_actor_user_id",
                table: "User_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_audit_trail_asp_net_users_user_id",
                table: "User_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_role_role_id",
                table: "user_role_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_users_user_id",
                table: "user_role_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trail_asp_net_users_actor_user_id",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trail_asp_net_users_user_id",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trail_role_role_id",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trail_user_role_mapping_user_role_m",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_roles_role_id",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_users_actor_user_id",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_users_mapping_user_id",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_xp_section_type",
                table: "xp_section_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles_audit_trail",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_role_mapping_audit_trail",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_role_mapping",
                table: "user_role_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_audit_trail",
                table: "User_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_permission_mapping_audit_trail",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_permission_mapping",
                table: "role_permission_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_audit_trail",
                table: "role_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "pk_progression_path_audit_trail",
                table: "progression_path_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_progression_path",
                table: "progression_path");

            migrationBuilder.DropPrimaryKey(
                name: "pk_progression_level_audit_trail",
                table: "progression_level_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_progression_level",
                table: "progression_level");

            migrationBuilder.DropPrimaryKey(
                name: "pk_players",
                table: "players");

            migrationBuilder.DropPrimaryKey(
                name: "pk_player_audit_trail",
                table: "Player_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_permission_resource",
                table: "permission_resource");

            migrationBuilder.DropPrimaryKey(
                name: "pk_permission",
                table: "permission");

            migrationBuilder.DropPrimaryKey(
                name: "pk_event_schedule_item_audit_trail",
                table: "event_schedule_item_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_event_question_audit_trail",
                table: "event_question_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_event_audit_trail",
                table: "event_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_contact_audit_trail",
                table: "contact_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_characters",
                table: "characters");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_xp_mapping",
                table: "character_xp_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "pk_assigned_xp_type_audit_trail",
                table: "assigned_xp_type_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_assigned_xp_type",
                table: "assigned_xp_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_assigned_xp_mapping_audit_trail",
                table: "assigned_xp_mapping_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_assigned_xp_mapping",
                table: "assigned_xp_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_tokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_users",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_roles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_logins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_claims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_roles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_role_claims",
                table: "AspNetRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_state_types",
                table: "state_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_stat_modifiers",
                table: "stat_modifiers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_stat_modifier_groups",
                table: "stat_modifier_groups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_stat_levels",
                table: "stat_levels");

            migrationBuilder.DropPrimaryKey(
                name: "pk_stat_group_mappings",
                table: "stat_group_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_stat_description_mappings",
                table: "stat_description_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_skill_types",
                table: "skill_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_skill_sub_types",
                table: "skill_sub_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_skill_levels",
                table: "skill_levels");

            migrationBuilder.DropPrimaryKey(
                name: "pk_skill_level_description_mappings",
                table: "skill_level_description_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_skill_level_benefits",
                table: "skill_level_benefits");

            migrationBuilder.DropPrimaryKey(
                name: "pk_question_types",
                table: "question_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_powers",
                table: "powers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_prerequisites",
                table: "power_prerequisites");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_prerequisite_powers",
                table: "power_prerequisite_powers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_paths",
                table: "power_paths");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_path_audit_trails",
                table: "power_path_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_levels",
                table: "power_levels");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_durations",
                table: "power_durations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_category_mappings",
                table: "power_category_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_categories",
                table: "power_categories");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_audit_trails",
                table: "power_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_area_of_effect_types",
                table: "power_area_of_effect_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_power_activation_timing_types",
                table: "power_activation_timing_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_modifier_types",
                table: "modifier_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_knowledges",
                table: "knowledges");

            migrationBuilder.DropPrimaryKey(
                name: "pk_knowledge_types",
                table: "knowledge_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_knowledge_education_levels",
                table: "knowledge_education_levels");

            migrationBuilder.DropPrimaryKey(
                name: "pk_knowledge_audit_trails",
                table: "knowledge_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expressions",
                table: "expressions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expression_types",
                table: "expression_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expression_sections",
                table: "expression_sections");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expression_section_types",
                table: "expression_section_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expression_section_audit_trails",
                table: "expression_section_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expression_publish_status",
                table: "expression_publish_status");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expression_audit_trails",
                table: "expression_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_events",
                table: "events");

            migrationBuilder.DropPrimaryKey(
                name: "pk_event_schedule_items",
                table: "event_schedule_items");

            migrationBuilder.DropPrimaryKey(
                name: "pk_event_questions",
                table: "event_questions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_contacts",
                table: "contacts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkins",
                table: "checkins");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkin_stages",
                table: "checkin_stages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkin_stage_mappings",
                table: "checkin_stage_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkin_question_responses",
                table: "checkin_question_responses");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkin_question_response_audit_trail",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkin_audit_trails",
                table: "checkin_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_skills_mappings",
                table: "character_skills_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_power_mappings",
                table: "character_power_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_knowledge_specializations",
                table: "character_knowledge_specializations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_knowledge_mappings",
                table: "character_knowledge_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_blessing_mappings",
                table: "character_blessing_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_blessings",
                table: "blessings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_blessing_levels",
                table: "blessing_levels");

            migrationBuilder.DropPrimaryKey(
                name: "pk_blessing_level_audit_trails",
                table: "blessing_level_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_blessing_audit_trails",
                table: "blessing_audit_trails");

            migrationBuilder.RenameTable(
                name: "players",
                newName: "Players");

            migrationBuilder.RenameTable(
                name: "characters",
                newName: "Characters");

            migrationBuilder.RenameTable(
                name: "state_types",
                newName: "StateTypes");

            migrationBuilder.RenameTable(
                name: "stat_modifiers",
                newName: "stat_modifier");

            migrationBuilder.RenameTable(
                name: "stat_modifier_groups",
                newName: "stat_modifier_group");

            migrationBuilder.RenameTable(
                name: "stat_levels",
                newName: "StatLevels");

            migrationBuilder.RenameTable(
                name: "stat_group_mappings",
                newName: "stat_group_mapping");

            migrationBuilder.RenameTable(
                name: "stat_description_mappings",
                newName: "StatDescriptionMappings");

            migrationBuilder.RenameTable(
                name: "skill_types",
                newName: "SkillType");

            migrationBuilder.RenameTable(
                name: "skill_sub_types",
                newName: "SkillSubType");

            migrationBuilder.RenameTable(
                name: "skill_levels",
                newName: "skill_level");

            migrationBuilder.RenameTable(
                name: "skill_level_description_mappings",
                newName: "SkillLevelDescriptionMapping");

            migrationBuilder.RenameTable(
                name: "skill_level_benefits",
                newName: "SkillLevelBenefit");

            migrationBuilder.RenameTable(
                name: "question_types",
                newName: "question_type");

            migrationBuilder.RenameTable(
                name: "powers",
                newName: "power");

            migrationBuilder.RenameTable(
                name: "power_prerequisites",
                newName: "power_prerequisite");

            migrationBuilder.RenameTable(
                name: "power_prerequisite_powers",
                newName: "power_prerequisite_power");

            migrationBuilder.RenameTable(
                name: "power_paths",
                newName: "power_path");

            migrationBuilder.RenameTable(
                name: "power_path_audit_trails",
                newName: "power_path_audit_trail");

            migrationBuilder.RenameTable(
                name: "power_levels",
                newName: "power_level");

            migrationBuilder.RenameTable(
                name: "power_durations",
                newName: "power_duration");

            migrationBuilder.RenameTable(
                name: "power_category_mappings",
                newName: "power_category_mapping");

            migrationBuilder.RenameTable(
                name: "power_categories",
                newName: "power_category");

            migrationBuilder.RenameTable(
                name: "power_audit_trails",
                newName: "power_audit_trail");

            migrationBuilder.RenameTable(
                name: "power_area_of_effect_types",
                newName: "power_area_of_effect_type");

            migrationBuilder.RenameTable(
                name: "power_activation_timing_types",
                newName: "power_activation_timing_type");

            migrationBuilder.RenameTable(
                name: "modifier_types",
                newName: "modifier_type");

            migrationBuilder.RenameTable(
                name: "knowledges",
                newName: "knowledge");

            migrationBuilder.RenameTable(
                name: "knowledge_types",
                newName: "knowledge_type");

            migrationBuilder.RenameTable(
                name: "knowledge_education_levels",
                newName: "knowledge_education_level");

            migrationBuilder.RenameTable(
                name: "knowledge_audit_trails",
                newName: "knowledges_audit_trail");

            migrationBuilder.RenameTable(
                name: "expressions",
                newName: "expression");

            migrationBuilder.RenameTable(
                name: "expression_types",
                newName: "expression_type");

            migrationBuilder.RenameTable(
                name: "expression_sections",
                newName: "ExpressionSections");

            migrationBuilder.RenameTable(
                name: "expression_section_types",
                newName: "ExpressionSectionTypes");

            migrationBuilder.RenameTable(
                name: "expression_section_audit_trails",
                newName: "ExpressionSection_AuditTrail");

            migrationBuilder.RenameTable(
                name: "expression_publish_status",
                newName: "ExpressionPublishStatus");

            migrationBuilder.RenameTable(
                name: "expression_audit_trails",
                newName: "Expression_AuditTrail");

            migrationBuilder.RenameTable(
                name: "events",
                newName: "event");

            migrationBuilder.RenameTable(
                name: "event_schedule_items",
                newName: "event_schedule_item");

            migrationBuilder.RenameTable(
                name: "event_questions",
                newName: "event_question");

            migrationBuilder.RenameTable(
                name: "contacts",
                newName: "contact");

            migrationBuilder.RenameTable(
                name: "checkins",
                newName: "checkin");

            migrationBuilder.RenameTable(
                name: "checkin_stages",
                newName: "checkin_stage");

            migrationBuilder.RenameTable(
                name: "checkin_stage_mappings",
                newName: "checkin_stage_mapping");

            migrationBuilder.RenameTable(
                name: "checkin_question_responses",
                newName: "checkin_question_response");

            migrationBuilder.RenameTable(
                name: "checkin_question_response_audit_trail",
                newName: "checkin_event_question_response_audit_trail");

            migrationBuilder.RenameTable(
                name: "checkin_audit_trails",
                newName: "checkin_audit_trail");

            migrationBuilder.RenameTable(
                name: "character_skills_mappings",
                newName: "CharacterSkillsMapping");

            migrationBuilder.RenameTable(
                name: "character_power_mappings",
                newName: "character_power_mapping");

            migrationBuilder.RenameTable(
                name: "character_knowledge_specializations",
                newName: "character_knowledge_specialization");

            migrationBuilder.RenameTable(
                name: "character_knowledge_mappings",
                newName: "character_knowledge_mapping");

            migrationBuilder.RenameTable(
                name: "character_blessing_mappings",
                newName: "character_blessing_mapping");

            migrationBuilder.RenameTable(
                name: "blessings",
                newName: "blessing");

            migrationBuilder.RenameTable(
                name: "blessing_levels",
                newName: "blessing_level");

            migrationBuilder.RenameTable(
                name: "blessing_level_audit_trails",
                newName: "blessing_level_audit_trail");

            migrationBuilder.RenameTable(
                name: "blessing_audit_trails",
                newName: "blessing_audit_trail");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "UserRoles_AuditTrail",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "UserRoles_AuditTrail",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserRoles_AuditTrail",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "UserRoles_AuditTrail",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "mapping_user_id",
                table: "UserRoles_AuditTrail",
                newName: "MappingUserId");

            migrationBuilder.RenameColumn(
                name: "changed_properties",
                table: "UserRoles_AuditTrail",
                newName: "ChangedProperties");

            migrationBuilder.RenameColumn(
                name: "actor_user_id",
                table: "UserRoles_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_audit_trail_role_id",
                table: "UserRoles_AuditTrail",
                newName: "IX_UserRoles_AuditTrail_RoleId");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_audit_trail_mapping_user_id",
                table: "UserRoles_AuditTrail",
                newName: "IX_UserRoles_AuditTrail_MappingUserId");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_audit_trail_actor_user_id",
                table: "UserRoles_AuditTrail",
                newName: "IX_UserRoles_AuditTrail_ActorUserId");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trail_user_role_mapping_id",
                table: "user_role_mapping_audit_trail",
                newName: "IX_user_role_mapping_audit_trail_user_role_mapping_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trail_user_id",
                table: "user_role_mapping_audit_trail",
                newName: "IX_user_role_mapping_audit_trail_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trail_role_id",
                table: "user_role_mapping_audit_trail",
                newName: "IX_user_role_mapping_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trail_actor_user_id",
                table: "user_role_mapping_audit_trail",
                newName: "IX_user_role_mapping_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_user_id",
                table: "user_role_mapping",
                newName: "IX_user_role_mapping_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_role_id",
                table: "user_role_mapping",
                newName: "IX_user_role_mapping_role_id");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "User_AuditTrail",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "User_AuditTrail",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "User_AuditTrail",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "User_AuditTrail",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "changed_properties",
                table: "User_AuditTrail",
                newName: "ChangedProperties");

            migrationBuilder.RenameColumn(
                name: "actor_user_id",
                table: "User_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "ix_user_audit_trail_user_id",
                table: "User_AuditTrail",
                newName: "IX_User_AuditTrail_UserId");

            migrationBuilder.RenameIndex(
                name: "ix_user_audit_trail_actor_user_id",
                table: "User_AuditTrail",
                newName: "IX_User_AuditTrail_ActorUserId");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trail_role_id",
                table: "role_permission_mapping_audit_trail",
                newName: "IX_role_permission_mapping_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trail_permission_id",
                table: "role_permission_mapping_audit_trail",
                newName: "IX_role_permission_mapping_audit_trail_permission_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trail_actor_user_id",
                table: "role_permission_mapping_audit_trail",
                newName: "IX_role_permission_mapping_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trail_role_permission_mapping",
                table: "role_permission_mapping_audit_trail",
                newName: "IX_role_permission_mapping_audit_trail_role_permission_mapping~");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_role_id",
                table: "role_permission_mapping",
                newName: "IX_role_permission_mapping_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_permission_id",
                table: "role_permission_mapping",
                newName: "IX_role_permission_mapping_permission_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_audit_trail_role_id",
                table: "role_audit_trail",
                newName: "IX_role_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_audit_trail_actor_user_id",
                table: "role_audit_trail",
                newName: "IX_role_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_progression_path_audit_trail_progression_path_id",
                table: "progression_path_audit_trail",
                newName: "IX_progression_path_audit_trail_progression_path_id");

            migrationBuilder.RenameIndex(
                name: "ix_progression_path_audit_trail_expression_id",
                table: "progression_path_audit_trail",
                newName: "IX_progression_path_audit_trail_expression_id");

            migrationBuilder.RenameIndex(
                name: "ix_progression_path_audit_trail_actor_user_id",
                table: "progression_path_audit_trail",
                newName: "IX_progression_path_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_progression_path_expression_id",
                table: "progression_path",
                newName: "IX_progression_path_expression_id");

            migrationBuilder.RenameIndex(
                name: "ix_progression_level_audit_trail_progression_path_id",
                table: "progression_level_audit_trail",
                newName: "IX_progression_level_audit_trail_progression_path_id");

            migrationBuilder.RenameIndex(
                name: "ix_progression_level_audit_trail_progression_level_id",
                table: "progression_level_audit_trail",
                newName: "IX_progression_level_audit_trail_progression_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_progression_level_audit_trail_actor_user_id",
                table: "progression_level_audit_trail",
                newName: "IX_progression_level_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "stat_modifier_group_id",
                table: "progression_level",
                newName: "stat_modifier_group");

            migrationBuilder.RenameIndex(
                name: "ix_progression_level_progression_path_id",
                table: "progression_level",
                newName: "IX_progression_level_progression_path_id");

            migrationBuilder.RenameIndex(
                name: "ix_progression_level_stat_modifier_group_id",
                table: "progression_level",
                newName: "IX_progression_level_stat_modifier_group");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Players",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Players",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Players",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_players_player_number",
                table: "Players",
                newName: "IX_Players_player_number");

            migrationBuilder.RenameIndex(
                name: "ix_players_lookup_id",
                table: "Players",
                newName: "IX_Players_lookup_id");

            migrationBuilder.RenameIndex(
                name: "ix_players_user_id",
                table: "Players",
                newName: "IX_Players_UserId");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Player_AuditTrail",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "Player_AuditTrail",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Player_AuditTrail",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "player_id",
                table: "Player_AuditTrail",
                newName: "PlayerId");

            migrationBuilder.RenameColumn(
                name: "changed_properties",
                table: "Player_AuditTrail",
                newName: "ChangedProperties");

            migrationBuilder.RenameColumn(
                name: "actor_user_id",
                table: "Player_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "ix_player_audit_trail_player_id",
                table: "Player_AuditTrail",
                newName: "IX_Player_AuditTrail_PlayerId");

            migrationBuilder.RenameIndex(
                name: "ix_player_audit_trail_actor_user_id",
                table: "Player_AuditTrail",
                newName: "IX_Player_AuditTrail_ActorUserId");

            migrationBuilder.RenameIndex(
                name: "ix_permission_permission_resource_id",
                table: "permission",
                newName: "IX_permission_permission_resource_id");

            migrationBuilder.RenameColumn(
                name: "event_id",
                table: "event_schedule_item_audit_trail",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "ix_event_schedule_item_audit_trail_event_schedule_item_id",
                table: "event_schedule_item_audit_trail",
                newName: "IX_event_schedule_item_audit_trail_event_schedule_item_id");

            migrationBuilder.RenameIndex(
                name: "ix_event_schedule_item_audit_trail_actor_user_id",
                table: "event_schedule_item_audit_trail",
                newName: "IX_event_schedule_item_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_event_schedule_item_audit_trail_event_id",
                table: "event_schedule_item_audit_trail",
                newName: "IX_event_schedule_item_audit_trail_EventId");

            migrationBuilder.RenameIndex(
                name: "ix_event_question_audit_trail_event_question_id",
                table: "event_question_audit_trail",
                newName: "IX_event_question_audit_trail_event_question_id");

            migrationBuilder.RenameIndex(
                name: "ix_event_question_audit_trail_actor_user_id",
                table: "event_question_audit_trail",
                newName: "IX_event_question_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_event_audit_trail_event_id",
                table: "event_audit_trail",
                newName: "IX_event_audit_trail_event_id");

            migrationBuilder.RenameIndex(
                name: "ix_event_audit_trail_actor_user_id",
                table: "event_audit_trail",
                newName: "IX_event_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_contact_audit_trail_contact_id",
                table: "contact_audit_trail",
                newName: "IX_contact_audit_trail_contact_id");

            migrationBuilder.RenameIndex(
                name: "ix_contact_audit_trail_actor_user_id",
                table: "contact_audit_trail",
                newName: "IX_contact_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Characters",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "background",
                table: "Characters",
                newName: "Background");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Characters",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "willpower_id",
                table: "Characters",
                newName: "WillpowerId");

            migrationBuilder.RenameColumn(
                name: "strength_id",
                table: "Characters",
                newName: "StrengthId");

            migrationBuilder.RenameColumn(
                name: "stat_experience_points",
                table: "Characters",
                newName: "StatExperiencePoints");

            migrationBuilder.RenameColumn(
                name: "player_id",
                table: "Characters",
                newName: "PlayerId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Characters",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "intelligence_id",
                table: "Characters",
                newName: "IntelligenceId");

            migrationBuilder.RenameColumn(
                name: "faction_id",
                table: "Characters",
                newName: "FactionId");

            migrationBuilder.RenameColumn(
                name: "expression_id",
                table: "Characters",
                newName: "ExpressionId");

            migrationBuilder.RenameColumn(
                name: "dexterity_id",
                table: "Characters",
                newName: "DexterityId");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Characters",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "constitution_id",
                table: "Characters",
                newName: "ConstitutionId");

            migrationBuilder.RenameColumn(
                name: "agility_id",
                table: "Characters",
                newName: "AgilityId");

            migrationBuilder.RenameIndex(
                name: "ix_characters_secondary_progression_id",
                table: "Characters",
                newName: "IX_Characters_secondary_progression_id");

            migrationBuilder.RenameIndex(
                name: "ix_characters_primary_progression_id",
                table: "Characters",
                newName: "IX_Characters_primary_progression_id");

            migrationBuilder.RenameIndex(
                name: "ix_characters_willpower_id",
                table: "Characters",
                newName: "IX_Characters_WillpowerId");

            migrationBuilder.RenameIndex(
                name: "ix_characters_strength_id",
                table: "Characters",
                newName: "IX_Characters_StrengthId");

            migrationBuilder.RenameIndex(
                name: "ix_characters_player_id",
                table: "Characters",
                newName: "IX_Characters_PlayerId");

            migrationBuilder.RenameIndex(
                name: "ix_characters_intelligence_id",
                table: "Characters",
                newName: "IX_Characters_IntelligenceId");

            migrationBuilder.RenameIndex(
                name: "ix_characters_faction_id",
                table: "Characters",
                newName: "IX_Characters_FactionId");

            migrationBuilder.RenameIndex(
                name: "ix_characters_expression_id",
                table: "Characters",
                newName: "IX_Characters_ExpressionId");

            migrationBuilder.RenameIndex(
                name: "ix_characters_dexterity_id",
                table: "Characters",
                newName: "IX_Characters_DexterityId");

            migrationBuilder.RenameIndex(
                name: "ix_characters_constitution_id",
                table: "Characters",
                newName: "IX_Characters_ConstitutionId");

            migrationBuilder.RenameIndex(
                name: "ix_characters_agility_id",
                table: "Characters",
                newName: "IX_Characters_AgilityId");

            migrationBuilder.RenameIndex(
                name: "ix_character_xp_mapping_xp_section_type_id",
                table: "character_xp_mapping",
                newName: "IX_character_xp_mapping_xp_section_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_type_audit_trail_assigned_xp_type_id",
                table: "assigned_xp_type_audit_trail",
                newName: "IX_assigned_xp_type_audit_trail_assigned_xp_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_type_audit_trail_actor_user_id",
                table: "assigned_xp_type_audit_trail",
                newName: "IX_assigned_xp_type_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "assigned_xp_type",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "assigned_xp_type",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_audit_trail_assigned_xp_mapping_id",
                table: "assigned_xp_mapping_audit_trail",
                newName: "IX_assigned_xp_mapping_audit_trail_assigned_xp_mapping_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_audit_trail_actor_user_id",
                table: "assigned_xp_mapping_audit_trail",
                newName: "IX_assigned_xp_mapping_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_player_id",
                table: "assigned_xp_mapping",
                newName: "IX_assigned_xp_mapping_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_event_id",
                table: "assigned_xp_mapping",
                newName: "IX_assigned_xp_mapping_event_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_character_id",
                table: "assigned_xp_mapping",
                newName: "IX_assigned_xp_mapping_character_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_assigned_xp_type_id",
                table: "assigned_xp_mapping",
                newName: "IX_assigned_xp_mapping_assigned_xp_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_assigned_by_user_id",
                table: "assigned_xp_mapping",
                newName: "IX_assigned_xp_mapping_assigned_by_user_id");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "AspNetUserTokens",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AspNetUserTokens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                table: "AspNetUserTokens",
                newName: "LoginProvider");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserTokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AspNetUsers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUsers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "two_factor_enabled",
                table: "AspNetUsers",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "security_stamp",
                table: "AspNetUsers",
                newName: "SecurityStamp");

            migrationBuilder.RenameColumn(
                name: "phone_number_confirmed",
                table: "AspNetUsers",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "AspNetUsers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "AspNetUsers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "normalized_user_name",
                table: "AspNetUsers",
                newName: "NormalizedUserName");

            migrationBuilder.RenameColumn(
                name: "normalized_email",
                table: "AspNetUsers",
                newName: "NormalizedEmail");

            migrationBuilder.RenameColumn(
                name: "lockout_end",
                table: "AspNetUsers",
                newName: "LockoutEnd");

            migrationBuilder.RenameColumn(
                name: "lockout_enabled",
                table: "AspNetUsers",
                newName: "LockoutEnabled");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                table: "AspNetUsers",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "AspNetUsers",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "access_failed_count",
                table: "AspNetUsers",
                newName: "AccessFailedCount");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "AspNetUserRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserLogins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "provider_display_name",
                table: "AspNetUserLogins",
                newName: "ProviderDisplayName");

            migrationBuilder.RenameColumn(
                name: "provider_key",
                table: "AspNetUserLogins",
                newName: "ProviderKey");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                table: "AspNetUserLogins",
                newName: "LoginProvider");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUserClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserClaims",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                table: "AspNetUserClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                table: "AspNetUserClaims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AspNetRoles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "normalized_name",
                table: "AspNetRoles",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "AspNetRoles",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoleClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "AspNetRoleClaims",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                table: "AspNetRoleClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                table: "AspNetRoleClaims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "StateTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "StateTypes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StateTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "short_name",
                table: "StateTypes",
                newName: "ShortName");

            migrationBuilder.RenameColumn(
                name: "bonus",
                table: "StatLevels",
                newName: "Bonus");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StatLevels",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "xp_cost",
                table: "StatLevels",
                newName: "XPCost");

            migrationBuilder.RenameColumn(
                name: "total_xp_cost",
                table: "StatLevels",
                newName: "TotalXPCost");

            migrationBuilder.RenameColumn(
                name: "scale_with_level",
                table: "stat_group_mapping",
                newName: "scales_with_level");

            migrationBuilder.RenameIndex(
                name: "ix_stat_group_mappings_target_expression_id",
                table: "stat_group_mapping",
                newName: "IX_stat_group_mapping_target_expression_id");

            migrationBuilder.RenameIndex(
                name: "ix_stat_group_mappings_stat_modifier_id",
                table: "stat_group_mapping",
                newName: "IX_stat_group_mapping_stat_modifier_id");

            migrationBuilder.RenameIndex(
                name: "ix_stat_group_mappings_stat_group_id",
                table: "stat_group_mapping",
                newName: "IX_stat_group_mapping_stat_group_id");

            migrationBuilder.RenameColumn(
                name: "reasonable_expectation",
                table: "StatDescriptionMappings",
                newName: "ReasonableExpectation");

            migrationBuilder.RenameColumn(
                name: "stat_level_id",
                table: "StatDescriptionMappings",
                newName: "StatLevelId");

            migrationBuilder.RenameColumn(
                name: "stat_type_id",
                table: "StatDescriptionMappings",
                newName: "StatTypeId");

            migrationBuilder.RenameIndex(
                name: "ix_stat_description_mappings_stat_level_id",
                table: "StatDescriptionMappings",
                newName: "IX_StatDescriptionMappings_StatLevelId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "SkillType",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "SkillType",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SkillType",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "skill_sub_type_id",
                table: "SkillType",
                newName: "SkillSubTypeId");

            migrationBuilder.RenameIndex(
                name: "ix_skill_types_skill_sub_type_id",
                table: "SkillType",
                newName: "IX_SkillType_SkillSubTypeId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "SkillSubType",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "SkillSubType",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SkillSubType",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "SkillLevelDescriptionMapping",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "skill_type_id",
                table: "SkillLevelDescriptionMapping",
                newName: "SkillTypeId");

            migrationBuilder.RenameColumn(
                name: "skill_level_id",
                table: "SkillLevelDescriptionMapping",
                newName: "SkillLevelId");

            migrationBuilder.RenameIndex(
                name: "ix_skill_level_description_mappings_skill_type_id",
                table: "SkillLevelDescriptionMapping",
                newName: "IX_SkillLevelDescriptionMapping_SkillTypeId");

            migrationBuilder.RenameColumn(
                name: "modifier",
                table: "SkillLevelBenefit",
                newName: "Modifier");

            migrationBuilder.RenameColumn(
                name: "modifier_type_id",
                table: "SkillLevelBenefit",
                newName: "ModifierTypeId");

            migrationBuilder.RenameColumn(
                name: "skill_type_id",
                table: "SkillLevelBenefit",
                newName: "SkillTypeId");

            migrationBuilder.RenameColumn(
                name: "skill_level_id",
                table: "SkillLevelBenefit",
                newName: "SkillLevelId");

            migrationBuilder.RenameIndex(
                name: "ix_skill_level_benefits_skill_type_id",
                table: "SkillLevelBenefit",
                newName: "IX_SkillLevelBenefit_SkillTypeId");

            migrationBuilder.RenameIndex(
                name: "ix_skill_level_benefits_modifier_type_id",
                table: "SkillLevelBenefit",
                newName: "IX_SkillLevelBenefit_ModifierTypeId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "power",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "limitation",
                table: "power",
                newName: "Limitation");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "power",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "power",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "stat_modifier_group_id",
                table: "power",
                newName: "stat_modifier_group");

            migrationBuilder.RenameColumn(
                name: "other_fields",
                table: "power",
                newName: "OtherFields");

            migrationBuilder.RenameColumn(
                name: "level_id",
                table: "power",
                newName: "LevelId");

            migrationBuilder.RenameColumn(
                name: "is_power_use",
                table: "power",
                newName: "IsPowerUse");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "power",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "game_mechanic_effect",
                table: "power",
                newName: "GameMechanicEffect");

            migrationBuilder.RenameColumn(
                name: "duration_id",
                table: "power",
                newName: "DurationId");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "power",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "area_of_effect_type_id",
                table: "power",
                newName: "AreaOfEffectTypeId");

            migrationBuilder.RenameColumn(
                name: "activation_timing_type_id",
                table: "power",
                newName: "ActivationTimingTypeId");

            migrationBuilder.RenameIndex(
                name: "ix_powers_stat_modifier_group_id",
                table: "power",
                newName: "IX_power_stat_modifier_group");

            migrationBuilder.RenameIndex(
                name: "ix_powers_power_path_id",
                table: "power",
                newName: "IX_power_power_path_id");

            migrationBuilder.RenameIndex(
                name: "ix_powers_level_id",
                table: "power",
                newName: "IX_power_LevelId");

            migrationBuilder.RenameIndex(
                name: "ix_powers_duration_id",
                table: "power",
                newName: "IX_power_DurationId");

            migrationBuilder.RenameIndex(
                name: "ix_powers_area_of_effect_type_id",
                table: "power",
                newName: "IX_power_AreaOfEffectTypeId");

            migrationBuilder.RenameIndex(
                name: "ix_powers_activation_timing_type_id",
                table: "power",
                newName: "IX_power_ActivationTimingTypeId");

            migrationBuilder.RenameIndex(
                name: "ix_power_prerequisites_power_id",
                table: "power_prerequisite",
                newName: "IX_power_prerequisite_power_id");

            migrationBuilder.RenameIndex(
                name: "ix_power_prerequisite_powers_power_id",
                table: "power_prerequisite_power",
                newName: "IX_power_prerequisite_power_power_id");

            migrationBuilder.RenameIndex(
                name: "ix_power_paths_expression_id",
                table: "power_path",
                newName: "IX_power_path_expression_id");

            migrationBuilder.RenameIndex(
                name: "ix_power_path_audit_trails_power_path_id",
                table: "power_path_audit_trail",
                newName: "IX_power_path_audit_trail_power_path_id");

            migrationBuilder.RenameIndex(
                name: "ix_power_path_audit_trails_expression_id",
                table: "power_path_audit_trail",
                newName: "IX_power_path_audit_trail_expression_id");

            migrationBuilder.RenameIndex(
                name: "ix_power_path_audit_trails_actor_user_id",
                table: "power_path_audit_trail",
                newName: "IX_power_path_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "power_duration",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "power_duration",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "power_duration",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "power_category_mapping",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "power_id",
                table: "power_category_mapping",
                newName: "PowerId");

            migrationBuilder.RenameIndex(
                name: "ix_power_category_mappings_category_id",
                table: "power_category_mapping",
                newName: "IX_power_category_mapping_CategoryId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "power_category",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "power_category",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "power_category",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_power_audit_trails_power_path_id",
                table: "power_audit_trail",
                newName: "IX_power_audit_trail_power_path_id");

            migrationBuilder.RenameIndex(
                name: "ix_power_audit_trails_power_id",
                table: "power_audit_trail",
                newName: "IX_power_audit_trail_power_id");

            migrationBuilder.RenameIndex(
                name: "ix_power_audit_trails_actor_user_id",
                table: "power_audit_trail",
                newName: "IX_power_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "power_area_of_effect_type",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "power_area_of_effect_type",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "power_area_of_effect_type",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "power_activation_timing_type",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "power_activation_timing_type",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "power_activation_timing_type",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_knowledges_knowledge_type_id",
                table: "knowledge",
                newName: "IX_knowledge_knowledge_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_knowledge_audit_trails_knowledge_id",
                table: "knowledges_audit_trail",
                newName: "IX_knowledges_audit_trail_knowledge_id");

            migrationBuilder.RenameIndex(
                name: "ix_knowledge_audit_trails_actor_user_id",
                table: "knowledges_audit_trail",
                newName: "IX_knowledges_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "nav_menu_image",
                table: "expression",
                newName: "nav_menu_item");

            migrationBuilder.RenameIndex(
                name: "ix_expressions_publish_status_id",
                table: "expression",
                newName: "IX_expression_publish_status_id");

            migrationBuilder.RenameIndex(
                name: "ix_expressions_expression_type_id",
                table: "expression",
                newName: "IX_expression_expression_type_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "ExpressionSections",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "ExpressionSections",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ExpressionSections",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "section_type_id",
                table: "ExpressionSections",
                newName: "SectionTypeId");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "ExpressionSections",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "order_index",
                table: "ExpressionSections",
                newName: "OrderIndex");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ExpressionSections",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "expression_id",
                table: "ExpressionSections",
                newName: "ExpressionId");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "ExpressionSections",
                newName: "DeletedAt");

            migrationBuilder.RenameIndex(
                name: "ix_expression_sections_section_type_id",
                table: "ExpressionSections",
                newName: "IX_ExpressionSections_SectionTypeId");

            migrationBuilder.RenameIndex(
                name: "ix_expression_sections_parent_id",
                table: "ExpressionSections",
                newName: "IX_ExpressionSections_ParentId");

            migrationBuilder.RenameIndex(
                name: "ix_expression_sections_expression_id",
                table: "ExpressionSections",
                newName: "IX_ExpressionSections_ExpressionId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "ExpressionSectionTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "ExpressionSectionTypes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ExpressionSectionTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "ExpressionSection_AuditTrail",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "ExpressionSection_AuditTrail",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ExpressionSection_AuditTrail",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "section_id",
                table: "ExpressionSection_AuditTrail",
                newName: "SectionId");

            migrationBuilder.RenameColumn(
                name: "expression_id",
                table: "ExpressionSection_AuditTrail",
                newName: "ExpressionId");

            migrationBuilder.RenameColumn(
                name: "changed_properties",
                table: "ExpressionSection_AuditTrail",
                newName: "ChangedProperties");

            migrationBuilder.RenameColumn(
                name: "actor_user_id",
                table: "ExpressionSection_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "ix_expression_section_audit_trails_section_id",
                table: "ExpressionSection_AuditTrail",
                newName: "IX_ExpressionSection_AuditTrail_SectionId");

            migrationBuilder.RenameIndex(
                name: "ix_expression_section_audit_trails_expression_id",
                table: "ExpressionSection_AuditTrail",
                newName: "IX_ExpressionSection_AuditTrail_ExpressionId");

            migrationBuilder.RenameIndex(
                name: "ix_expression_section_audit_trails_actor_user_id",
                table: "ExpressionSection_AuditTrail",
                newName: "IX_ExpressionSection_AuditTrail_ActorUserId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "ExpressionPublishStatus",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "ExpressionPublishStatus",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ExpressionPublishStatus",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Expression_AuditTrail",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "Expression_AuditTrail",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Expression_AuditTrail",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "expression_id",
                table: "Expression_AuditTrail",
                newName: "ExpressionId");

            migrationBuilder.RenameColumn(
                name: "changed_properties",
                table: "Expression_AuditTrail",
                newName: "ChangedProperties");

            migrationBuilder.RenameColumn(
                name: "actor_user_id",
                table: "Expression_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "ix_expression_audit_trails_expression_id",
                table: "Expression_AuditTrail",
                newName: "IX_Expression_AuditTrail_ExpressionId");

            migrationBuilder.RenameIndex(
                name: "ix_expression_audit_trails_actor_user_id",
                table: "Expression_AuditTrail",
                newName: "IX_Expression_AuditTrail_ActorUserId");

            migrationBuilder.RenameIndex(
                name: "ix_event_questions_question_type_id",
                table: "event_question",
                newName: "IX_event_question_question_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_event_questions_event_id",
                table: "event_question",
                newName: "IX_event_question_event_id");

            migrationBuilder.RenameIndex(
                name: "ix_contacts_knowledge_level_id",
                table: "contact",
                newName: "IX_contact_knowledge_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_contacts_knowledge_id",
                table: "contact",
                newName: "IX_contact_knowledge_id");

            migrationBuilder.RenameIndex(
                name: "ix_contacts_character_id",
                table: "contact",
                newName: "IX_contact_character_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkins_player_id",
                table: "checkin",
                newName: "IX_checkin_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkins_event_id",
                table: "checkin",
                newName: "IX_checkin_event_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_stage_mappings_checkin_stage_id",
                table: "checkin_stage_mapping",
                newName: "IX_checkin_stage_mapping_checkin_stage_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_stage_mappings_checkin_id",
                table: "checkin_stage_mapping",
                newName: "IX_checkin_stage_mapping_checkin_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_stage_mappings_approver_user_id",
                table: "checkin_stage_mapping",
                newName: "IX_checkin_stage_mapping_approver_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_responses_event_question_id",
                table: "checkin_question_response",
                newName: "IX_checkin_question_response_event_question_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_response_audit_trail_event_question_id",
                table: "checkin_event_question_response_audit_trail",
                newName: "IX_checkin_event_question_response_audit_trail_event_question_~");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_response_audit_trail_checkin_id_event_ques",
                table: "checkin_event_question_response_audit_trail",
                newName: "IX_checkin_event_question_response_audit_trail_checkin_id_even~");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_response_audit_trail_actor_user_id",
                table: "checkin_event_question_response_audit_trail",
                newName: "IX_checkin_event_question_response_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_audit_trails_checkin_id",
                table: "checkin_audit_trail",
                newName: "IX_checkin_audit_trail_checkin_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_audit_trails_actor_user_id",
                table: "checkin_audit_trail",
                newName: "IX_checkin_audit_trail_actor_user_id");

            migrationBuilder.RenameColumn(
                name: "skill_level_id",
                table: "CharacterSkillsMapping",
                newName: "SkillLevelId");

            migrationBuilder.RenameColumn(
                name: "skill_type_id",
                table: "CharacterSkillsMapping",
                newName: "SkillTypeId");

            migrationBuilder.RenameColumn(
                name: "character_id",
                table: "CharacterSkillsMapping",
                newName: "CharacterId");

            migrationBuilder.RenameIndex(
                name: "ix_character_skills_mappings_skill_type_id",
                table: "CharacterSkillsMapping",
                newName: "IX_CharacterSkillsMapping_SkillTypeId");

            migrationBuilder.RenameIndex(
                name: "ix_character_skills_mappings_skill_level_id",
                table: "CharacterSkillsMapping",
                newName: "IX_CharacterSkillsMapping_SkillLevelId");

            migrationBuilder.RenameIndex(
                name: "ix_character_power_mappings_power_level_id",
                table: "character_power_mapping",
                newName: "IX_character_power_mapping_power_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_power_mappings_power_id",
                table: "character_power_mapping",
                newName: "IX_character_power_mapping_power_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_power_mappings_character_id",
                table: "character_power_mapping",
                newName: "IX_character_power_mapping_character_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_knowledge_specializations_knowledge_mapping_id",
                table: "character_knowledge_specialization",
                newName: "IX_character_knowledge_specialization_knowledge_mapping_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_knowledge_mappings_knowledge_level_id",
                table: "character_knowledge_mapping",
                newName: "IX_character_knowledge_mapping_knowledge_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_knowledge_mappings_knowledge_id",
                table: "character_knowledge_mapping",
                newName: "IX_character_knowledge_mapping_knowledge_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_knowledge_mappings_character_id",
                table: "character_knowledge_mapping",
                newName: "IX_character_knowledge_mapping_character_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_blessing_mappings_character_id",
                table: "character_blessing_mapping",
                newName: "IX_character_blessing_mapping_character_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_blessing_mappings_blessing_level_id",
                table: "character_blessing_mapping",
                newName: "IX_character_blessing_mapping_blessing_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_blessing_mappings_blessing_id",
                table: "character_blessing_mapping",
                newName: "IX_character_blessing_mapping_blessing_id");

            migrationBuilder.RenameColumn(
                name: "stat_modifier_group_id",
                table: "blessing_level",
                newName: "stat_modifier_group");

            migrationBuilder.RenameIndex(
                name: "ix_blessing_levels_stat_modifier_group_id",
                table: "blessing_level",
                newName: "IX_blessing_level_stat_modifier_group");

            migrationBuilder.RenameIndex(
                name: "ix_blessing_levels_blessing_id",
                table: "blessing_level",
                newName: "IX_blessing_level_blessing_id");

            migrationBuilder.RenameIndex(
                name: "ix_blessing_level_audit_trails_blessing_level_id",
                table: "blessing_level_audit_trail",
                newName: "IX_blessing_level_audit_trail_blessing_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_blessing_level_audit_trails_blessing_id",
                table: "blessing_level_audit_trail",
                newName: "IX_blessing_level_audit_trail_blessing_id");

            migrationBuilder.RenameIndex(
                name: "ix_blessing_level_audit_trails_actor_user_id",
                table: "blessing_level_audit_trail",
                newName: "IX_blessing_level_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_blessing_audit_trails_blessing_id",
                table: "blessing_audit_trail",
                newName: "IX_blessing_audit_trail_blessing_id");

            migrationBuilder.RenameIndex(
                name: "ix_blessing_audit_trails_actor_user_id",
                table: "blessing_audit_trail",
                newName: "IX_blessing_audit_trail_actor_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_xp_section_type",
                table: "xp_section_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles_AuditTrail",
                table: "UserRoles_AuditTrail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_role_mapping_audit_trail",
                table: "user_role_mapping_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_role_mapping",
                table: "user_role_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_AuditTrail",
                table: "User_AuditTrail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permission_mapping_audit_trail",
                table: "role_permission_mapping_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permission_mapping",
                table: "role_permission_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_audit_trail",
                table: "role_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role",
                table: "role",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_progression_path_audit_trail",
                table: "progression_path_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_progression_path",
                table: "progression_path",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_progression_level_audit_trail",
                table: "progression_level_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_progression_level",
                table: "progression_level",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player_AuditTrail",
                table: "Player_AuditTrail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission_resource",
                table: "permission_resource",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission",
                table: "permission",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_event_schedule_item_audit_trail",
                table: "event_schedule_item_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_event_question_audit_trail",
                table: "event_question_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_event_audit_trail",
                table: "event_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contact_audit_trail",
                table: "contact_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characters",
                table: "Characters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_character_xp_mapping",
                table: "character_xp_mapping",
                columns: new[] { "character_id", "xp_section_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_assigned_xp_type_audit_trail",
                table: "assigned_xp_type_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_assigned_xp_type",
                table: "assigned_xp_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_assigned_xp_mapping_audit_trail",
                table: "assigned_xp_mapping_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_assigned_xp_mapping",
                table: "assigned_xp_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StateTypes",
                table: "StateTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stat_modifier",
                table: "stat_modifier",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stat_modifier_group",
                table: "stat_modifier_group",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatLevels",
                table: "StatLevels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stat_group_mapping",
                table: "stat_group_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatDescriptionMappings",
                table: "StatDescriptionMappings",
                columns: new[] { "StatTypeId", "StatLevelId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkillType",
                table: "SkillType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkillSubType",
                table: "SkillSubType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_skill_level",
                table: "skill_level",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkillLevelDescriptionMapping",
                table: "SkillLevelDescriptionMapping",
                columns: new[] { "SkillLevelId", "SkillTypeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkillLevelBenefit",
                table: "SkillLevelBenefit",
                columns: new[] { "SkillLevelId", "SkillTypeId", "ModifierTypeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_question_type",
                table: "question_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power",
                table: "power",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_prerequisite",
                table: "power_prerequisite",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_prerequisite_power",
                table: "power_prerequisite_power",
                columns: new[] { "prerequisite_id", "power_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_path",
                table: "power_path",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_path_audit_trail",
                table: "power_path_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_level",
                table: "power_level",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_duration",
                table: "power_duration",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_category_mapping",
                table: "power_category_mapping",
                columns: new[] { "PowerId", "CategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_category",
                table: "power_category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_audit_trail",
                table: "power_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_area_of_effect_type",
                table: "power_area_of_effect_type",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_activation_timing_type",
                table: "power_activation_timing_type",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_modifier_type",
                table: "modifier_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_knowledge",
                table: "knowledge",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_knowledge_type",
                table: "knowledge_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_knowledge_education_level",
                table: "knowledge_education_level",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_knowledges_audit_trail",
                table: "knowledges_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_expression",
                table: "expression",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_expression_type",
                table: "expression_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpressionSections",
                table: "ExpressionSections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpressionSectionTypes",
                table: "ExpressionSectionTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpressionSection_AuditTrail",
                table: "ExpressionSection_AuditTrail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpressionPublishStatus",
                table: "ExpressionPublishStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expression_AuditTrail",
                table: "Expression_AuditTrail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_event",
                table: "event",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_event_schedule_item",
                table: "event_schedule_item",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_event_question",
                table: "event_question",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contact",
                table: "contact",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_checkin",
                table: "checkin",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_checkin_stage",
                table: "checkin_stage",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_checkin_stage_mapping",
                table: "checkin_stage_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_checkin_question_response",
                table: "checkin_question_response",
                columns: new[] { "checkin_id", "event_question_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_checkin_event_question_response_audit_trail",
                table: "checkin_event_question_response_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_checkin_audit_trail",
                table: "checkin_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterSkillsMapping",
                table: "CharacterSkillsMapping",
                columns: new[] { "CharacterId", "SkillTypeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_character_power_mapping",
                table: "character_power_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_character_knowledge_specialization",
                table: "character_knowledge_specialization",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_character_knowledge_mapping",
                table: "character_knowledge_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_character_blessing_mapping",
                table: "character_blessing_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blessing",
                table: "blessing",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blessing_level",
                table: "blessing_level",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blessing_level_audit_trail",
                table: "blessing_level_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blessing_audit_trail",
                table: "blessing_audit_trail",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_assigned_xp_mapping_AspNetUsers_assigned_by_user_id",
                table: "assigned_xp_mapping",
                column: "assigned_by_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assigned_xp_mapping_Characters_character_id",
                table: "assigned_xp_mapping",
                column: "character_id",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assigned_xp_mapping_Players_player_id",
                table: "assigned_xp_mapping",
                column: "player_id",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assigned_xp_mapping_assigned_xp_type_assigned_xp_type_id",
                table: "assigned_xp_mapping",
                column: "assigned_xp_type_id",
                principalTable: "assigned_xp_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assigned_xp_mapping_event_event_id",
                table: "assigned_xp_mapping",
                column: "event_id",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assigned_xp_mapping_audit_trail_AspNetUsers_actor_user_id",
                table: "assigned_xp_mapping_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assigned_xp_mapping_audit_trail_assigned_xp_mapping_assigne~",
                table: "assigned_xp_mapping_audit_trail",
                column: "assigned_xp_mapping_id",
                principalTable: "assigned_xp_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assigned_xp_type_audit_trail_AspNetUsers_actor_user_id",
                table: "assigned_xp_type_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assigned_xp_type_audit_trail_assigned_xp_type_assigned_xp_t~",
                table: "assigned_xp_type_audit_trail",
                column: "assigned_xp_type_id",
                principalTable: "assigned_xp_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_audit_trail_AspNetUsers_actor_user_id",
                table: "blessing_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_audit_trail_blessing_blessing_id",
                table: "blessing_audit_trail",
                column: "blessing_id",
                principalTable: "blessing",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_level_blessing_blessing_id",
                table: "blessing_level",
                column: "blessing_id",
                principalTable: "blessing",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_level_stat_modifier_group_stat_modifier_group",
                table: "blessing_level",
                column: "stat_modifier_group",
                principalTable: "stat_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_level_audit_trail_AspNetUsers_actor_user_id",
                table: "blessing_level_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_level_audit_trail_blessing_blessing_id",
                table: "blessing_level_audit_trail",
                column: "blessing_id",
                principalTable: "blessing",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_level_audit_trail_blessing_level_blessing_level_id",
                table: "blessing_level_audit_trail",
                column: "blessing_level_id",
                principalTable: "blessing_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_blessing_mapping_Characters_character_id",
                table: "character_blessing_mapping",
                column: "character_id",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_blessing_mapping_blessing_blessing_id",
                table: "character_blessing_mapping",
                column: "blessing_id",
                principalTable: "blessing",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_blessing_mapping_blessing_level_blessing_level_id",
                table: "character_blessing_mapping",
                column: "blessing_level_id",
                principalTable: "blessing_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_knowledge_mapping_Characters_character_id",
                table: "character_knowledge_mapping",
                column: "character_id",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_knowledge_mapping_knowledge_education_level_knowl~",
                table: "character_knowledge_mapping",
                column: "knowledge_level_id",
                principalTable: "knowledge_education_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_knowledge_mapping_knowledge_knowledge_id",
                table: "character_knowledge_mapping",
                column: "knowledge_id",
                principalTable: "knowledge",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_knowledge_specialization_character_knowledge_mapp~",
                table: "character_knowledge_specialization",
                column: "knowledge_mapping_id",
                principalTable: "character_knowledge_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_power_mapping_Characters_character_id",
                table: "character_power_mapping",
                column: "character_id",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_power_mapping_power_level_power_level_id",
                table: "character_power_mapping",
                column: "power_level_id",
                principalTable: "power_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_power_mapping_power_power_id",
                table: "character_power_mapping",
                column: "power_id",
                principalTable: "power",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_xp_mapping_Characters_character_id",
                table: "character_xp_mapping",
                column: "character_id",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_character_xp_mapping_xp_section_type_xp_section_type_id",
                table: "character_xp_mapping",
                column: "xp_section_type_id",
                principalTable: "xp_section_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ExpressionSections_FactionId",
                table: "Characters",
                column: "FactionId",
                principalTable: "ExpressionSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Players_PlayerId",
                table: "Characters",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_AgilityId",
                table: "Characters",
                column: "AgilityId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_ConstitutionId",
                table: "Characters",
                column: "ConstitutionId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_DexterityId",
                table: "Characters",
                column: "DexterityId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_IntelligenceId",
                table: "Characters",
                column: "IntelligenceId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_StrengthId",
                table: "Characters",
                column: "StrengthId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_WillpowerId",
                table: "Characters",
                column: "WillpowerId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_expression_ExpressionId",
                table: "Characters",
                column: "ExpressionId",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_progression_path_primary_progression_id",
                table: "Characters",
                column: "primary_progression_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_progression_path_secondary_progression_id",
                table: "Characters",
                column: "secondary_progression_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterSkillsMapping_Characters_CharacterId",
                table: "CharacterSkillsMapping",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterSkillsMapping_SkillType_SkillTypeId",
                table: "CharacterSkillsMapping",
                column: "SkillTypeId",
                principalTable: "SkillType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterSkillsMapping_skill_level_SkillLevelId",
                table: "CharacterSkillsMapping",
                column: "SkillLevelId",
                principalTable: "skill_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_Players_player_id",
                table: "checkin",
                column: "player_id",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_event_event_id",
                table: "checkin",
                column: "event_id",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_audit_trail_AspNetUsers_actor_user_id",
                table: "checkin_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_audit_trail_checkin_checkin_id",
                table: "checkin_audit_trail",
                column: "checkin_id",
                principalTable: "checkin",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_AspNetUsers_act~",
                table: "checkin_event_question_response_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_checkin_checkin~",
                table: "checkin_event_question_response_audit_trail",
                column: "checkin_id",
                principalTable: "checkin",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_checkin_questio~",
                table: "checkin_event_question_response_audit_trail",
                columns: new[] { "checkin_id", "event_question_id" },
                principalTable: "checkin_question_response",
                principalColumns: new[] { "checkin_id", "event_question_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_event_question_~",
                table: "checkin_event_question_response_audit_trail",
                column: "event_question_id",
                principalTable: "event_question",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_question_response_checkin_checkin_id",
                table: "checkin_question_response",
                column: "checkin_id",
                principalTable: "checkin",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_question_response_event_question_event_question_id",
                table: "checkin_question_response",
                column: "event_question_id",
                principalTable: "event_question",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_stage_mapping_AspNetUsers_approver_user_id",
                table: "checkin_stage_mapping",
                column: "approver_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_stage_mapping_checkin_checkin_id",
                table: "checkin_stage_mapping",
                column: "checkin_id",
                principalTable: "checkin",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_stage_mapping_checkin_stage_checkin_stage_id",
                table: "checkin_stage_mapping",
                column: "checkin_stage_id",
                principalTable: "checkin_stage",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contact_Characters_character_id",
                table: "contact",
                column: "character_id",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contact_knowledge_education_level_knowledge_level_id",
                table: "contact",
                column: "knowledge_level_id",
                principalTable: "knowledge_education_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contact_knowledge_knowledge_id",
                table: "contact",
                column: "knowledge_id",
                principalTable: "knowledge",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contact_audit_trail_AspNetUsers_actor_user_id",
                table: "contact_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contact_audit_trail_contact_contact_id",
                table: "contact_audit_trail",
                column: "contact_id",
                principalTable: "contact",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_event_audit_trail_AspNetUsers_actor_user_id",
                table: "event_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_event_audit_trail_event_event_id",
                table: "event_audit_trail",
                column: "event_id",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_event_question_event_event_id",
                table: "event_question",
                column: "event_id",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_event_question_question_type_question_type_id",
                table: "event_question",
                column: "question_type_id",
                principalTable: "question_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_event_question_audit_trail_AspNetUsers_actor_user_id",
                table: "event_question_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_event_question_audit_trail_event_question_event_question_id",
                table: "event_question_audit_trail",
                column: "event_question_id",
                principalTable: "event_question",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_event_schedule_item_audit_trail_AspNetUsers_actor_user_id",
                table: "event_schedule_item_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_event_schedule_item_audit_trail_event_EventId",
                table: "event_schedule_item_audit_trail",
                column: "EventId",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_event_schedule_item_audit_trail_event_schedule_item_event_s~",
                table: "event_schedule_item_audit_trail",
                column: "event_schedule_item_id",
                principalTable: "event_schedule_item",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_expression_ExpressionPublishStatus_publish_status_id",
                table: "expression",
                column: "publish_status_id",
                principalTable: "ExpressionPublishStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_expression_expression_type_expression_type_id",
                table: "expression",
                column: "expression_type_id",
                principalTable: "expression_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_AuditTrail_AspNetUsers_ActorUserId",
                table: "Expression_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_AuditTrail_expression_ExpressionId",
                table: "Expression_AuditTrail",
                column: "ExpressionId",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_ActorUserId",
                table: "ExpressionSection_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_AuditTrail_ExpressionSections_SectionId",
                table: "ExpressionSection_AuditTrail",
                column: "SectionId",
                principalTable: "ExpressionSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_AuditTrail_expression_ExpressionId",
                table: "ExpressionSection_AuditTrail",
                column: "ExpressionId",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSections_ExpressionSectionTypes_SectionTypeId",
                table: "ExpressionSections",
                column: "SectionTypeId",
                principalTable: "ExpressionSectionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSections_ExpressionSections_ParentId",
                table: "ExpressionSections",
                column: "ParentId",
                principalTable: "ExpressionSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSections_expression_ExpressionId",
                table: "ExpressionSections",
                column: "ExpressionId",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_knowledge_knowledge_type_knowledge_type_id",
                table: "knowledge",
                column: "knowledge_type_id",
                principalTable: "knowledge_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_knowledges_audit_trail_AspNetUsers_actor_user_id",
                table: "knowledges_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_knowledges_audit_trail_knowledge_knowledge_id",
                table: "knowledges_audit_trail",
                column: "knowledge_id",
                principalTable: "knowledge",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_permission_permission_resource_permission_resource_id",
                table: "permission",
                column: "permission_resource_id",
                principalTable: "permission_resource",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_AuditTrail_AspNetUsers_ActorUserId",
                table: "Player_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_AuditTrail_Players_PlayerId",
                table: "Player_AuditTrail",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_AspNetUsers_UserId",
                table: "Players",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_activation_timing_type_ActivationTimingTypeId",
                table: "power",
                column: "ActivationTimingTypeId",
                principalTable: "power_activation_timing_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_area_of_effect_type_AreaOfEffectTypeId",
                table: "power",
                column: "AreaOfEffectTypeId",
                principalTable: "power_area_of_effect_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_duration_DurationId",
                table: "power",
                column: "DurationId",
                principalTable: "power_duration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_level_LevelId",
                table: "power",
                column: "LevelId",
                principalTable: "power_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_path_power_path_id",
                table: "power",
                column: "power_path_id",
                principalTable: "power_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_stat_modifier_group_stat_modifier_group",
                table: "power",
                column: "stat_modifier_group",
                principalTable: "stat_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_audit_trail_AspNetUsers_actor_user_id",
                table: "power_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_audit_trail_power_path_power_path_id",
                table: "power_audit_trail",
                column: "power_path_id",
                principalTable: "power_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_audit_trail_power_power_id",
                table: "power_audit_trail",
                column: "power_id",
                principalTable: "power",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_category_mapping_power_PowerId",
                table: "power_category_mapping",
                column: "PowerId",
                principalTable: "power",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_category_mapping_power_category_CategoryId",
                table: "power_category_mapping",
                column: "CategoryId",
                principalTable: "power_category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_expression_expression_id",
                table: "power_path",
                column: "expression_id",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_audit_trail_AspNetUsers_actor_user_id",
                table: "power_path_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_audit_trail_expression_expression_id",
                table: "power_path_audit_trail",
                column: "expression_id",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_audit_trail_power_path_power_path_id",
                table: "power_path_audit_trail",
                column: "power_path_id",
                principalTable: "power_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_prerequisite_power_power_id",
                table: "power_prerequisite",
                column: "power_id",
                principalTable: "power",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_prerequisite_power_power_power_id",
                table: "power_prerequisite_power",
                column: "power_id",
                principalTable: "power",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_prerequisite_power_power_prerequisite_prerequisite_id",
                table: "power_prerequisite_power",
                column: "prerequisite_id",
                principalTable: "power_prerequisite",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_level_progression_path_progression_path_id",
                table: "progression_level",
                column: "progression_path_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_level_stat_modifier_group_stat_modifier_group",
                table: "progression_level",
                column: "stat_modifier_group",
                principalTable: "stat_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_level_audit_trail_AspNetUsers_actor_user_id",
                table: "progression_level_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_level_audit_trail_progression_level_progression~",
                table: "progression_level_audit_trail",
                column: "progression_level_id",
                principalTable: "progression_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_level_audit_trail_progression_path_progression_~",
                table: "progression_level_audit_trail",
                column: "progression_path_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_path_expression_expression_id",
                table: "progression_path",
                column: "expression_id",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_path_audit_trail_AspNetUsers_actor_user_id",
                table: "progression_path_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_path_audit_trail_expression_expression_id",
                table: "progression_path_audit_trail",
                column: "expression_id",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_path_audit_trail_progression_path_progression_p~",
                table: "progression_path_audit_trail",
                column: "progression_path_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_role_audit_trail_AspNetUsers_actor_user_id",
                table: "role_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_role_audit_trail_role_role_id",
                table: "role_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_permission_permission_id",
                table: "role_permission_mapping",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_role_role_id",
                table: "role_permission_mapping",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_AspNetUsers_actor_user_~",
                table: "role_permission_mapping_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail",
                column: "role_permission_mapping_id",
                principalTable: "role_permission_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelBenefit_SkillType_SkillTypeId",
                table: "SkillLevelBenefit",
                column: "SkillTypeId",
                principalTable: "SkillType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelBenefit_modifier_type_ModifierTypeId",
                table: "SkillLevelBenefit",
                column: "ModifierTypeId",
                principalTable: "modifier_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelBenefit_skill_level_SkillLevelId",
                table: "SkillLevelBenefit",
                column: "SkillLevelId",
                principalTable: "skill_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelDescriptionMapping_SkillType_SkillTypeId",
                table: "SkillLevelDescriptionMapping",
                column: "SkillTypeId",
                principalTable: "SkillType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelDescriptionMapping_skill_level_SkillLevelId",
                table: "SkillLevelDescriptionMapping",
                column: "SkillLevelId",
                principalTable: "skill_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillType_SkillSubType_SkillSubTypeId",
                table: "SkillType",
                column: "SkillSubTypeId",
                principalTable: "SkillSubType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_stat_group_mapping_expression_target_expression_id",
                table: "stat_group_mapping",
                column: "target_expression_id",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_stat_group_mapping_stat_modifier_group_stat_group_id",
                table: "stat_group_mapping",
                column: "stat_group_id",
                principalTable: "stat_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_stat_group_mapping_stat_modifier_stat_modifier_id",
                table: "stat_group_mapping",
                column: "stat_modifier_id",
                principalTable: "stat_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StatDescriptionMappings_StatLevels_StatLevelId",
                table: "StatDescriptionMappings",
                column: "StatLevelId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StatDescriptionMappings_StateTypes_StatTypeId",
                table: "StatDescriptionMappings",
                column: "StatTypeId",
                principalTable: "StateTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_ActorUserId",
                table: "User_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_UserId",
                table: "User_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_mapping_AspNetUsers_user_id",
                table: "user_role_mapping",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_mapping_role_role_id",
                table: "user_role_mapping",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_mapping_audit_trail_AspNetUsers_actor_user_id",
                table: "user_role_mapping_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_mapping_audit_trail_AspNetUsers_user_id",
                table: "user_role_mapping_audit_trail",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_mapping_audit_trail_role_role_id",
                table: "user_role_mapping_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_mapping_audit_trail_user_role_mapping_user_role_m~",
                table: "user_role_mapping_audit_trail",
                column: "user_role_mapping_id",
                principalTable: "user_role_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetRoles_RoleId",
                table: "UserRoles_AuditTrail",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetUsers_ActorUserId",
                table: "UserRoles_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetUsers_MappingUserId",
                table: "UserRoles_AuditTrail",
                column: "MappingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
