using Dapper;
using cultured.server.Infrasructure.Models.Helpers;
using cultured.server.Infrastructure.Models.Helpers;
using cultured.server.Infrastructure.Models.Culture;
using cultured.server.Infrastructure.Data.Interface.Culture;

namespace cultured.server.Infrastructure.Data.Repo.Culture
{
    public class CultureRepository : AppSettings, ICulture
    {
        public async Task<bool> DeleteCharacter(int ID)
        {
            try
            {
                string query = $@"
                DELETE FROM character t WHERE t.id = {ID};
                DELETE FROM character_images t WHERE t.characterid = {ID};";

                using (var connection = GetConnection)
                {
                    var res = await connection.ExecuteAsync(query);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Character>> CharactersByCategory(string category)
        {
            try
            {
                string WhereClause = $@" WHERE t.categoryid in (SELECT id FROM category 
                  WHERE name ILIKE '%{category}%')";

                string query = $@"
                    SELECT * FROM character t
                    {WhereClause}
                    ORDER BY t.id ASC";

                using (var con = GetConnection)
                {
                    var data = await con.QueryAsync<Character>(query);
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<FilteredList<Character>> FilterCharacters(Filter filter)
        {
            try
            {
                var filterModel = new Character();
                filter.pageSize = 20;
                FilteredList<Character> request = new FilteredList<Character>()
                {
                    filter = filter,
                    filterModel = filterModel,
                };
                FilteredList<Character> result = new FilteredList<Character>();
                string WhereClause = "";
                if (filter.Keyword != null && filter.Keyword != "")
                {
                    WhereClause = $@" WHERE name ILIKE '%{filter.Keyword}%'";
                }
                if (filter.CategoryName != null && filter.CategoryName != "")
                {
                    WhereClause = $@" WHERE t.categoryid in (SELECT id FROM category 
                    WHERE name ILIKE '%{filter.CategoryName}%')
                    AND t.name ILIKE '%{filter.Keyword}%'";
                }
                if (filter.CategoryID > 0)
                {
                    WhereClause = $@"WHERE t.categoryid = {filter.CategoryID}
                    AND t.name ILIKE '%{filter.Keyword}%'";
                }

                string query_count = $@"Select Count(t.id) from character t {WhereClause}";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    string query = $@"
                    SELECT * FROM character t
                    {WhereClause}
                    ORDER BY t.id DESC 
                    OFFSET {request.filter.pager.StartIndex} ROWS
                    FETCH NEXT {request.filter.pageSize} ROWS ONLY";
                    result.data = await con.QueryAsync<Character>(query);
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Category>> GetCategory()
        {
            try
            {
                string query = $@"SELECT * FROM category";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryAsync<Category>(query);
                    return res;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Category>> ManageCategory(Category model)
        {
            try
            {
                dynamic identity = model.ID.HasValue ? model.ID.Value : "default";

                string query = $@"
                INSERT INTO category (id, name)
	 	                VALUES ({identity}, '{model.Name}')
                ON CONFLICT (id) DO UPDATE 
                SET Name = '{model.Name}',
                       parentid = {model.ParentID};
                SELECT * FROM category c ";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryAsync<Category>(query);
                    return res;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Character> ManageCharacter(Character model)
        {
            try
            {
                dynamic identity = model.ID.HasValue ? model.ID.Value : "default";

                if (model.Name.Contains("'"))
                {
                    model.Name = model.Name.Replace("'", "''");
                }

                string query = $@"
                INSERT INTO character (id, name, body, categoryid, alt)
	 	                VALUES ({identity}, '{model.Name}', '{model.Body}', '{model.Category.ID}', '{model.Alt}')
                ON CONFLICT (id) DO UPDATE 
                SET name = '{model.Name}',
                       body = '{model.Body}',
                       categoryid = '{model.Category.ID}',
                       alt = '{model.Alt}'
                RETURNING *";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryFirstOrDefaultAsync<Character>(query);
                    if (res.Images.Count > 0)
                    {
                        res.Images = await ManageImages(res.Images, res.ID.Value);
                    }
                    return res;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Image>> ManageImages(List<Image> model, int ID)
        {
            try
            {
                using (var connection = GetConnection)
                {
                    List<Image> fmodel = new List<Image>();
                    string dquery = $@"
                    DELETE FROM character_images t WHERE t.characterid = {ID};";
                    await connection.ExecuteAsync(dquery);
                    foreach (var item in model)
                    {
                        dynamic identity = item.ID.HasValue ? item.ID.Value : "default";
                        string query = $@"
                        INSERT INTO character_images (id, characterid, imageurl)
	 	                VALUES ({identity}, '{item.CharacterID}', '{item.ImageURL}')
                        ON CONFLICT (id) DO UPDATE 
                        SET characterid = '{item.CharacterID}',
                               imageurl = '{item.ImageURL}'
                        RETURNING *";
                        var res = await connection.QueryFirstOrDefaultAsync<Image>(query);
                        fmodel.Add(res);
                    }
                    return fmodel;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
