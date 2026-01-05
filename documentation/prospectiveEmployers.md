# Prospective Employers

Hello!

This is here to highlight a few different things regarding the project that might be of interest to people involved in
the hiring process.

There are two different areas I wanted to cover: Communication / Writing and Coding Examples.

To start out with, some basic info:

## Numbers
- App consists of 70+ tables, ~150 API endpoints, 600+ unit tests, and 44,000+ lines of code
- Launched to ~40 active players during a live event, maintaining <100ms average API response times under gameplay load.

## Communication / Writing
I have some tickets down below that go in depth on aspects of the system that need to be implemented at some point, or
have been implemented.  When I'm responsible for drawing up specs, this is how I tend to write:

### Ticket Writing Style, FRD's, BRD's
- [Character XP Tracking System](https://github.com/Society-In-Shadow/ExpressedRealms/issues/803)
- [Event Checking System](https://github.com/Society-In-Shadow/ExpressedRealms/issues/799)
- [Character Tracking System](https://github.com/Society-In-Shadow/ExpressedRealms/issues/798)

### Technical Documentation
I do like writing technical documentation, I think it's important for a team to be on the same page when it comes to
various aspects of the codebase.  Everything I have so far can be found in the documentation folder.  It's by no means
comprehensive, but I am starting to make an effort to update or add as I go.

- [Architecture](./architecture.md) is a good place to start, has links to most of the other documentation

## Coding Examples


### Complex Use Case Example
Use cases are where I put all the domain logic, it's a domain driven design concept.

- [Update Blessings For Character Use Case](./../api/ExpressedRealms.Blessings.UseCases/CharacterBlessingMappings/Edit/UpdateBlessingForCharacterUseCase.cs)
- [Add Blessings To Character Use Case](./../api/ExpressedRealms.Blessings.UseCases/CharacterBlessingMappings/Create/AddBlessingToCharacterUseCase.cs)

### API Example
The newer API endpoints tend to follow the same pattern, I have a Endpoints class that connects all the endpoints together.

- [Character Blessing Endpoints](./../api/ExpressedRealms.Blessings.API/BlessingLevels/BlessingLevelEndpoints.cs)
- [Create Blessing Mapping Endpoint](./../api/ExpressedRealms.Blessings.API/BlessingLevels/EditBlessingLevel/EditBlessingLevelEndpoint.cs)

### Unit Testing
- [Add Blessing To Character Use Case Tests](./../api/ExpressedRealms.Blessings.UseCases.Tests.Unit/CharacterBlessingMappings/AddBlessingToCharacterUseCaseTests.cs)

### UI Examples
Following the same Blessing path here, you can take a look at the UI for it below

- [Blessing UI Structure](./../client/src/components/blessings)

### SQL Example
One of the core parts of a character is keeping track of their XP, both during character creation, and during gameplay.
The following view was thrown together to handle gathering the 6 or so different XP types.

- [Character XP View](./../api/ExpressedRealms.DB/Migrations/20250921054147_UpdateCharacterXpView.cs)