# Scriban
Scriban is a templating nuget package that we use for generating basic crud operations. It handles creating the api, use
cases, repository, and unit tests for a given entity.

You can generally run it by doing this

## Initial Setup
To get this tool up and running, you will need to install the tool globally.

To do this, navigate to the scriban directory and run the following command:

```bash
./installUpdateTool.sh
```

## Usage
Detailed usage of the tool can be found in the help section once installed

```bash
scaffold --help
```

## Example
Here's an example on how to create the tests for all use cases:

```bash
scaffold tests Faction Factions -props "Name:string:required:max.250,Background:string:required:max.20000,ExpressionId:int"
```

Where:

- `Faction` is the name of the entity you want to generate crud operations for.
- `Factions` is the plural name of the entity.
- `-props` is used to specify the properties of the entity.
- `"Background:string:required,ExpressionId:int"` is an example of how to specify properties. It specifies that the 
entity has a property called `Background` of type `string` and is required, and a property called `ExpressionId` 
of type `int`.
    - "Background" - This is the name of the property to be used throughout the backend
    - "string" - This is the type of the property
    - "required" - This is a flag that specifies that the property is required - can be in any order after type
    - "min.10" - This is a flag that specifies the minimum length of the string property. Number after period is the value.
    - "max.20" - This is a flag that specifies the maximum length of the string property. Number after the period is the value.

Things to add
 - Identify the Primary Id's, for things such as edit, get item, and delete
