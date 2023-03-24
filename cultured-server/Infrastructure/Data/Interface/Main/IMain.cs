using cultured.server.Infrastructure.Models.Main;
using cultured.server.Infrasructure.Models.Helpers;
using cultured.server.Infrastructure.Models.Culture;

namespace cultured.server.Infrastructure.Data.Interface.Culture
{
    public interface IMain
    {
        Task<Backgrounds> ManageBackground(Backgrounds model);
        Task<FilteredList<Backgrounds>> FilterBackgrounds(Filter filter);
        Task<bool> DeleteBackground(int ID);
        Task<string> GetBackgroundImage();
    }
}
