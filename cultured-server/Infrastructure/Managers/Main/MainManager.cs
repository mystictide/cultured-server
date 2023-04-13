using cultured.server.Infrastructure.Models.Main;
using cultured.server.Infrasructure.Models.Helpers;
using cultured.server.Infrastructure.Data.Repo.Main;
using cultured.server.Infrastructure.Models.Helpers;
using cultured.server.Infrastructure.Models.Culture;
using cultured.server.Infrastructure.Data.Interface.Culture;

namespace cultured.server.Infrastructure.Managers.Main
{
    public class MainManager : AppSettings, IMain
    {
        private readonly IMain _repo;
        public MainManager()
        {
            _repo = new MainRepository();
        }

        public async Task<bool> DeleteBackground(int ID)
        {
            return await _repo.DeleteBackground(ID);
        }

        public async Task<IEnumerable<Category>> GetCategory(bool? main, int? parentid)
        {
            return await _repo.GetCategory(main, parentid);
        }
        public async Task<FilteredList<Backgrounds>> FilterBackgrounds(Filter filter)
        {
            return await _repo.FilterBackgrounds(filter);
        }

        public async Task<string> GetBackgroundImage()
        {
            return await _repo.GetBackgroundImage();
        }

        public async Task<Character> GetCharacter(int? ID, string? Name)
        {
            return await _repo.GetCharacter(ID, Name);
        }

        public async Task<Backgrounds> ManageBackground(Backgrounds model)
        {
            return await _repo.ManageBackground(model);
        }
    }
}
