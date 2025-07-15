namespace Wpm.Clinic.ExternalServices;

public class ManagementService(HttpClient client)
{
    public async Task<PetInfo> GetPetInfoAsync(int Id) 
    {
        var petinfo = await client.GetFromJsonAsync<PetInfo>($"api/pets/{Id}");
        return petinfo;
    }
}

public record PetInfo(int Id, string Name, int Age,int BreedId);
