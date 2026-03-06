# API Design

## Summary Endpoints
When one needs to get a list of objects for assigning, so basic info such as an id, name and description, they should
use the GenericListDto<T> class.  It's in the shared folder.

The only purpose of this endpoint is to return just this.

Correspondingly, the use case will be simple pass through of this object from the repo, through the use case, and finally
to the endpoint.

It's located in the Shared Repository.

A good example can be found in [GetRoleSummaryUseCase](ExpressedRealms.Admin.UseCases/Roles/GetRoleSummary/GetRoleSummeryUseCase.cs)