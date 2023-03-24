using cultured.server.Infrasructure.Models.Helpers;
using cultured.server.Infrastructure.Models.Helpers;
using cultured.server.Infrastructure.Models.Culture;
using cultured.server.Infrastructure.Data.Repo.Culture;
using cultured.server.Infrastructure.Data.Interface.Culture;

namespace cultured.server.Infrastructure.Managers.Culture
{
    public class CultureManager : AppSettings, ICulture
    {
        private readonly ICulture _repo;
        public CultureManager()
        {
            _repo = new CultureRepository();
        }

        public async Task<bool> DeleteCharacter(int ID)
        {
            return await _repo.DeleteCharacter(ID);
        }

        public async Task<IEnumerable<Character>> CharactersByCategory(string category)
        {
            return await _repo.CharactersByCategory(category);
        }

        public async Task<FilteredList<Character>> FilterCharacters(Filter filter)
        {
            return await _repo.FilterCharacters(filter);
        }

        public async Task<IEnumerable<Category>> GetCategory()
        {
            return await _repo.GetCategory();
        }

        public async Task<IEnumerable<Category>> ManageCategory(Category model)
        {
            return await _repo.ManageCategory(model);
        }

        public async Task<Character> ManageCharacter(Character model)
        {
            return await _repo.ManageCharacter(model);
        }
    }
}
