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
                filter.pageSize = 16;
                FilteredList<Character> request = new FilteredList<Character>()
                {
                    filter = filter,
                    filterModel = filterModel,
                };
                FilteredList<Character> result = new FilteredList<Character>();
                string WhereClause = "";
                string recursive = "";
                if (filter.Keyword != null && filter.Keyword != "")
                {
                    WhereClause = $@" WHERE name ILIKE '%{filter.Keyword}%'";
                }
                if (filter.CategoryName != null && filter.CategoryName != "")
                {
                    WhereClause = $@" WHERE t.categoryid in (SELECT id from cat)
                    AND t.name ILIKE '%{filter.Keyword}%'";
                    recursive = $@"
                    with recursive cat as (
                          select * from category c 
                          where c.name ILIKE '%{filter.CategoryName}%' union
                          select category.* from category 
                          join cat on cat.id = category.parentid)";
                }
                if (filter.CategoryID > 0)
                {
                    WhereClause = $@"WHERE t.categoryid in (SELECT id from cat)
                    AND t.name ILIKE '%{filter.Keyword}%'";
                    recursive = $@"
                    with recursive cat as (
                          select * from category c 
                          where c.id = {filter.CategoryID} union
                          select category.* from category 
                          join cat on cat.id = category.parentid)";
                }

                string query_count = $@"{recursive} Select Count(t.id) from character t {WhereClause}";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    string query = $@"
                    {recursive}
                    SELECT * FROM character t
                    left join category c on c.id = t.categoryid
                    {WhereClause}
                    ORDER BY t.id ASC
                    OFFSET {request.filter.pager.StartIndex} ROWS
                    FETCH NEXT {request.filter.pageSize} ROWS ONLY";
                    result.data = await con.QueryAsync<Character, Category, Character>(query, (chr, cat) =>
                    {
                        chr.Category = cat;
                        return chr;
                    },
    splitOn: "id");
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

        public async Task<IEnumerable<Category>> ManageCategory(Category model)
        {
            try
            {
                dynamic identity = model.ID.HasValue ? model.ID.Value : "default";

                if (model.Description.Contains("'"))
                {
                    model.Description = model.Description.Replace("'", "''");
                }

                string query = $@"
                INSERT INTO category (id, name, imageurl, description, parentid)
	 	                VALUES ({identity}, '{model.Name}', '{model.ImageURL}', '{model.Description}', NULLIF('{model.ParentID}', '')::integer)
                ON CONFLICT (id) DO UPDATE 
                SET name = '{model.Name}',
                       imageurl = '{model.ImageURL}',
                       description = '{model.Description}',
                       parentid = NULLIF('{model.ParentID}', '')::integer;
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
                if (model.Body.Contains("'"))
                {
                    model.Body = model.Body.Replace("'", "''");
                }

                string query = $@"
                INSERT INTO character (id, name, body, categoryid, alt, imageurl)
	 	                VALUES ({identity}, '{model.Name}', '{model.Body}', '{model.Category.ID}', '{model.Alt}', '{model.ImageURL}')
                ON CONFLICT (id) DO UPDATE 
                SET name = '{model.Name}',
                       body = '{model.Body}',
                       categoryid = '{model.Category.ID}',
                       alt = '{model.Alt ?? null}',
                       imageurl = '{model.ImageURL ?? null}'
                RETURNING *";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryFirstOrDefaultAsync<Character>(query);
                    res.Category = model.Category;
                    return res;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
