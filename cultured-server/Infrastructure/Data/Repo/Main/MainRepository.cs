using Dapper;
using cultured.server.Infrastructure.Models.Main;
using cultured.server.Infrasructure.Models.Helpers;
using cultured.server.Infrastructure.Models.Helpers;
using cultured.server.Infrastructure.Models.Culture;
using cultured.server.Infrastructure.Data.Interface.Culture;

namespace cultured.server.Infrastructure.Data.Repo.Main
{
    public class MainRepository : AppSettings, IMain
    {
        public async Task<bool> DeleteBackground(int ID)
        {
            try
            {
                string query = $@"
                DELETE FROM backgrounds t WHERE t.id = {ID};";

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

        public async Task<FilteredList<Backgrounds>> FilterBackgrounds(Filter filter)
        {
            try
            {
                var filterModel = new Backgrounds();
                filter.pageSize = 20;
                FilteredList<Backgrounds> request = new FilteredList<Backgrounds>()
                {
                    filter = filter,
                    filterModel = filterModel,
                };
                FilteredList<Backgrounds> result = new FilteredList<Backgrounds>();
                string query_count = $@"Select Count(t.id) from backgrounds t";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    string query = $@"
                    SELECT * FROM backgrounds t
                    ORDER BY t.id DESC 
                    OFFSET {request.filter.pager.StartIndex} ROWS
                    FETCH NEXT {request.filter.pageSize} ROWS ONLY";
                    result.data = await con.QueryAsync<Backgrounds>(query);
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

        public async Task<string> GetBackgroundImage()
        {
            try
            {
                string query = $@"SELECT * FROM backgrounds ORDER BY RANDOM() LIMIT 1";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryFirstOrDefaultAsync<Backgrounds>(query);
                    return res.ImageURL;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Character> GetCharacter(int? ID, string? Name)
        {
            try
            {
                string WhereClause = "";
                if (ID.HasValue)
                {
                    WhereClause = $@" WHERE id = {ID.Value}";
                }
                else
                {
                    WhereClause = $@" WHERE name ILIKE '%{Name}%'";
                }
                string query = $@"
                    SELECT * FROM character t
                    {WhereClause}";
                using (var con = GetConnection)
                {
                    var result = await con.QueryFirstOrDefaultAsync<Character>(query);
                    string cQuery = $@"
                    with recursive cat as (
                      select * from category c 
                      where c.id = {result.ID} union
                      select category.* from category 
                      join cat on cat.parentid = category.id
                    )
                    select * from cat order by id desc;";
                    result.Categories = await con.QueryAsync<Category>(cQuery);
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Backgrounds> ManageBackground(Backgrounds model)
        {
            try
            {
                dynamic identity = model.ID.HasValue ? model.ID.Value : "default";

                string query = $@"
                INSERT INTO backgrounds (id, imageurl)
	 	                VALUES ({identity}, '{model.ImageURL}')
                ON CONFLICT (id) DO UPDATE 
                SET imageurl = '{model.ImageURL}'
                RETURNING *";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryFirstOrDefaultAsync<Backgrounds>(query);
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
