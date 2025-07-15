using Wpm.Clinic.Controllers;
using Wpm.Clinic.DataAccess;
using Wpm.Clinic.ExternalServices;

namespace Wpm.Clinic.Application;

public class ClinicApplicationService(ClinicDbContext dbContext, ManagementService service)
{

    public async Task<Consulation> Handle(StartConsulationCommand cmd)
    {
        var petinfo = await service.GetPetInfoAsync(cmd.PatientId);
        
        var newConsulation = new Consulation(Guid.NewGuid(), cmd.PatientId,petinfo.Name,petinfo.Age,DateTime.UtcNow);
        
        await dbContext.Consulations.AddAsync(newConsulation);
        await dbContext.SaveChangesAsync();
        return newConsulation;
    }
}
