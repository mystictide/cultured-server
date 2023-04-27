using cultured.server.Infrasructure.Models.Helpers;
using cultured.server.Infrastructure.Models.Culture;

namespace cultured.server.Infrastructure.Data.Interface.Culture
{
    public interface ICulture
    {
        Task<Character> ManageCharacter(Character model);
        Task<bool> DeleteCharacter(int ID);
        Task<bool> DeleteCategory(int ID);
        Task<IEnumerable<Category>> ManageCategory(Category model);
        Task<FilteredList<Character>> FilterCharacters(Filter filter);
        Task<IEnumerable<Character>> CharactersByCategory(string category);
    }
}
