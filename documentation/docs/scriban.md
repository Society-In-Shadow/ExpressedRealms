# Scriban
Scriban is a templating nuget package that we use for generating basic crud operations. It handles creating the api, use
cases, repository, and unit tests for a given entity.

You can generally run it by doing this

```bash
dotnet run ./../ExpressedRealms.Scaffolder/ -- tests Faction Factions -props "Background:string:required,ExpressionId:int"
```

Where:

- `Faction` is the name of the entity you want to generate crud operations for.
- `Factions` is the plural name of the entity.
- `-props` is used to specify the properties of the entity.
- `"Background:string:required,ExpressionId:int"` is an example of how to specify properties. It specifies that the 
entity has a property called `Background` of type `string` and is required, and a property called `ExpressionId` 
of type `int`.