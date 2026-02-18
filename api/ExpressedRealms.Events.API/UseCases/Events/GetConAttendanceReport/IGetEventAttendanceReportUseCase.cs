using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.GetConAttendanceReport;

public interface IGetEventAttendanceReportUseCase : IGenericUseCase<Result<MemoryStream>, GetConAttendanceReportModel> { }
