# Adding New CMS Sections

## Database

You need to add the following in a migration, this will get you the new section information.
Basically each CMS will have it's dedicated expression type, and expression. Below adds them all in

```csharp
    migrationBuilder.InsertData(
        table: "expression_type",
        columns: new[] { "id", "name", "description" },
        values: new object[,]
        {
            { 4, "Adversaries", "lorem ipsum",  },
            { 5, "Factions", "lorem ipsum",  },
            { 6, "The Society", "lorem ipsum",  },
            { 7, "Character Setup", "lorem ipsum",  },
            { 8, "Knowledges", "lorem ipsum",  },
            { 9, "Advantage / Disadvantage / Mixed Blessings", "lorem ipsum",  },
            { 10, "Combat", "lorem ipsum",  },
        });
    
    migrationBuilder.InsertData(
        table: "expression",
        columns: new[] { "name", "short_description", "nav_menu_item", "publish_status_id", "expression_type_id" },
        values: new object[,]
        {
            { "Adversaries", "All of the rules", "pi-prime", 1, 4 },
            { "Factions", "All the stories", "pi-prime", 1, 5 },
            { "The Society", "All the stories", "pi-prime", 1, 6 },
            { "Character Setup", "All the stories", "pi-prime", 1, 7 },
            { "Knowledges", "All the stories", "pi-prime", 1, 8 },
            { "Advantage / Disadvantage / Mixed Blessings", "All the stories", "pi-prime", 1, 9 },
            { "Combat", "All the stories", "pi-prime", 1, 10 },
        });

```

## Backend

There's nothing that needs to be done here, it's been generalized so you don't need to modify this after adding a new one

## Frontend

The dynamic nature is driven by the vue router meta data, you will need to add something like below to get it to properly
connect to the backend.

The import bit is the isCMS and id, where id is the expression type id as set above

```json
{
    path: "/blessings",
    name: "blessings",
    component: () => import("./../../components/expressions/CmsBase.vue"),
    meta: { isCMS: true, id: 9 },
},

```

# Creating Dynamic Sections

A dynamic section is one that shows up in the list, but has additional functionality beyond the standard title / description
mix.  The two existing ones as of writing are the Knowledge list and Blessings list.

## Codeside

There's a few pieces to this, most of which I don't remember off the top of my head as of writing.  Will add when I need
to do this again.

## Database

So, in addition to adding the new component, you will also need to run something like this in the database to make sure
that the section shows up appropriately.

```postgresql
DO $$
DECLARE
    expression_id INTEGER;
    section_type_id INTEGER;
    max_sort_order INTEGER; 
BEGIN
    -- Get the ID and store it in a variable
    SELECT id INTO expression_id 
    FROM public.expression 
    WHERE expression.expression_type_id = 9 
    LIMIT 1; -- Add LIMIT if you expect only one result

    -- Insert section type if it doesn't exist
    insert into public."ExpressionSectionTypes" ("Name", "Description")
    select 'Advantage / Disadvantage / Mixed Blessings', 'Placeholder for the Advantange / Disadvantage / Mixed Blessings section'
    WHERE NOT EXISTS (
        SELECT 1 FROM public."ExpressionSectionTypes" 
        WHERE "Name" = 'Advantage / Disadvantage / Mixed Blessings'
    );

    -- Get the section type id
    SELECT "Id" INTO section_type_id 
    FROM public."ExpressionSectionTypes" 
    WHERE "ExpressionSectionTypes"."Name" = 'Blessings Section'
    LIMIT 1; -- Add LIMIT if you expect only one result

    -- Get next order id for the given section
    SELECT COALESCE(MAX("OrderIndex"), 0) + 1 INTO max_sort_order 
    FROM public."ExpressionSections" 
    WHERE "ExpressionSections"."ParentId" is null and "ExpressionSections"."ExpressionId" = expression_id;

    -- Use RAISE NOTICE to display values (can't use SELECT in DO block)
    RAISE NOTICE 'expression_id: %, section_type_id: %, max_sort_order: %', 
        expression_id, section_type_id, max_sort_order;
    
    -- Insert if it doesn't exist
    INSERT INTO public."ExpressionSections" ("ExpressionId", "SectionTypeId", "Name", "Content", "OrderIndex")
    select expression_id, section_type_id, 'Blessings Section', '', max_sort_order
    WHERE NOT EXISTS (
        SELECT 1 FROM public."ExpressionSections" 
        WHERE "Name" = 'Blessings Section' and "ExpressionId" = expression_id and "SectionTypeId" = section_type_id
    );
    
END $$;

```
